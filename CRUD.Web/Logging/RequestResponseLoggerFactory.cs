using CRUD.Application.Optionss;
using Serilog;

namespace CRUD.Web.Logging
{
    public static class RequestResponseLoggerFactory
    {
        private const string OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm} | {Message}{NewLine}";

        public static Serilog.ILogger Create(RequestResponseLoggingOptions options)
        {
            var intervalMinutes = Math.Clamp(options.RollingIntervalMinutes, 1, 60);
            var directory = options.LogDirectory;
            var prefix = options.FileNamePrefix;

            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Map(
                    _ => GetBucket(DateTime.Now, intervalMinutes),
                    (bucket, writeTo) => writeTo.File(
                        Path.Combine(directory, $"{prefix}{bucket}.log"),
                        rollingInterval: RollingInterval.Infinite,
                        outputTemplate: OutputTemplate,
                        shared: true))
                .CreateLogger();
        }

        private static string GetBucket(DateTime time, int intervalMinutes)
        {
            var roundedMinute = (time.Minute / intervalMinutes) * intervalMinutes;
            return $"{time:yyyyMMdd_HH}{roundedMinute:D2}";
        }
    }
}
