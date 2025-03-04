using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Settings
{
    public class OpenAISettings
    {
        public static int MinimalTestCount = 15;
        private static string _notes = "";
        public static string Notes { get { return _notes; } set { _notes = value; Prompt = GeneratePrompt(); } }
        public static int MaxTokens = 4000;
        public static int TestCasePriority = 2;
        public static string Model = "gpt-4o";
        public const string URL = "https://api.openai.com/v1/chat/completions";
        public const string BalanceURL = "https://api.openai.com/v1/dashboard/billing/credit_grants";
        public static string Prompt = GeneratePrompt();


        private static string GeneratePrompt()
        {
            return $"Extract structured test cases from the following unstructured notes." +
                    $" Generate **at least {MinimalTestCount} test cases** (do not merge them!). If the data allows, generate even more." +
                    $" Each test case must include the following fields:\n" +
                    $"- **Title**: An unique test case title\n" +
                    $"- **Description**: A short summary of the test case\n" +
                    $"- **Preconditions**: If applicable, otherwise state 'None'\n" +
                    $"- **Steps**: Each step must include an action and an expected result (minimum 2 steps, but use more if needed)\n" +
                    $"### Notes:\n" +
                    $"{Notes}\n\n" +
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
                    $"- **Ensure at least {MinimalTestCount} test cases** (or more if possible).\n" +
                    $"- **Every test case must have at least 2 steps**, but some will require more.\n" +
                    $"- **Extract as many test cases as possible** based on the given notes.\n" +
                    $"- **Follow format exactly**.";
        }
        public static object GetRequestBody()
        {
            return new
            {
                model = UserSettings.OpenAIModel,
                messages = new[]
                {
                    new { role = "system", content = "You are an AI that extracts structured test cases from unstructured notes." },
                    new { role = "user", content = Prompt }
                },
                max_tokens = MaxTokens
            };
        }

    }
}
