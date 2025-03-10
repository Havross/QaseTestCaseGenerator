﻿using QaseTestCaseGenerator.Models;
using Spectre.Console;
using System.Text.Json;

namespace QaseTestCaseGenerator.Commands
{
    public class FileCommands
    {
        #region Commands
        /// <summary>
        /// Displays a list of saved test case JSON files and allows the user to perform actions on them.
        /// </summary>
        /// <returns>A function that displays and manages saved test case JSON files.</returns>
        public static Func<Task> ShowSavedTestCaseJsons()
        {
            return async () =>
            {

                var files = GetSavedTestCaseJsons();
                if (files == null || files.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No test case files available![/]");
                    return;
                }
                var selectedFile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]Select a test case file:[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select, press [green]Enter[/] to open)[/]")
                        .AddChoices(files)
                );

                AnsiConsole.Write(
                    new Panel($"[green]You selected:[/] [yellow]{selectedFile}[/]")
                        .Border(BoxBorder.Heavy)
                        .Header("[green]▲ Selected File[/]")
                        .Expand()
                );

                var filePath = Path.Combine("TestCases", selectedFile);
                var content = File.ReadAllText(filePath);

                var selectedAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]What do you want to do?[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select, press [green]Enter[/] to execute)[/]")
                        .AddChoices(new List<string> { "View File Content", "Delete File", "Send file to qase", "Show file in TestCase viewer", "Return" }));

                switch (selectedAction)
                {
                    case "View File Content":
                        ShowFileContent(selectedFile, content);
                        return;

                    case "Delete File":
                        DeleteFile(selectedFile);
                        return;

                    case "Send file to qase":
                        await QaseCommands.SendFileToQase(selectedFile).Invoke();
                        return;

                    case "Show file in TestCase viewer":
                        ShowFileInTestCaseViewer(selectedFile, content);
                        return;

                    case "Return":
                        return;

                    default:
                        return;
                }
            };
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Displays the content of the selected file.
        /// </summary>
        /// <param name="selectedFile">The selected file.</param>
        /// <param name="content">The content of the file.</param>
        private static void ShowFileContent(string selectedFile, string content)
        {
            AnsiConsole.MarkupLine($"[cyan]File Content of {selectedFile}:[/]\n");
            AnsiConsole.WriteLine(content);
        }

        /// <summary>
        /// Deletes the selected file.
        /// </summary>
        /// <param name="selectedFile">The selected file.</param>
        private static void DeleteFile(string selectedFile)
        {
            string delete = AnsiConsole.Ask<string>("[red]Are you sure you want to delete this file? [/]([green]y[/]/[red]n[/])", "y");
            if (delete != "y")
            {
                AnsiConsole.MarkupLine("[yellow]File deletion canceled![/]");
                return;
            }
            var filePath = Path.Combine("TestCases", selectedFile);
            File.Delete(filePath);
            AnsiConsole.MarkupLine($"[green]File {selectedFile} deleted successfully![/]");
        }

        /// <summary>
        /// Displays the selected file in the test case viewer.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <param name="content">The content of the file.</param>
        private static void ShowFileInTestCaseViewer(string filePath, string content)
        {
            AnsiConsole.MarkupLine("[cyan]▲ Opening TestCase Viewer...[/]");

            List<TestCase>? testCases;
            try
            {
                testCases = JsonSerializer.Deserialize<List<TestCase>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (testCases == null || testCases.Count == 0)
                {
                    throw new Exception("The test case file is empty or improperly formatted.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: Failed to parse JSON. Returning to file selection.[/] \n[grey]{ex.Message}[/]");
                return;
            }

            while (true)
            {
                Console.Clear();

                if (testCases.FirstOrDefault(tc => tc.Title == "Return") == null)
                    testCases.Add(new TestCase { Title = "Return", Description = "", Steps = Array.Empty<TestStep>() });
                if (testCases.FirstOrDefault(tc => tc.Title == "Save & Exit") == null)
                    testCases.Add(new TestCase { Title = "Save & Exit", Description = "", Steps = Array.Empty<TestStep>() });
                var selectedTestCase = AnsiConsole.Prompt(
                    new SelectionPrompt<TestCase>()
                        .Title("[blue]Select a test case to view:[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select, press [green]Enter[/] to view/edit)[/]")
                        .UseConverter(tc => tc.Title)
                        .AddChoices(testCases)
                );
                if (selectedTestCase.Title == "Return")
                {
                    AnsiConsole.MarkupLine("[green]Returning to file selection...[/]");
                    return;
                }
                if (selectedTestCase.Title == "Save & Exit")
                {
                    Save(testCases, filePath);
                    return;
                }
                // Display Selected Test Case
                Console.Clear();
                AnsiConsole.Write(
                    new Panel($"[bold green]▲ Viewing Test Case[/]\n[blue]File:[/] [yellow]{Path.GetFileName(filePath)}[/]")
                        .Border(BoxBorder.Heavy)
                        .Expand()
                );

                AnsiConsole.MarkupLine($"[bold cyan]Title:[/] {selectedTestCase.Title}");
                AnsiConsole.MarkupLine($"[bold cyan]Description:[/] {selectedTestCase.Description}");
                AnsiConsole.MarkupLine($"[bold cyan]Preconditions:[/] {selectedTestCase.Preconditions ?? "[grey]None[/]"}");
                AnsiConsole.MarkupLine($"[bold cyan]Priority:[/] {selectedTestCase.Priority?.ToString() ?? "[grey]Not Set[/]"}");
                AnsiConsole.MarkupLine($"[bold cyan]Suite ID:[/] {selectedTestCase.SuiteId?.ToString() ?? "[grey]Not Set[/]"}");
                AnsiConsole.MarkupLine("\n[bold cyan]Test Steps:[/]");
                foreach (var step in selectedTestCase.Steps.Select((s, index) => $"{index + 1}. {s.Action} → {s.ExpectedResult}"))
                {
                    Console.WriteLine($"  {step}");
                }

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]What would you like to do?[/]")
                        .AddChoices("Edit Title", "Edit Description", "Edit Preconditions", "Edit Priority", "Edit Suite ID", "Delete TestCase", "Cancel")
                );

                switch (action)
                {
                    case "Edit Title":
                        selectedTestCase.Title = AnsiConsole.Ask<string>("[yellow]Enter new Title:[/]", selectedTestCase.Title);
                        break;

                    case "Edit Description":
                        selectedTestCase.Description = AnsiConsole.Ask<string>("[yellow]Enter new Description:[/]", selectedTestCase.Description);
                        break;

                    case "Edit Preconditions":
                        selectedTestCase.Preconditions = AnsiConsole.Ask<string>("[yellow]Enter new Preconditions (or leave blank):[/]", selectedTestCase.Preconditions ?? "");
                        break;

                    case "Edit Priority":
                        selectedTestCase.Priority = AnsiConsole.Ask<int?>("[yellow]Enter new Priority (or leave blank):[/]", selectedTestCase.Priority);
                        break;

                    case "Edit Suite ID":
                        selectedTestCase.SuiteId = AnsiConsole.Ask<int?>("[yellow]Enter new Suite ID (or leave blank):[/]", selectedTestCase.SuiteId);
                        break;

                    case "Delete TestCase":
                        testCases.Remove(selectedTestCase);
                        break;

                    case "Cancel":
                        break;
                }
            }
        }

        /// <summary>
        /// Saves the test cases to the specified file.
        /// </summary>
        /// <param name="testCases">The list of test cases to save.</param>
        /// <param name="filePath">The path of the file to save to.</param>
        private static void Save(List<TestCase> testCases, string filePath)
        {
            bool isValid = true;
            var returnTestCase = testCases.FirstOrDefault(tc => tc.Title == "Return");
            var saveExitTestCase = testCases.FirstOrDefault(tc => tc.Title == "Save & Exit");
            if (returnTestCase != null)
                testCases.Remove(returnTestCase);
            if (saveExitTestCase != null)
                testCases.Remove(saveExitTestCase
                    );

            foreach (var item in testCases)
                isValid = ValidateTestCase(item);
            if (!isValid)
                return;
            var json = JsonSerializer.Serialize(testCases, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            AnsiConsole.MarkupLine("[green]Test case saved successfully![/]");
            AnsiConsole.MarkupLine("[red]Warning: Modifying JSON may result in incompatibility with Qase.[/]");
            AnsiConsole.MarkupLine("[red]Invalid data! Fix errors before saving.[/]");
        }

        /// <summary>
        /// Validates the specified test case.
        /// </summary>
        /// <param name="testCase">The test case to validate.</param>
        /// <returns>True if the test case is valid; otherwise, false.</returns>
        private static bool ValidateTestCase(TestCase testCase)
        {
            if (string.IsNullOrWhiteSpace(testCase.Title))
            {
                AnsiConsole.MarkupLine($"[red]Title cannot be empty! Description: {testCase.Description}[/]");
                return false;
            }
            if (string.IsNullOrWhiteSpace(testCase.Description))
            {
                AnsiConsole.MarkupLine($"[red]Description cannot be empty! Title: {testCase.Title}[/]");
                return false;
            }
            if (testCase.Steps == null || testCase.Steps.Length == 0)
            {
                AnsiConsole.MarkupLine($"[red]Test case must have at least one step! Title: {testCase.Title}[/]");
                return false;
            }
            return true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the list of saved test case JSON files.
        /// </summary>
        /// <returns>A list of saved test case JSON files.</returns>
        public static List<string> GetSavedTestCaseJsons()
        {
            AnsiConsole.MarkupLine("[cyan]Saved Test Cases:[/]\n");

            if (!Directory.Exists("TestCases"))
            {
                AnsiConsole.MarkupLine("[red]No test case files found![/]");
                return new();
            }
            List<string> files = Directory.GetFiles("TestCases")
                                 .Select(Path.GetFileName)
                                 .ToList();
            return files;

        }
        #endregion
    }
}
