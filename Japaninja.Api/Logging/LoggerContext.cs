using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Core;

namespace Japaninja.Logging;

public static class LoggerContext
{
    public static ILogger Current { get; set; }


    static LoggerContext()
    {
        Current = NullLogger.Instance;
    }
}