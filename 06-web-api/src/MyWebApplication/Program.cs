using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;
using Mohaymen.FullTextSearch.MyWebApplication.Interfaces;
using Mohaymen.FullTextSearch.MyWebApplication.Services;

namespace Mohaymen.FullTextSearch.MyWebApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IApplicationService, ApplicationService>();
        builder.Services.AddSingleton<IInvertedIndexBuilder, FilesAdvancedInvertedIndexBuilder>();
        builder.Services.AddSingleton<IFileReader, FileReader>();
        builder.Services.AddSingleton<ITokenizer, Tokenizer>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.Run();
    }
}