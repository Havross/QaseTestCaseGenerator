namespace QaseTestCaseGenerator.Settings
{
    public class TestCaseSettings
    {
        #region Properties
        public static string? Postconditions { get; set; }
        public static int? Status { get; set; } = 1; // 1 = New, 2 = In Progress, 3 = Passed, 4 = Failed, etc.

        public static int? Priority { get; set; } = 2;  // 1 = High, 2 = Medium, 3 = Low

        public static int? Severity { get; set; } = 2; // 0 = Critical, 1 = Major, 2 = Minor, etc.

        public static int? Behavior { get; set; } = 0; // 0 = Expected, 1 = Unexpected

        public static int? Type { get; set; } = 1; // 1 = Functional, 2 = Performance, 3 = Security, etc.

        public static int? Layer { get; set; } = 2; // 1 = Unit, 2 = Integration, 3 = System, etc.

        public static bool? IsFlaky { get; set; } = false;

        public static int? AuthorId { get; set; } = 0; // 0 = Unassigned, otherwise set to a valid user ID.

        public static int? SuiteId { get; set; } = 1; // 1 = General, otherwise specific suite ID.

        public static int? MilestoneId { get; set; } = null;

        public static int? Automation { get; set; } = 0; // 0 = Manual, 1 = Automated

        public static DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public static DateTime? UpdatedAt { get; set; } = null;
        #endregion
    }
}
