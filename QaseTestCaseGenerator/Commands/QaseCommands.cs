using HtmlAgilityPack;
using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Settings;
using QaseTestCaseGenerator.Static;
using RestSharp;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace QaseTestCaseGenerator.Commands
{
    public class QaseCommands
    {
        #region Commands
        public static Func<Task> GenerateTestCasesFromConfluence()
        {
            return async () =>
            {                
                string enteredUrl = AnsiConsole.Ask<string>("Enter URL of the Confluence page> ") ?? "";
                if (!Uri.TryCreate(enteredUrl, UriKind.Absolute, out Uri? baseUri))
                {
                    AnsiConsole.MarkupLine("[red]Invalid URL entered. Please restart and enter a correct Confluence URL.[/]");
                    return;
                }

                if (string.IsNullOrEmpty(UserSettings.Adm2AuthCookie))
                {

                    AnsiConsole.Markup("[yellow]Enter the [fuchsia]session cookie (adm2.auth.dev)[/]: [/]");
                    UserSettings.Adm2AuthCookie = AnsiConsole.Ask<string>("> ") ?? "";
                }
                if (string.IsNullOrEmpty(UserSettings.JsessionIdCookie))
                {
                    AnsiConsole.Markup("[yellow]Enter [fuchsia]JSESSIONID[/] cookie: [/]");
                    UserSettings.JsessionIdCookie = AnsiConsole.Ask<string>("> ") ?? "";
                }
                StaticObjects.cookieContainer.Add(baseUri, new Cookie("adm2.auth.dev", UserSettings.Adm2AuthCookie));
                StaticObjects.cookieContainer.Add(baseUri, new Cookie("JSESSIONID", UserSettings.JsessionIdCookie));
                AnsiConsole.MarkupLine("[blue]Trying to retrieve the page....[/]");
                var response = await StaticObjects.confluenceHttpClient.GetAsync(enteredUrl);
                string pageHtml = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode || pageHtml.Contains("<title>Přihlásit se - Confluence </title>"))
                {
                    AnsiConsole.MarkupLine($"[red]Error, page did not load correctly.[/]");
                    return;
                }
                AnsiConsole.MarkupLine("[green]Page successfully retrieved![/]");
                AnsiConsole.MarkupLine($"[grey]Page HTML preview:\n[/] {pageHtml.Substring(0, Math.Min(500, pageHtml.Length))}...");
                AnsiConsole.MarkupLine("[blue]Running the extractor...[/]");
                string extractedText = ExtractTextFromHtml(pageHtml);
                AnsiConsole.MarkupLine($"[green]Text extracted successfully![/]");
                AnsiConsole.MarkupLine($"[grey]Extracted text preview:\n[/] {extractedText.Substring(0, Math.Min(500, extractedText.Length))}...");
                var showFullTextChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Would you like to see the full text?[/]")
                        .AddChoices("Yes", "No")
                );
                if (showFullTextChoice == "Yes")
                {
                    AnsiConsole.MarkupLine($"[grey]Full text:[/]");
                    Console.WriteLine(extractedText);
                }
                await GenerateAndExportTests(extractedText);
            };
        }

        public static Func<Task> GenerateTestCasesFromManualyInsertedText()
        {
            return async () =>
            {
                AnsiConsole.MarkupLine("[yellow]Please enter the text you want to generate test cases from. [/]");
                AnsiConsole.MarkupLine("[grey](Type 'END' on a new line and press Enter to finish input.)[/]");
                StringBuilder enteredText = new StringBuilder();
                string? line;

                while (true)
                {
                    line = Console.ReadLine();

                    if (line?.Trim().ToUpper() == "END") 
                        break;

                    enteredText.AppendLine(line);
                }
                await GenerateAndExportTests(enteredText.ToString());
            };
        }

        public static Func<Task> SendFileToQase(string selectedFile)
        {
            return async () =>
            {
                await ImportDataIntoQase(selectedFile);
            };
        }
        public static Func<Task> SendFileToQase()
        {
            return async () =>
            {
                var testCases = FileCommands.GetSavedTestCaseJsons();
                string selectedFile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select the file you want to send to Qase:[/]")
                        .PageSize(10)
                        .AddChoices(testCases)
                );
                await ImportDataIntoQase(selectedFile);
            };
        }
        #endregion


        #region Methods
        private static async Task GenerateAndExportTests(string notes)
        {
            OpenAISettings.Notes = notes;
            List<TestCase> testCases = await GenerateTestCasesWithAI();
            if(testCases.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Error test case generation returned '0' cases![/]");
                return;
            }
            AnsiConsole.MarkupLine($"[green]Generated '{testCases.Count}' test cases![/]");

            AnsiConsole.MarkupLine("[blue]Starting enriching process...[/]");
            foreach (var testCase in testCases)            
                testCase.Enrich();
            AnsiConsole.MarkupLine("[green]Enriching completed successfully![/]");

            var exportChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Would you like to export into JSON (you need to export to send data to qase)?[/]")
                    .AddChoices("Yes", "No")
            );

            if (exportChoice == "Yes")
            {
                string json = JsonSerializer.Serialize(testCases, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                string fileName = AnsiConsole.Ask<string>("[yellow]Enter filename (without extension):[/]");
                string filePath = $"{UserSettings.UserTestCaseDirectory}/{fileName}.json";
                if (!Directory.Exists(UserSettings.UserTestCaseDirectory))
                {
                    Directory.CreateDirectory(UserSettings.UserTestCaseDirectory);
                }
                await File.WriteAllTextAsync(filePath, json);
                AnsiConsole.MarkupLine($"[green]Test cases exported successfully to '{filePath}'![/]");

                var importChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[yellow]Would you like to send the data into qase?[/]")
                        .AddChoices("Yes", "No"));                
                if (importChoice == "Yes")                
                    await ImportDataIntoQase($"{fileName}.json");                
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Export skipped.[/]");
                return;
            }
            AnsiConsole.MarkupLine("[green]Operation completed successfully![/]");
        }

        private static async Task ImportDataIntoQase(string file)
        {
            string filePath = $"{UserSettings.UserTestCaseDirectory}/{file}";
            if (!File.Exists(filePath))
            {
                AnsiConsole.MarkupLine($"[red]File '{filePath}' does not exist.[/]");
                return;
            }
            string json = await File.ReadAllTextAsync(filePath);
            AnsiConsole.MarkupLine("[yellow]Wrapping test case data in [green]'cases'[/] object[/]");
            var testCases = new List<TestCase>();
            try
            {
                testCases = JsonSerializer.Deserialize<List<TestCase>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                            ?? new List<TestCase>();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Failed to parse JSON: {ex.Message}[/]");
                return;
            }
            var requestBody = new
            {
                cases = testCases
            };
            string finalJson = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
            AppSettings.PrepareQaseHttpClientForRequest();
            var content = new StringContent(finalJson, Encoding.UTF8, "application/json");
            try
            {
                string selectedQaseProject = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Select the project you want to send the test cases to:[/]")
                            .PageSize(10)
                            .AddChoices(QaseSettings.Trade, QaseSettings.Admin)
                    );
                AnsiConsole.MarkupLine($"[yellow]Selected project: [green]{selectedQaseProject}[/][/]");

                AnsiConsole.Write(
                    new Panel(
                        "[bold red]WARNING![/]\n" +
                        "Make sure you have the **correct SuiteID** in the JSON.\n\n" +
                        "[red]If the wrong SuiteID is selected, you might import a LOT of tests into the wrong place![/]\n\n" +
                        "[bold yellow]Double-check before continuing![/]"                    )
                    .Header("[red]IMPORTANT WARNING[/]")
                    .Border(BoxBorder.Heavy)
                    .Expand()
                );
                
                var confirmation = AnsiConsole.Confirm("[bold green]Are you sure you want to continue?[/]");

                if (!confirmation)
                {
                    AnsiConsole.MarkupLine("[red]Import cancelled by user.[/]");
                    return;
                }
                AnsiConsole.MarkupLine("[blue]Sending test cases into qase...[/]");

                string finalUrl = $"{QaseSettings.QaseBaseURL}/{selectedQaseProject}/bulk";
                AnsiConsole.MarkupLine($"[grey]Sending to: {finalUrl}[/]");
                bool success = await AnsiConsole.Status()
                    .StartAsync("[yellow]Waiting...[/]", async ctx =>
                    {
                        HttpResponseMessage response = await StaticObjects.qaseHttpClient.PostAsync($"{QaseSettings.QaseBaseURL}/{selectedQaseProject}/bulk", content);

                        if (!response.IsSuccessStatusCode)
                        {
                            AnsiConsole.MarkupLine($"[red]Error: {response.StatusCode}[/]");
                            string errorDetails = await response.Content.ReadAsStringAsync();
                            AnsiConsole.MarkupLine($"[red]Details: {errorDetails}[/]");
                            return false;
                        }
                        return true;
                    });
                if (!success)
                {
                    AnsiConsole.MarkupLine($"[red]Error has occured while sending request[/]");
                    return;
                }
                AnsiConsole.Write(
                    new Panel("[green]▲ Test cases successfully imported into Qase![/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Green)
                        .Expand()
                );
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]There has been an error while sending request: {ex.Message}[/]");
                return;
            }
        }
        private static string ExtractTextFromHtml(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Remove all script, style, and unnecessary elements
            foreach (var script in doc.DocumentNode.SelectNodes("//script|//style") ?? Enumerable.Empty<HtmlNode>())
                script.Remove();

            // Extract all text from paragraphs and headings
            var extractedText = new StringBuilder();
            foreach (var node in doc.DocumentNode.SelectNodes("//p | //h1 | //h2 | //h3 | //h4 | //h5 | //h6") ?? Enumerable.Empty<HtmlNode>())
            {
                extractedText.AppendLine(HtmlEntity.DeEntitize(node.InnerText.Trim()));
            }

            return extractedText.ToString();
        }
        private static List<TestCase> ParseAiResponseToTestCases(string aiResponse)
        {
            List<TestCase> testCases = new List<TestCase>();
            string[] lines = aiResponse.Split("\n");

            TestCase? currentTestCase = null;
            List<TestStep> steps = new List<TestStep>();

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("Title:"))
                {
                    if (currentTestCase != null)
                    {
                        currentTestCase.Steps = steps.ToArray();
                        testCases.Add(currentTestCase);
                    }

                    currentTestCase = new TestCase
                    {
                        Title = trimmedLine.Replace("Title:", "").Trim(),
                        Description = "",
                        Preconditions = "None",
                        Steps = Array.Empty<TestStep>()
                    };

                    steps = new List<TestStep>();
                }
                else if (trimmedLine.StartsWith("Description:") && currentTestCase != null)
                {
                    currentTestCase.Description = trimmedLine.Replace("Description:", "").Trim();
                }
                else if (trimmedLine.StartsWith("Preconditions:") && currentTestCase != null)
                {
                    string preconditions = trimmedLine.Replace("Preconditions:", "").Trim();
                    currentTestCase.Preconditions = string.IsNullOrWhiteSpace(preconditions) ? "None" : preconditions;
                }
                else if (Regex.IsMatch(trimmedLine, @"^\d+\.")) 
                {
                    string stepText = Regex.Replace(trimmedLine, @"^\d+\.", "").Trim();
                    steps.Add(new TestStep { Action = stepText, ExpectedResult = "[Missing Expected Result]" });
                }
                else if (trimmedLine.StartsWith("Expected:") && steps.Count > 0)
                {
                    steps[^1].ExpectedResult = trimmedLine.Replace("Expected:", "").Trim();
                }
            }
            if (currentTestCase != null)
            {
                currentTestCase.Steps = steps.ToArray();
                testCases.Add(currentTestCase);
            }
            return testCases;
        }
        private static async Task<List<TestCase>> GenerateTestCasesWithAI()
        {
            var requestBody = OpenAISettings.GetRequestBody();
            AppSettings.PrepareOpenAiHttpClientForRequest();
            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            try
            {
                AnsiConsole.MarkupLine("[blue]Generating test cases with AI...[/]");
                string responseBody = await AnsiConsole.Status()
                    .StartAsync("[yellow]Waiting for AI response...[/]", async ctx =>
                    {
                        HttpResponseMessage response = await StaticObjects.openAiHttpClient.PostAsync(OpenAISettings.URL, content);

                        if (!response.IsSuccessStatusCode)
                        {
                            AnsiConsole.MarkupLine($"[red]Error: {response.StatusCode}[/]");
                            string errorDetails = await response.Content.ReadAsStringAsync();
                            AnsiConsole.MarkupLine($"[red]Details: {errorDetails}[/]");
                            return string.Empty;
                        }
                        return await response.Content.ReadAsStringAsync();
                    });
                if (string.IsNullOrEmpty(responseBody))
                {
                    AnsiConsole.MarkupLine("[yellow]No response received from AI.[/]");
                    return new();
                }
                using JsonDocument jsonResponse = JsonDocument.Parse(responseBody);
                string aiMessage = jsonResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "[No AI response]";
                if(string.IsNullOrEmpty(aiMessage) || aiMessage.Length < 10)
                {
                    AnsiConsole.MarkupLine("[red]Nothing was succesfully parsed from response.[/]");
                    return new();
                }
                AnsiConsole.MarkupLine("[green]AI test case generation completed successfully![/]");
                AnsiConsole.MarkupLine($"[grey]AI response preview:\n[/] {aiMessage.Substring(0, Math.Min(500, aiMessage.Length))}...");
                AnsiConsole.MarkupLine("[blue]Trying to parse response into test cases...[/]");
                List<TestCase> testCases = ParseAiResponseToTestCases(aiMessage);
                AnsiConsole.MarkupLine("[green]Test cases successfully parsed![/]");
                return testCases;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]There has been an erorr: {ex.Message}[/]");
                return new();
            }
        }
        #endregion

    }
}
