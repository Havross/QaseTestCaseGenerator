using QaseTestCaseGenerator.Models;
using QaseTestCaseGenerator.Settings;
using Spectre.Console;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace QaseTestCaseGenerator.Commands
{
    internal class SettingsCommands
    {
        #region Commands
        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <returns>An action that exits the application.</returns>
        public static Action Exit()
        {
            return () =>
            {
                Environment.Exit(0);
            };
        }

        /// <summary>
        /// Changes the Qase settings.
        /// </summary>
        /// <returns>An action that changes the Qase settings.</returns>
        public static Action ChangeQaseSettings()
        {
            return () =>
            {
                var settingsSelected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Select settings you want to change[/]")
                    .AddChoices(
                  $"Postconditions"
                , $"Status"
                , $"Priority"
                , $"Severity"
                , $"Behavior"
                , $"Type"
                , $"Layer"
                , $"IsFlaky"
                , $"AuthorId"
                , $"SuiteId"
                , $"MilestoneId"
                , $"Automation"
                , $"CreatedAt"
                , $"UpdatedAt"
                , $"Return"));
                switch (settingsSelected)
                {
                    case "Status":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Status}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Status = int.Parse(newValue);
                            break;
                        }
                    case "Priority":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Priority}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Priority = int.Parse(newValue);
                            break;
                        }
                    case "Severity":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Severity}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Severity = int.Parse(newValue);
                            break;
                        }
                    case "Behavior":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Behavior}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Behavior = int.Parse(newValue);
                            break;
                        }
                    case "Type":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Type}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Type = int.Parse(newValue);
                            break;
                        }
                    case "Layer":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Layer}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Layer = int.Parse(newValue);
                            break;
                        }
                    case "IsFlaky":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.IsFlaky}'\nEnter new value (true/false) or type 'return' to cancel> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.IsFlaky = bool.Parse(newValue);
                            break;
                        }
                    case "AuthorId":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.AuthorId}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.AuthorId = int.Parse(newValue);
                            break;
                        }
                    case "SuiteId":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.SuiteId}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.SuiteId = int.Parse(newValue);
                            break;
                        }
                    case "MilestoneId":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.MilestoneId}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.MilestoneId = int.Parse(newValue);
                            break;
                        }
                    case "Automation":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.Automation}'\nEnter new value (or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.Automation = int.Parse(newValue);
                            break;
                        }
                    case "CreatedAt":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.CreatedAt}'\nEnter new value (YYYY-MM-DD or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.CreatedAt = DateTime.Parse(newValue);
                            break;
                        }
                    case "UpdatedAt":
                        {
                            string newValue = AnsiConsole.Ask<string>($"Current value is '{TestCaseSettings.UpdatedAt}'\nEnter new value (YYYY-MM-DD or type 'return' to cancel)> ");
                            if (newValue.ToLower() != "return") TestCaseSettings.UpdatedAt = DateTime.Parse(newValue);
                            break;
                        }
                    case "Return":
                        return;
                }

                AnsiConsole.MarkupLine("[green]Qase changed successfully![/]");
            };
        }

        /// <summary>
        /// Changes the user settings.
        /// </summary>
        /// <returns>An action that changes the user settings.</returns>
        public static Action ChangeUserSettings()
        {
            return () =>
            {
                var settingsSelected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Select settings you want to change[/]")
                    .AddChoices(
                        "Adm2AuthCookie"
                        , "JsessionIdCookie"
                        , "OpenAIApiKey"
                        , "UserTestCaseDirectory"
                        , "QaseApiToken"
                        , "OpenAIModel"
                        , "Return"));
                switch (settingsSelected)
                {
                    case "Adm2AuthCookie":
                        {
                            string newValue = AnsiConsole.Ask<string>($"[yellow]Current {nameof(UserSettings.Adm2AuthCookie)} is [fuchsia]'{UserSettings.Adm2AuthCookie}'[/]\nEnter new value (or type [blue]'return'[/] to cancel)> [/]");
                            if (newValue.ToLower() != "return") UserSettings.Adm2AuthCookie = newValue;
                            break;
                        }
                    case "JsessionIdCookie":
                        {
                            string newValue = AnsiConsole.Ask<string>($"[yellow]Current JsessionIdCookie is [fuchsia]'{UserSettings.JsessionIdCookie}'[/]\nEnter new value (or type [blue]'return'[/] to cancel)> [/]");
                            if (newValue.ToLower() != "return") UserSettings.JsessionIdCookie = newValue;
                            break;
                        }
                    case "OpenAIApiKey":
                        {
                            string newValue = AnsiConsole.Ask<string>($"[yellow]Current OpenAIApiKey is [fuchsia]'{UserSettings.OpenAIApiKey}'[/]\nEnter new value (or type [blue]'return'[/] to cancel)> [/]");
                            if (newValue.ToLower() != "return") UserSettings.OpenAIApiKey = newValue;
                            break;
                        }
                    case "UserTestCaseDirectory":
                        {
                            string newValue = AnsiConsole.Ask<string>($"[yellow]Current UserTestCaseDirectory is [fuchsia]'{UserSettings.UserTestCaseDirectory}'[/]\nEnter new value (or type [blue]'return'[/] to cancel)> [/]");
                            if (newValue.ToLower() != "return") UserSettings.UserTestCaseDirectory = newValue;
                            break;
                        }
                    case "QaseApiToken":
                        {
                            string newValue = AnsiConsole.Ask<string>($"[yellow]Current QaseApiToken is [fuchsia]'{UserSettings.QaseApiToken}'[/]\nEnter new value (or type [blue]'return'[/] to cancel)> [/]");
                            if (newValue.ToLower() != "return") UserSettings.QaseApiToken = newValue;
                            break;
                        }
                    case "OpenAIModel":
                        {
                            AnsiConsole.MarkupLine($"[yellow]Current OpenAI model: [fuchsia]'{UserSettings.OpenAIModel}'[/][/]");
                            var selectedModel = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("[yellow]Which OpenAI model do you want to use?[/]")
                                    .AddChoices(
                                        "GPT-3.5 Turbo (Cheap) - $0.002 per 1K tokens",
                                        "GPT-4o (Expensive) - $0.01 per 1K tokens",
                                        "Return")
                            );

                            switch (selectedModel)
                            {
                                case "GPT-3.5 Turbo (Cheap) - $0.002 per 1K tokens":
                                    UserSettings.OpenAIModel = "gpt-3.5-turbo";
                                    break;

                                case "GPT-4o (Expensive) - $0.01 per 1K tokens":
                                    UserSettings.OpenAIModel = "gpt-4o";
                                    break;

                                case "Return":
                                    return;
                            }
                            AnsiConsole.MarkupLine($"[green]OpenAI model set to: {UserSettings.OpenAIModel}[/]");
                            break;
                        }
                    case "Return":
                        return;
                    default:
                        return;
                }

                AnsiConsole.MarkupLine("[green]User settings saved successfully![/]");
            };
        }

        /// <summary>
        /// Saves the current user settings to a profile.
        /// </summary>
        /// <returns>An action that saves the user profile.</returns>
        public static Action SaveUserProfile()
        {
            return () =>
            {
                string profileName = AnsiConsole.Ask<string>("[yellow]Enter a name for the profile> [/]");
                string password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter a password to encrypt this profile> [/]").Secret());

                string filePath = Path.Combine(UserSettings.ProfileDirectory, $"{profileName}.dat");
                UserSettingsDto userSettings = UserSettingsDto.FromUserSettings();
                string json = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                byte[] encryptedData = EncryptData(json, password);
                if (!Directory.Exists(UserSettings.ProfileDirectory))
                {
                    Directory.CreateDirectory(UserSettings.ProfileDirectory);
                }
                File.WriteAllBytes(filePath, encryptedData);

                AnsiConsole.MarkupLine($"[green]Profile '{profileName}' saved successfully![/]");
            };
        }

        /// <summary>
        /// Deletes a saved user profile.
        /// </summary>
        /// <returns>An action that deletes the user profile.</returns>
        public static Action DeleteUserProfile()
        {
            return () =>
            {
                var profiles = GetAvailableProfiles();
                if (profiles.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No saved profiles found![/]");
                    return;
                }
                string selectedProfile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select a profile to delete:[/]")
                        .AddChoices(profiles)
                );

                bool confirmDelete = AnsiConsole.Confirm($"[red]Are you sure you want to delete the profile '{selectedProfile}'? This cannot be undone![/]");
                if (!confirmDelete)
                {
                    AnsiConsole.MarkupLine("[yellow]Deletion canceled.[/]");
                    return;
                }

                string filePath = Path.Combine(UserSettings.ProfileDirectory, $"{selectedProfile}.dat");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    AnsiConsole.MarkupLine($"[green]Profile '{selectedProfile}' deleted successfully![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Error: Profile '{selectedProfile}' not found![/]");
                }
            };
        }

        /// <summary>
        /// Loads a saved user profile.
        /// </summary>
        /// <returns>An action that loads the user profile.</returns>
        public static Action LoadUserProfile()
        {
            return () =>
            {
                var profiles = GetAvailableProfiles();
                if (profiles.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No saved profiles found![/]");
                    return;
                }
                profiles.Add("Return");
                string selectedProfile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select a profile to load:[/]")
                        .AddChoices(profiles)
                );
                if (selectedProfile == "Return")
                    return;
                string password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter your password:[/]").Secret());
                string filePath = Path.Combine(UserSettings.ProfileDirectory, $"{selectedProfile}.dat");
                if (!Directory.Exists(UserSettings.ProfileDirectory))
                {
                    AnsiConsole.MarkupLine("[red]Error: Profile directory not found![/]");
                    return;
                }
                try
                {
                    byte[] encryptedData = File.ReadAllBytes(filePath);
                    string decryptedJson = DecryptData(encryptedData, password);
                    if(string.IsNullOrEmpty(decryptedJson))
                    {
                        AnsiConsole.MarkupLine("[red]Invalid password or corrupted profile![/]");
                        return;
                    }
                    var loadedSettings = JsonSerializer.Deserialize<UserSettingsDto>(decryptedJson);
                    if (loadedSettings == null)
                    {
                        AnsiConsole.MarkupLine("[red]Invalid password or corrupted profile![/]");
                        return;
                    }
                    loadedSettings.ApplyToUserSettings();
                    AnsiConsole.MarkupLine($"[green]Profile '{selectedProfile}' loaded successfully![/]");
                }
                catch (Exception)
                {
                    AnsiConsole.MarkupLine("[red]Invalid password or corrupted profile![/]");
                }
            };
        }

        /// <summary>
        /// Saves the current Qase settings to a profile.
        /// </summary>
        /// <returns>An action that saves the Qase profile.</returns>
        public static Action SaveQaseProfile()
        {
            return () =>
            {

            };
        }

        /// <summary>
        /// Loads a saved Qase profile.
        /// </summary>
        /// <returns>An action that loads the Qase profile.</returns>
        public static Action LoadQaseProfile()
        {
            return () =>
            {

            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the available user profiles.
        /// </summary>
        /// <returns>A list of available user profiles.</returns>
        public static List<string> GetAvailableProfiles()
        {
            List<string> profiles = new();
            if (!Directory.Exists(UserSettings.ProfileDirectory))            
                Directory.CreateDirectory(UserSettings.ProfileDirectory);
            
            foreach (var file in Directory.GetFiles(UserSettings.ProfileDirectory, "*.dat"))            
                profiles.Add(Path.GetFileNameWithoutExtension(file));         
            
            return profiles;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Encrypts the given plain text using the specified password.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <param name="password">The password to use for encryption.</param>
        /// <returns>The encrypted data as a byte array.</returns>
        private static byte[] EncryptData(string plainText, string password)
        {
            using var aes = Aes.Create();
            byte[] key = DeriveKeyFromPassword(password, aes.KeySize / 8);
            aes.Key = key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            byte[] combinedData = new byte[aes.IV.Length + encryptedBytes.Length];
            Array.Copy(aes.IV, 0, combinedData, 0, aes.IV.Length);
            Array.Copy(encryptedBytes, 0, combinedData, aes.IV.Length, encryptedBytes.Length);

            return combinedData;
        }

        /// <summary>
        /// Decrypts the given encrypted data using the specified password.
        /// </summary>
        /// <param name="encryptedData">The encrypted data to decrypt.</param>
        /// <param name="password">The password to use for decryption.</param>
        /// <returns>The decrypted plain text.</returns>
        private static string DecryptData(byte[] encryptedData, string password)
        {
            using var aes = Aes.Create();
            byte[] key = DeriveKeyFromPassword(password, aes.KeySize / 8);

            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] encryptedBytes = new byte[encryptedData.Length - iv.Length];
            Array.Copy(encryptedData, 0, iv, 0, iv.Length);
            Array.Copy(encryptedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        /// <summary>
        /// Derives a cryptographic key from the given password.
        /// </summary>
        /// <param name="password">The password to derive the key from.</param>
        /// <param name="keySize">The size of the key to derive.</param>
        /// <returns>The derived key as a byte array.</returns>
        private static byte[] DeriveKeyFromPassword(string password, int keySize)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SALT_VALUE_HERE"), 10000, HashAlgorithmName.SHA256);
            return deriveBytes.GetBytes(keySize);
        }
        #endregion
    }
}

