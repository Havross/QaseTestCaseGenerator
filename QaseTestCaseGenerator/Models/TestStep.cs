using System.Text.Json.Serialization;

namespace QaseTestCaseGenerator.Models
{
    public class TestStep
    {
        #region Properties
        [JsonPropertyName("action")]
        public required string Action { get; set; }

        [JsonPropertyName("expected_result")]
        public required string ExpectedResult { get; set; }
        #endregion
    }
}
