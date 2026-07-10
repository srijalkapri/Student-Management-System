using CRUD.Application.Optionss;
using Serilog;

namespace CRUD.Web.Logging
{
    public static class RequestResponseLoggerFactory
    {
        private const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm} | {Message}{NewLine}";

        public static Serilog.ILogger Create(RequestResponseLoggingOptions options)
        {
            var directory = options.LogDirectory;
            var prefix = options.FileNamePrefix;
            var mode = options.RollingMode?.Trim();
            var maxFileSizeBytes = Math.Max(1, options.MaxFileSizeKb) * 1024L;
            var loggerConfiguration = new LoggerConfiguration().MinimumLevel.Information();

            if (string.Equals(mode, "Custom", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(mode))
            {
                var intervalMinutes = Math.Clamp(options.RollingIntervalMinutes, 1, 60);
                return loggerConfiguration
                    .WriteTo.Map(
                        _ => GetBucket(DateTime.Now, intervalMinutes),
                        (bucket, writeTo) => writeTo.File(
                            Path.Combine(directory, $"{prefix}{bucket}.log"),
                            rollingInterval: RollingInterval.Infinite,
                            fileSizeLimitBytes: maxFileSizeBytes,
                            rollOnFileSizeLimit: true,
                            outputTemplate: OutputTemplate,
                            shared: true))
                    .CreateLogger();
            }

            if (!Enum.TryParse<RollingInterval>(mode, true, out var rollingInterval))
            {
                rollingInterval = RollingInterval.Day;
            }

            return loggerConfiguration
                .WriteTo.File(
                    path: Path.Combine(directory, $"{prefix}-.log"),
                    rollingInterval: rollingInterval,
                    fileSizeLimitBytes: maxFileSizeBytes,
                    rollOnFileSizeLimit: true,
                    outputTemplate: OutputTemplate,
                    shared: true)
                .CreateLogger();
        }

        private static string GetBucket(DateTime time, int intervalMinutes)
        {
            var roundedMinute = (time.Minute / intervalMinutes) * intervalMinutes;
            return $"{time:yyyyMMdd_HH}{roundedMinute:D2}";
        }
    }
}
