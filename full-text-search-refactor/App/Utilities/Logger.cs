using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.App.Utilities;

public class Logging
{
    public static ILogger Logger{get; private set;} = InitializeLogger();

    public static ILogger InitializeLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        return loggerFactory.CreateLogger("Program");
    }
}