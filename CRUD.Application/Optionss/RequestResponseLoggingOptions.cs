

namespace CRUD.Application.Optionss
{
    public class RequestResponseLoggingOptions
    {

        public bool Enabled { get; set; } = true;

        public string[] ExcludedPaths { get; set; } = new [] { "/swagger", "/health" };

        public string LogDirectory { get; set; } = "Logs";

        public string FileNamePrefix { get; set; } = "RequestResponseLog_";

        public int RollingIntervalMinutes { get; set; } = 5;

    }
}
