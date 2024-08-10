using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.App.Utilities;

public static class Logging<TCategoryName>
{
    public static ILogger<TCategoryName> Logger{get; private set;}
    
    static Logging()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        Logger = loggerFactory.CreateLogger<TCategoryName>();
    }
}