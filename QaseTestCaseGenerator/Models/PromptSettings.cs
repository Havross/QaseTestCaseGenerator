using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Models
{
    public class PromptSettings
    {
        public string Notes { get; set; } = "";
        private int MinimalTestCount { get; set; } = 15;
        public int MaxTokens { get; set; } = 4000;
        public PromptType Type { get; private set; }
        public string Prompt => GeneratePrompt();
        public string PromptTemplate => GeneratePrompt(asTemplate: true);
        public PromptSettings(PromptType type)
        {
            Type = type;
        }


        /// <summary>
        /// Generates and returns a default prompt for the OpenAI API based on the current notes and minimal test count.
        /// </summary>
        /// <param name="asTemplate">Whether to return prompt as a template.</param>
        /// <returns>A string representing the prompt.</returns>
        private string GenerateDefaultPrompt(bool asTemplate = false)
        {
            string minimalTestCount = asTemplate ? "[fuchsia]<Minimal Test Count>[/]" : MinimalTestCount.ToString();
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




        /// <summary>
        /// Generates a prompt for the OpenAI API based on the current notes and minimal test count.
        /// </summary>
        /// <param name="asTemplate"></param>
        /// <returns>Returns string that has current properties injected into selected template</returns>
        private string GeneratePrompt(bool asTemplate = false)
        {
            string minimalTestCount = MinimalTestCount.ToString();
            string notes = Notes;

            switch (Type)
            {
                case PromptType.Default:
                    return GenerateDefaultPrompt(asTemplate);

                case PromptType.Trade:
                    return GenerateTradePrompt(asTemplate);

                case PromptType.AMS:
                    return GenerateAMSPrompt(asTemplate);

                case PromptType.AT:
                    return GenerateATPrompt(asTemplate);

                default:
                    return "Invalid prompt type.";
            }
        }
    }
}
