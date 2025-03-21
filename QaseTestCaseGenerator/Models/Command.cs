﻿using QaseTestCaseGenerator.Static;
using Spectre.Console;


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
        /// <summary>
        /// Runs the specified command by name with the given arguments.
        /// </summary>
        /// <param name="commandName">The name of the command to run.</param>
        /// <param name="args">The arguments to pass to the command method.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
