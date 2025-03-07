using QaseTestCaseGenerator.Models;

namespace QaseTestCaseGenerator.Settings
{
    public class OpenAISettings
    {
        #region Consts
        public const string URL = "https://api.openai.com/v1/chat/completions";
        public const string BalanceURL = "https://api.openai.com/v1/dashboard/billing/credit_grants";
        #endregion

        #region Properties
        public static Dictionary<PromptType, PromptSettings> OpenAIPrompts { get; } = new Dictionary<PromptType, PromptSettings>
        {
            { PromptType.Default, new PromptSettings(PromptType.Default) },
            { PromptType.Trade, new PromptSettings(PromptType.Trade) },
            { PromptType.AMS, new PromptSettings(PromptType.AMS) },
            { PromptType.AT, new PromptSettings(PromptType.AT) }
        };
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the active prompt type.
        /// </summary>
        public static void SetActivePrompt(PromptType promptType)
        {
            if (OpenAIPrompts.ContainsKey(promptType))
            {
                UserSettings.PromptSettings = OpenAIPrompts[promptType];
            }
        }

        /// <summary>
        /// Constructs and returns the request body for the OpenAI API.
        /// </summary>
        /// <returns>An object representing the request body.</returns>
        public static object GetRequestBody()
        {
            return new
            {
                model = UserSettings.OpenAIModel,
                messages = new[]
                {
                    new { role = "system", content = "You are an AI that extracts structured test cases from unstructured notes." },
                    new { role = "user", content = UserSettings.PromptSettings.Prompt }
                },
                max_tokens = UserSettings.PromptSettings.MaxTokens
            };
        }

        #endregion
    }
}
