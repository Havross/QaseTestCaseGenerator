using QaseTestCaseGenerator.Commands;
using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Static;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Settings
{
    internal class AppSettings
    {
        public static void InitializeClients()
        {
            AnsiConsole.MarkupLine("[blue]Creating HttpClients....[/]");

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = StaticObjects.cookieContainer
            };

            StaticObjects.confluenceHttpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Set default headers
            StaticObjects.confluenceHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            StaticObjects.confluenceHttpClient.DefaultRequestHeaders.Add("accept-language", "cs");

            StaticObjects.openAiHttpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            StaticObjects.openAiHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            StaticObjects.qaseHttpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(20)
            };
            StaticObjects.qaseHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AnsiConsole.MarkupLine("[green]Finished HTTP client configurations[/]");
        }

        public static void InitializeCommands()
        {
            AnsiConsole.MarkupLine("[blue]Initializing commands....[/]");
            InitializeQaseCommands();
            InitializeShowCommands();
            InitializeFileCommands();
            InitializeSettingsCommands();
            AnsiConsole.MarkupLine("[green]Finished command initialization[/]");
        }

        public static async Task RunInterface()
        {
            string command = string.Empty;
            while (true)
            {
                try
                {
                    await IOSettings.HandleInput();
                }
                catch (Exception)
                {
                    continue;                    
                }
            }
        }
        public static void InitializeFileCommands()
        {
            StaticObjects.commands.Add(new Command { CommandName = "browse_saved_jsons", CommandMethod = args => Task.Run(() => FileCommands.ShowSavedTestCaseJsons().Invoke()), Description = "Displays files in TestCases in a menu" });
        }
        public static void PrepareOpenAiHttpClientForRequest()
        {
            if (StaticObjects.openAiHttpClient == null)
            {
                AnsiConsole.MarkupLine("[red]HttpClient is not initialized! Call InitializeClient() first.[/]");
                return;
            }

            if (string.IsNullOrEmpty(UserSettings.OpenAIApiKey))
            {
                AnsiConsole.Markup("[yellow]Enter the OpenAI API key: [/]");
                UserSettings.OpenAIApiKey = Console.ReadLine() ?? "";
            }
            var currentAuth = StaticObjects.openAiHttpClient.DefaultRequestHeaders.Authorization?.Parameter;
            if (currentAuth != UserSettings.OpenAIApiKey)
            {
                StaticObjects.openAiHttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", UserSettings.OpenAIApiKey);

                AnsiConsole.MarkupLine("[green]OpenAI API Key updated.[/]");
            }        
        }


        public static void PrepareQaseHttpClientForRequest()
        {
            if (StaticObjects.qaseHttpClient == null)
            {
                AnsiConsole.MarkupLine("[red]HttpClient is not initialized! Call InitializeClient() first.[/]");
                return;
            }

            if (string.IsNullOrEmpty(UserSettings.QaseApiToken))
            {
                AnsiConsole.Markup("[yellow]Enter the Qase API token: [/]");
                UserSettings.QaseApiToken = Console.ReadLine() ?? "";
            }

            if (StaticObjects.qaseHttpClient.DefaultRequestHeaders.Contains("Token"))
            {
                var existingToken = StaticObjects.qaseHttpClient.DefaultRequestHeaders.GetValues("Token").FirstOrDefault();
                if (existingToken != UserSettings.QaseApiToken)
                {
                    StaticObjects.qaseHttpClient.DefaultRequestHeaders.Remove("Token");
                    StaticObjects.qaseHttpClient.DefaultRequestHeaders.Add("Token", UserSettings.QaseApiToken);
                    AnsiConsole.MarkupLine("[green]Qase API Token updated.[/]");
                }
            }
            else
            {
                StaticObjects.qaseHttpClient.DefaultRequestHeaders.Add("Token", UserSettings.QaseApiToken);
            }
            if (!StaticObjects.qaseHttpClient.DefaultRequestHeaders.Accept.Any(h => h.MediaType == "application/json"))
            {
                StaticObjects.qaseHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    


        private static void InitializeQaseCommands()
        {
            StaticObjects.commands.Add(new Command
            {
                CommandName = "generate_testcases_from_confluence",
                CommandMethod = args => Task.Run(() => QaseCommands.GenerateTestCasesFromConfluence().Invoke()),
                Description = "Generates json file from confluence page, and allows user to send them into qase",
            });

            StaticObjects.commands.Add(new Command
            {
                CommandName = "generate_testcases_from_inserted_text",
                CommandMethod = args => Task.Run(() => QaseCommands.GenerateTestCasesFromManualyInsertedText().Invoke()),
                Description = "Generates json file from text user inserted, and allows user to send them into qase"
            });

            StaticObjects.commands.Add(new Command
            {
                CommandName = "send_file_to_qase",
                CommandMethod = args => Task.Run(() => QaseCommands.SendFileToQase().Invoke()),
                Description = "Sends json file to qase"
            });
        }

        private static void InitializeSettingsCommands()
        {
            StaticObjects.commands.Add(new Command { CommandName = "save_user_profile", CommandMethod = args => Task.Run(() => SettingsCommands.SaveUserProfile().Invoke()), Description = "Saves current settings into a password protected profile" });
            StaticObjects.commands.Add(new Command { CommandName = "load_user_profile", CommandMethod = args => Task.Run(() => SettingsCommands.LoadUserProfile().Invoke()), Description = "Loads and applies settings from a profile" });
            StaticObjects.commands.Add(new Command { CommandName = "change_qase_settings" , CommandMethod = args => Task.Run(() => SettingsCommands.ChangeQaseSettings().Invoke()), Description = "Change Qase settings" });
            StaticObjects.commands.Add(new Command { CommandName = "change_user_settings", CommandMethod = args => Task.Run(() => SettingsCommands.ChangeUserSettings().Invoke()), Description = "Change user settings, app wont function properly if settings are not set" });
            StaticObjects.commands.Add(new Command { CommandName = "delete_user_profile", CommandMethod = args => Task.Run(() => SettingsCommands.DeleteUserProfile().Invoke()), Description = "Deletes existing profile" });
            StaticObjects.commands.Add(new Command { CommandName = "exit", CommandMethod = args => Task.Run(() => SettingsCommands.Exit().Invoke()), Description = "Closes the app" });
        }
        
        private static void InitializeShowCommands()
        {
            StaticObjects.commands.Add(new Command { CommandName = "about", CommandMethod = args => Task.Run(() => ShowCommands.About().Invoke()), Description = "Shows information about the app" });
            StaticObjects.commands.Add(new Command { CommandName = "show_client_config", CommandMethod = args => Task.Run(() => ShowCommands.ShowClientConfig().Invoke()), Description = "Show httpclient setting" });
            StaticObjects.commands.Add(new Command { CommandName = "show_all_command_details", CommandMethod = args => Task.Run(() => ShowCommands.ShowAllCommands().Invoke()), Description = "Show all command details" });
            StaticObjects.commands.Add(new Command { CommandName = "show_user_profiles", CommandMethod = args => Task.Run(() => ShowCommands.ShowUserProfiles().Invoke()), Description = "Shows user profiles" });

        }


    }
}
