using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Settings
{
    public class UserSettings
    {
        #region Consts
        public const string ProfileDirectory = "Profiles";
        #endregion

        #region Props
        public static string? Adm2AuthCookie { get; set; }
        public static string? JsessionIdCookie { get; set; }
        public static string? OpenAIApiKey { get; set; }
        public static string? QaseApiToken { get; set; }
        public static string UserTestCaseDirectory { get; set; } = "TestCases";
        public static string OpenAIModel { get; set; } = "gpt-3.5-turbo";
        #endregion
    }
}
