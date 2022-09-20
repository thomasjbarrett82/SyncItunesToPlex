using Serilog;

namespace Core.Services {
    public static class Helpers {
        public static void SetupLog(string className, string methodName) {
            var appLocation = Path.GetDirectoryName(Environment.ProcessPath);
            if (string.IsNullOrWhiteSpace(appLocation))
                throw new NullReferenceException("Unable to get current app location");

            var logFolder = Path.Combine(appLocation, "logs");
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            var logFile = Path.Combine(logFolder, $"{className}.{methodName}.log");
            if (File.Exists(logFile))
                File.Delete(logFile);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFile)
                .CreateLogger();
        }

        public static int RoundTo20(double? input) {
            if (input == null)
                return 0;
            
            if (input % 20 < 10)
                return Convert.ToInt32(input - (input % 20));
            else
                return Convert.ToInt32(input + (20 - (input % 20)));
        }
    }
}
