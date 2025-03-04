using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
