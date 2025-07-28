using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace D_Chandrakant
{
    public class Logger
    {
        private readonly string _logDirectory;
        private static readonly object _lock = new object();

        public Logger(IWebHostEnvironment env)
        {
            _logDirectory = Path.Combine(env.WebRootPath, "FileManager", "Logs");

            // Ensure log directory exists
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void Log(string message)
        {
            var logPath = Path.Combine(_logDirectory, $"log-{DateTime.Now:yyyy-MM-dd}.txt");
            lock (_lock)
            {
                using (var writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
                }
            }
        }

        public void LogError(Exception ex)
        {
            Log($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
}

