using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Serilog;

namespace InstantGram.Common.Helper
{
    public static class LoggerHelper
    {
        public static void ConfigureSeriLog()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.RollingFile("Logs\\Logs-{Date}.log")
                            .CreateLogger();

        }
    }
}