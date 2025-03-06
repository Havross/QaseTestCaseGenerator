namespace QaseTestCaseGenerator.Settings
{
    public class OpenAISettings
    {
        #region Consts
        public const string URL = "https://api.openai.com/v1/chat/completions";
        public const string BalanceURL = "https://api.openai.com/v1/dashboard/billing/credit_grants";
        #endregion

        #region Fields
        private static int _maxTokens = 4000;
        private static string _notes = "";
        private static string _prompt = GenerateDefaultPrompt();
        private static int _minimalTestCount = 15;
        #endregion

        #region Properties
        public static string Notes { get { return _notes; } set { _notes = value; _prompt = GenerateDefaultPrompt(); } }
        public static Dictionary<string, string> OpenAIPrompts = new Dictionary<string, string>
        {
            {"Default Prompt", GenerateDefaultPrompt(true)},
            {"Trade Prompt", GenerateTradePrompt(true) },
            {"AMS Prompt", GenerateAMSPrompt(true) },
            {"AT Prompt", GenerateATPrompt(true) }
        };
        #endregion

        #region Public Methods
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
                    new { role = "user", content = _prompt }
                },
                max_tokens = _maxTokens
            };
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Generates and returns a default prompt for the OpenAI API based on the current notes and minimal test count.
        /// </summary>
        /// <param name="asTemplate">Whether to return prompt as a template.</param>
        /// <returns>A string representing the prompt.</returns>
        private static string GenerateDefaultPrompt(bool asTemplate = false)
        {
            string minimalTestCount = asTemplate ? "[fuchsia]<Minimal Test Count>[/]" : _minimalTestCount.ToString();
            string notes = asTemplate ? "[fuchsia]<Notes>[/]" : Notes;
            return $"Extract structured test cases from the following unstructured notes." +
                    $" Generate **at least {minimalTestCount} test cases** (do not merge them!). If the data allows, generate even more." +
                    $" Each test case must include the following fields:\n" +
                    $"- **Title**: An unique test case title\n" +
                    $"- **Description**: A short summary of the test case\n" +
                    $"- **Preconditions**: If applicable, otherwise state 'None'\n" +
                    $"- **Steps**: Each step must include an action and an expected result (minimum 2 steps, but use more if needed)\n" +
                    $"### Notes:\n" +
                    $"{notes}\n\n" +
                    $"### Format:\n" +
                    $"Title: <Test Case Title>\n" +
                    $"Description: <Short summary of the test case>\n" +
                    $"Preconditions: <If applicable, otherwise state 'None'>\n" +
                    $"Steps:\n" +
                    $"1. <Step 1>\n" +
                    $"   Expected: <Expected result>\n" +
                    $"2. <Step 2>\n" +
                    $"   Expected: <Expected result>\n" +
                    $"3. <Step 3> \n" +
                    $"   Expected: <Expected result>\n" +
                    $"...\n\n" +
                    $"**Rules**:\n" +
                    $"- **DO NOT** merge test cases.\n" +
                    $"- **Ensure at least {minimalTestCount} test cases** (or more if possible).\n" +
                    $"- **Every test case must have at least 2 steps**, but some will require more.\n" +
                    $"- **Extract as many test cases as possible** based on the given notes.\n" +
                    $"- **Follow format exactly**.";
        }

        private static string GenerateTradePrompt(bool asTemplate = false)
        {
            return "";
        }

        private static string GenerateAMSPrompt(bool asTemplate = false)
        {
            return "";
        }

        private static string GenerateATPrompt(bool asTemplate = false)
        {
            return "";
        }
        #endregion
    }
}
