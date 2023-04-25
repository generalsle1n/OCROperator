using Microsoft.Extensions.Configuration.Json;
using OCROperator;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureAppConfiguration(conf =>
    {
        conf.Sources.OfType<JsonConfigurationSource>()
            .Where(source => source.Path.Equals("appsettings.json"))
            .First()
            .Optional = false;

        conf.Sources.OfType<JsonConfigurationSource>()
            .Where(source => source.Path.Equals("appsettings.json"))
            .First()
            .ReloadOnChange = true;
    })
    .ConfigureLogging(logger =>
    {
        string LogPath = Path.Combine(Directory.GetCurrentDirectory(), @"Logs\");
        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }
        LoggerConfiguration LoggerConfig = new LoggerConfiguration()
            .WriteTo.Console().MinimumLevel.Debug()
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .MinimumLevel.Information();
        Log.Logger = LoggerConfig.CreateLogger();
    })
    .UseWindowsService(conf =>
    {
        conf.ServiceName = "OCROperator";
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
