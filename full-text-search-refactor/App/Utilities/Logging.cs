using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.App.Utilities;

public static class Logging
{
    public static ILogger Logger{get; private set;} = InitializeLogger();

    private static ILogger InitializeLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        return loggerFactory.CreateLogger("Program");
    }
}