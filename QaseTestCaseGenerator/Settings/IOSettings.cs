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
        public static async Task HandleInput()
        {
            await SelectAndExecuteCommand();
        }
        public static void InitializeConsole()
        {
            Console.OutputEncoding = Encoding.Unicode;            
        }
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

                // Display the selected command in a panel
                AnsiConsole.Write(
                    new Panel($"[green]✔ Selected Command:[/] [yellow]{selectedCommand.CommandName}[/]")
                        .Border(BoxBorder.Heavy)
                        .Header("[green]▲ Command Selection[/]")
                        .Expand()
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


        private static (Command? commandToExecute, string[] commandArgs) ParseCommand(string input)
        {
            string[] inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = inputParts[0];
            string[] commandArgs = inputParts.Length > 1 ? inputParts[1..] : Array.Empty<string>();
            var commandToExecute = StaticObjects.commands.FirstOrDefault(c => c.CommandName == command) ?? null;
            return new(commandToExecute , commandArgs);
        }
    }
}
