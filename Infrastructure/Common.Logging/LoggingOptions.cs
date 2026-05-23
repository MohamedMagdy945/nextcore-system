namespace Common.Logging
{
    public class LoggingOptions
    {
        public string ServiceName { get; set; } = string.Empty;

        public string SeqUrl { get; set; } = string.Empty;

        public string ElasticsearchUrl { get; set; } = string.Empty;

        public bool UseConsole { get; set; } = true;

        public bool UseSeq { get; set; } = true;

        public bool UseElasticsearch { get; set; }
    }
}
