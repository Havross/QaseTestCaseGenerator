﻿using QaseTestCaseGenerator.Settings;

namespace QaseTestCaseGenerator.Models
{
    public class UserSettingsDto
    {
        #region Properties
        public required string UserTestCaseDirectory { get; set; }
        public required string OpenAIModel { get; set; }
        public string? Adm2AuthCookie { get; set; }
        public string? JsessionIdCookie { get; set; }
        public string? OpenAIApiKey { get; set; }
        public string? QaseApiToken { get; set; }
        public PromptSettings? PromptSettings { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new instance of <see cref="UserSettingsDto"/> from the current user settings.
        /// </summary>
        /// <returns>A new instance of <see cref="UserSettingsDto"/>.</returns>
        public static UserSettingsDto FromUserSettings()
        {
            return new UserSettingsDto
            {
                Adm2AuthCookie = UserSettings.Adm2AuthCookie,
                JsessionIdCookie = UserSettings.JsessionIdCookie,
                OpenAIApiKey = UserSettings.OpenAIApiKey,
                UserTestCaseDirectory = UserSettings.UserTestCaseDirectory,
                OpenAIModel = UserSettings.OpenAIModel,
                QaseApiToken = UserSettings.QaseApiToken,
                PromptSettings = UserSettings.PromptSettings
            };
        }

        /// <summary>
        /// Applies the settings from this instance to the current user settings.
        /// </summary>
        public void ApplyToUserSettings()
        {
            UserSettings.Adm2AuthCookie = Adm2AuthCookie;
            UserSettings.JsessionIdCookie = JsessionIdCookie;
            UserSettings.OpenAIApiKey = OpenAIApiKey;
            UserSettings.UserTestCaseDirectory = UserTestCaseDirectory;
            UserSettings.OpenAIModel = OpenAIModel;
            UserSettings.QaseApiToken = QaseApiToken;
            UserSettings.PromptSettings = PromptSettings ?? OpenAISettings.OpenAIPrompts[PromptType.Default];
        }
        #endregion
    }
}
