using Demo_API.Infrastructure;
using  Demo_Api.Application;
using Serilog;
using Serilog.Formatting.Json;
using Demo_API.Application.Exceptions;

try
{
    //Create the logger and setup
     Log.Logger = new LoggerConfiguration()

     // Write logs to a file for warning and logs with a higher severity
     // Logs are written in JSON
     .WriteTo.File(new JsonFormatter(),
         "Logs/important-logs.JSON")

     // Add a log file that will be replaced by a new log file each day
     .WriteTo.File("all-daily-.logs",
         rollingInterval: RollingInterval.Day)

     // Set default minimum log level
     .MinimumLevel.Debug()

     // Create the actual logger
     .CreateLogger();

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

   
    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


