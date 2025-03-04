using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Static;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Settings
{
    public class IOSettings
    {
        #region Public Methods
        /// <summary>
        /// Handles user input by selecting and executing commands asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task HandleInput() => await SelectAndExecuteCommand();
        /// <summary>
        /// Initializes the console settings, setting the output encoding to Unicode.
        /// </summary>
        public static void InitializeConsole() => Console.OutputEncoding = Encoding.Unicode;
        #endregion

        #region Private Methods
        /// <summary>
        /// Continuously prompts the user to select and execute a command from a list of available commands.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task SelectAndExecuteCommand()
        {
            while (true)
            {
                Console.Clear();
                var selectedCommand = AnsiConsole.Prompt(
                    new SelectionPrompt<Command>()
                        .Title("[blue]Select a command:[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select, press [green]Enter[/] to execute)[/]")
                        .UseConverter(cmd => cmd.CommandName ?? "COMMAND NAME MISSING!")
                        .AddChoices(StaticObjects.commands)
                );
                if (selectedCommand != null)
                {
                    Console.Clear(); 

                    try
                    {
                        await selectedCommand.CommandMethod(Array.Empty<string>());
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error, exception: [yellow]{ex.Message}[/][/]");
                    }

                    AnsiConsole.MarkupLine("\n[green]Press any key to return to the menu...[/]");
                    Console.ReadKey();
                }
            }
        }
        #endregion
    }
}
