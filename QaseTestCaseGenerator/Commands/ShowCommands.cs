using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Settings;
using QaseTestCaseGenerator.Static;
using Spectre.Console;

namespace QaseTestCaseGenerator.Commands
{
    internal class ShowCommands
    {
        #region Commands
        /// <summary>
        /// Displays information about the Qase Test Case Generator CLI.
        /// </summary>
        /// <returns>An action that displays the information.</returns>
        public static Action About()
        {
            return () =>
            {

                Console.Clear();

                AnsiConsole.Write(
                    new FigletText("Qase Test Case Generator")
                        .Centered()
                        .Color(Color.Blue));

                AnsiConsole.MarkupLine("[bold green]▲ Welcome to the OpenAI Test Case Generator CLI![/]\n");

                AnsiConsole.Write(
                    new Panel("[yellow]This tool helps you generate, manage, and export structured test cases using AI.[/]")
                        .Header("[blue]■ About[/]")
                        .Border(BoxBorder.Double)
                        .Expand()
                );

                AnsiConsole.MarkupLine("\n[bold cyan]Features:[/]");
                AnsiConsole.MarkupLine("- [green]▲[/] Generate structured test cases from unstructured notes");
                AnsiConsole.MarkupLine("- [green]▲[/] Manually input test case descriptions");
                AnsiConsole.MarkupLine("- [green]▲[/] Export test cases to JSON");
                AnsiConsole.MarkupLine("- [green]▲[/] Modify OpenAI settings (API keys, models, etc.)");
                AnsiConsole.MarkupLine("- [green]▲[/] Choose OpenAI model (GPT-3.5 Turbo vs GPT-4o)");
                AnsiConsole.MarkupLine("- [green]▲[/] Save & Load profiles with encryption");
                AnsiConsole.MarkupLine("- [green]▲[/] Securely store API keys using AES encryption");
                AnsiConsole.MarkupLine("- [green]▲[/] Interactive menu with arrow keys and ENTER\n");

                AnsiConsole.MarkupLine("[bold cyan]Navigation & Controls:[/]");
                AnsiConsole.MarkupLine("- [yellow]→[/] Use arrow keys (↑ ↓) to navigate");
                AnsiConsole.MarkupLine("- [green]▲[/] Press ENTER to select");
                AnsiConsole.MarkupLine("- [red]■[/] Press ESC to exit\n");

                AnsiConsole.MarkupLine("[bold red]Security Features:[/]");
                AnsiConsole.MarkupLine("- AES encryption for API keys");
                AnsiConsole.MarkupLine("- Password-protected user profiles");
                AnsiConsole.MarkupLine("- No sensitive data stored in plain text\n");

                AnsiConsole.Write(
                    new Panel(
                        "[bold red]Important Notices:[/]\n" +
                        "- Large requests (4,000+ tokens) may fail.\n" +
                        "- Ensure your API key is **correct** before generating test cases."
                    )
                    .Border(BoxBorder.Heavy)
                    .Header("[red]▲ Limitations & Known Issues[/]")
                    .Expand()
                );

                AnsiConsole.MarkupLine("\n[bold cyan]How to Start:[/]");
                AnsiConsole.MarkupLine("[yellow]1.[/] Run the application");
                AnsiConsole.MarkupLine("[yellow]2.[/] Load a profile via menu or enter settings interactively");
                AnsiConsole.MarkupLine("[yellow]3.[/] Follow instructions to generate, modify, or export test cases");

                AnsiConsole.Write(
                    new Panel(
                        "[bold cyan]There are three main methods available in this CLI:[/]\n\n" +
                        "[green]■ Import to Qase:[/] Import structured test cases into Qase using your Qase Access Token.\n\n" +
                        "[green]■ Generate Test Cases from Confluence:[/] Extract structured test cases from Confluence pages using your session cookies.\n" +
                        "   - Requires two cookies from Confluence DevTools:\n" +
                        "     \t[yellow]JsessionIdCookie[/] (copy from your browser)\n" +
                        "     \t[yellow]Adm2AuthCookie[/] (copy from your browser)\n\n" +
                        "[green]■ Generate Test Cases from Text:[/] Convert unstructured text into structured test cases using OpenAI.\n" +
                        "   - Requires an OpenAI API Key.\n\n"
                    )
                    .Border(BoxBorder.Rounded)
                    .Header("[bold blue]▲ Available Methods[/]")
                    .Expand()
                );

                AnsiConsole.MarkupLine("[bold green]Need Help?[/]");
                AnsiConsole.MarkupLine("stepan.havrlant@seznam.cz [blue]If you have questions, you can contact me via email[/]");
                AnsiConsole.MarkupLine("https://platform.openai.com/docs [blue]Read OpenAI API Documentation[/]");
                AnsiConsole.Write(
                    new Panel(
                        $"[bold cyan]Version {AppSettings.CurrentVersion}[/]\n"
                        + "[bold blue]Release notes: [/]\n"
                        + "[green]\t-[/] Test case browser fixes\n"
                        + "[green]\t-[/] Fixed bad parsing from HTML on generate from confluence\n"
                        + "[green]\t-[/] Adjusted About text\n"
                    )
                    .Border(BoxBorder.Heavy)
                    .Header("[green]▲ Version: [/]")
                    .Expand()
                );
                AnsiConsole.MarkupLine("\n[bold green]Enjoy using the Qase Test Case Generator CLI![/]");
            };
        }

        /// <summary>
        /// Displays the configuration of the HTTP clients.
        /// </summary>
        /// <returns>An action that displays the HTTP client configuration.</returns>
        public static Action ShowClientConfig()
        {
            return () =>
            {
                AnsiConsole.MarkupLine("\n[blue]confluence HTTP Client Configuration:[/]");
                if (StaticObjects.confluenceHttpClient != null)
                {
                    AnsiConsole.MarkupLine($"[white]\t--> Base URL: {StaticObjects.confluenceHttpClient.BaseAddress}[/]");
                    AnsiConsole.MarkupLine($"[white]\t--> Timeout: {StaticObjects.confluenceHttpClient.Timeout.TotalSeconds} seconds[/]");

                    var confluenceAuthHeader = StaticObjects.confluenceHttpClient.DefaultRequestHeaders.Authorization;
                    AnsiConsole.MarkupLine($"[white]\t--> Authorization: {(confluenceAuthHeader != null ? "[green]Cookie-based[/]" : "[yellow]No authorization configured[/]")}[/]");

                    if (StaticObjects.confluenceHttpClient.DefaultRequestHeaders.Any())
                    {
                        AnsiConsole.MarkupLine("[white]\t--> Default Headers:[/]");
                        foreach (var header in StaticObjects.confluenceHttpClient.DefaultRequestHeaders)
                        {
                            AnsiConsole.MarkupLine($"[cyan]\t\t- {header.Key}: {string.Join(", ", header.Value)}[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]\t--> No default headers configured[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]\t--> confluence client is not initialized![/]");
                }

                AnsiConsole.MarkupLine("\n[blue]openAI HTTP Client Configuration:[/]");
                if (StaticObjects.openAiHttpClient != null)
                {
                    AnsiConsole.MarkupLine($"[white]\t--> Timeout: {StaticObjects.openAiHttpClient.Timeout.TotalSeconds} seconds[/]");

                    var openAiAuthHeader = StaticObjects.openAiHttpClient.DefaultRequestHeaders.Authorization;
                    AnsiConsole.MarkupLine($"[white]\t--> Authorization: {(openAiAuthHeader != null ? "[green]Bearer token configured[/]" : "[yellow]No authorization configured[/]")}[/]");

                    if (StaticObjects.openAiHttpClient.DefaultRequestHeaders.Any())
                    {
                        AnsiConsole.MarkupLine("[white]\t--> Default Headers:[/]");
                        foreach (var header in StaticObjects.openAiHttpClient.DefaultRequestHeaders)
                        {
                            AnsiConsole.MarkupLine($"[cyan]\t\t- {header.Key}: {string.Join(", ", header.Value)}[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]\t--> No default headers configured[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]\t--> openAI client is not initialized![/]");
                }

                AnsiConsole.MarkupLine("\n[blue]qase HTTP Client Configuration:[/]");
                if (StaticObjects.openAiHttpClient != null)
                {
                    AnsiConsole.MarkupLine($"[white]\t--> Timeout: {StaticObjects.qaseHttpClient.Timeout.TotalSeconds} seconds[/]");

                    var qaseAuthHeader = StaticObjects.qaseHttpClient.DefaultRequestHeaders.Authorization;
                    AnsiConsole.MarkupLine($"[white]\t--> Authorization: {(qaseAuthHeader != null ? "[green]Bearer token configured[/]" : "[yellow]No authorization configured[/]")}[/]");

                    if (StaticObjects.qaseHttpClient.DefaultRequestHeaders.Any())
                    {
                        AnsiConsole.MarkupLine("[white]\t--> Default Headers:[/]");
                        foreach (var header in StaticObjects.qaseHttpClient.DefaultRequestHeaders)
                        {
                            AnsiConsole.MarkupLine($"[cyan]\t\t- {header.Key}: {string.Join(", ", header.Value)}[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]\t--> No default headers configured[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]\t--> openAI client is not initialized![/]");
                }
            };
        }

        /// <summary>
        /// Displays all available commands.
        /// </summary>
        /// <returns>An action that displays the available commands.</returns>
        public static Action ShowAllCommands()
        {
            return () =>
            {                
                AnsiConsole.MarkupLine("[blue]Available commands:[/]");
                foreach (var item in StaticObjects.commands)
                    AnsiConsole.MarkupLine($"[blue]\t -->   [white]{item.DetailedCommandName ?? item.CommandName ?? "[red]COMMAND NAME MISSING![/]"}[/] [green] | [/] [yellow]{item?.Description}[/][/]");
            };
        }

        /// <summary>
        /// Displays the available prompt templates.
        /// </summary>
        /// <returns>An action that display available prompt templates</returns>
        public static Action ShowPromptTemplates()
        {
            return () =>
            {
                var promptSelected = AnsiConsole.Prompt(
                    new SelectionPrompt<PromptType>()
                        .Title($"[yellow]Select prompt template you want to view[/]")
                        .AddChoices(OpenAISettings.OpenAIPrompts.Keys));
                AnsiConsole.Write(new Panel($"[yellow]Selected prompt template: \n[/]{OpenAISettings.OpenAIPrompts[promptSelected].PromptTemplate}").Expand());
            };
        }

        /// <summary>
        /// Displays the selected prompt.
        /// </summary>
        /// <returns>An action that display selected prompt</returns>
        public static Action ShowCurrentPrompt()
        {
            return () =>
            {
                AnsiConsole.Write(new Panel($"[yellow]Selected prompt [fuchsia]{UserSettings.PromptSettings.Type}[/]: \n[/]{UserSettings.PromptSettings.Prompt}").Expand());
            };
        }


        /// <summary>
        /// Displays the available user profiles.
        /// </summary>
        /// <returns>An action that displays the user profiles.</returns>
        public static Action ShowUserProfiles()
        {
            return () =>
            {
                var profiles = SettingsCommands.GetAvailableProfiles();
                if(profiles.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No profiles found![/]");
                    return;
                }
                foreach (var item in profiles)
                    AnsiConsole.MarkupLine($"\t {item}");
            };
        }
        #endregion
    }
}
