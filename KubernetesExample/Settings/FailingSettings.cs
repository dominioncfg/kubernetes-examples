namespace KubernetesExample.Settings
{
    public class FailingSettings
    {
        public const string SectionName = "Failing";
        public bool EnableRandomFailure { get; set; }
        public int FailureRate { get; set; }
    }
}
