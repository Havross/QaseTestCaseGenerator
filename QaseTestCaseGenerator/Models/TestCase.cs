using QaseTestCaseGenerator.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QaseTestCaseGenerator.Models
{
    public class TestCase
    {
        #region Properties
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("preconditions")]
        public string? Preconditions { get; set; }

        [JsonPropertyName("steps")]
        public required TestStep[] Steps { get; set; }

        //Nongenerated props
        [JsonPropertyName("postconditions")]
        public string? Postconditions { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("severity")]
        public int? Severity { get; set; }

        [JsonPropertyName("behavior")]
        public int? Behavior { get; set; }

        [JsonPropertyName("type")]
        public int? Type { get; set; }

        [JsonPropertyName("layer")]
        public int? Layer { get; set; }

        [JsonPropertyName("is_flaky")]
        public bool? IsFlaky { get; set; }

        [JsonPropertyName("author_id")]
        public int? AuthorId { get; set; }

        [JsonPropertyName("suite_id")]
        public int? SuiteId { get; set; }

        [JsonPropertyName("milestone_id")]
        public int? MilestoneId { get; set; }

        [JsonPropertyName("automation")]
        public int? Automation { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("attachments")]
        public List<string> Attachments { get; set; } = new();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new();

        [JsonPropertyName("params")]
        public Dictionary<string, string> Params { get; set; } = new();

        [JsonPropertyName("custom_field")]
        public Dictionary<int, string> CustomField { get; set; } = new();
        #endregion

        #region Public Methods
        public void Enrich()
        {
            Postconditions = TestCaseSettings.Postconditions;
            Status = TestCaseSettings.Status;
            Priority = TestCaseSettings.Priority;
            Severity = TestCaseSettings.Severity;
            Behavior = TestCaseSettings.Behavior;
            Type = TestCaseSettings.Type;
            Layer = TestCaseSettings.Layer;
            IsFlaky = TestCaseSettings.IsFlaky;
            AuthorId = TestCaseSettings.AuthorId;
            SuiteId = TestCaseSettings.SuiteId;
            MilestoneId = TestCaseSettings.MilestoneId;
            Automation = TestCaseSettings.Automation;
            CreatedAt = DateTime.Now;
        }
        #endregion
    }
}
