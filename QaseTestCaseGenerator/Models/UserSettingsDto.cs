using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QaseTestCaseGenerator.Settings;

namespace QaseTestCaseGenerator.Models
{
    public class UserSettingsDto
    {
        public string? Adm2AuthCookie { get; set; }
        public string? JsessionIdCookie { get; set; }
        public string? OpenAIApiKey { get; set; }
        public string? QaseApiToken { get; set; }
        public required string UserTestCaseDirectory { get; set; }
        public required string OpenAIModel { get; set; }
        
        public static UserSettingsDto FromUserSettings()
        {
            return new UserSettingsDto
            {
                Adm2AuthCookie = UserSettings.Adm2AuthCookie,
                JsessionIdCookie = UserSettings.JsessionIdCookie,
                OpenAIApiKey = UserSettings.OpenAIApiKey,
                UserTestCaseDirectory = UserSettings.UserTestCaseDirectory,
                OpenAIModel = UserSettings.OpenAIModel,
                QaseApiToken = UserSettings.QaseApiToken
            };
        }

        public void ApplyToUserSettings()
        {
            UserSettings.Adm2AuthCookie = Adm2AuthCookie;
            UserSettings.JsessionIdCookie = JsessionIdCookie;
            UserSettings.OpenAIApiKey = OpenAIApiKey;
            UserSettings.UserTestCaseDirectory = UserTestCaseDirectory;
            UserSettings.OpenAIModel = OpenAIModel;
            UserSettings.QaseApiToken = QaseApiToken;
        }
    }

}
