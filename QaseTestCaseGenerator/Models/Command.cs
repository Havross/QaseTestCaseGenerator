using QaseTestCaseGenerator.Settings;
using QaseTestCaseGenerator.Static;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Models
{
    public class Command
    {
        #region Properties
        public required string CommandName { get; set; }
        public string? DetailedCommandName { get; set; }
        public required Func<string[], Task> CommandMethod { get; set; }
        public required string Description { get; set; }
        #endregion

        #region Public Methods
        public static async Task RunCommand(string commandName, string[] args)
        {
            var command = StaticObjects.commands.FirstOrDefault(c => c.CommandName == commandName);
            if (command == null)
            {
                AnsiConsole.MarkupLine($"[red]Unknown command '{commandName}'[/]");
                return;
            }
            await command.CommandMethod(args);
        }
        #endregion
    }
}
