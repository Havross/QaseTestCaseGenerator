using QaseTestCaseGenerator.Static;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Commands
{
    internal class ShowCommands
    {
        public static Action About()
        {
            return () =>
            {

                Console.Clear();

                AnsiConsole.Write(
                    new FigletText("Qase Test Case Generator")
                        .Centered()
                        .Color(Color.Blue));

                AnsiConsole.MarkupLine("[bold green]● Welcome to the OpenAI Test Case Generator CLI![/]\n");

                AnsiConsole.Write(
                    new Panel("[yellow]This tool helps you generate, manage, and export structured test cases using AI.[/]")
                        .Header("[blue]■ About[/]")
                        .Border(BoxBorder.Double)
                        .Expand()
                );

                AnsiConsole.MarkupLine("\n[bold cyan]Features:[/]");
                AnsiConsole.MarkupLine("- [green]●[/] Generate structured test cases from unstructured notes");
                AnsiConsole.MarkupLine("- [green]●[/] Manually input test case descriptions");
                AnsiConsole.MarkupLine("- [green]●[/] Export test cases to JSON");
                AnsiConsole.MarkupLine("- [green]●[/] Modify OpenAI settings (API keys, models, etc.)");
                AnsiConsole.MarkupLine("- [green]●[/] Choose OpenAI model (GPT-3.5 Turbo vs GPT-4o)");
                AnsiConsole.MarkupLine("- [green]●[/] Save & Load profiles with encryption");
                AnsiConsole.MarkupLine("- [green]●[/] Securely store API keys using AES encryption");
                AnsiConsole.MarkupLine("- [green]●[/] Interactive menu with arrow keys and ENTER\n");

                AnsiConsole.MarkupLine("[bold cyan]Navigation & Controls:[/]");
                AnsiConsole.MarkupLine("- [yellow]→[/] Use arrow keys (↑ ↓) to navigate");
                AnsiConsole.MarkupLine("- [green]●[/] Press ENTER to select");
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
                        "     • [yellow]JsessionIdCookie[/] (copy from your browser)\n" +
                        "     • [yellow]Adm2AuthCookie[/] (copy from your browser)\n\n" +
                        "[green]■ Generate Test Cases from Text:[/] Convert unstructured text into structured test cases using OpenAI.\n" +
                        "   - Requires an OpenAI API Key.\n\n" +
                        "[bold red]NOTE:[/] A default profile is available with an API key stored behind a password. It will eventually run out and will have to recharge (5$)."
                    )
                    .Border(BoxBorder.Rounded)
                    .Header("[bold blue]● Available Methods[/]")
                    .Expand()
                );

                AnsiConsole.MarkupLine("[bold green]Need Help?[/]");
                AnsiConsole.MarkupLine("stepan.havrlant@alza.cz [blue]If you have questions, you can contact me via email[/]");
                AnsiConsole.MarkupLine("https://platform.openai.com/docs [blue]Read OpenAI API Documentation[/]");

                AnsiConsole.MarkupLine("\n[bold green]Enjoy using the Qase Test Case Generator CLI![/]");
            };
        }

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
            };
        }

        public static Action ShowAllCommands()
        {
            return () =>
            {                
                AnsiConsole.MarkupLine("[blue]Available commands:[/]");
                foreach (var item in StaticObjects.commands)
                    AnsiConsole.MarkupLine($"[blue]\t -->   [white]{item.DetailedCommandName ?? item.CommandName ?? "[red]COMMAND NAME MISSING![/]"}[/] [green] | [/] [yellow]{item?.Description}[/][/]");
            };
        }

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
    }
}
