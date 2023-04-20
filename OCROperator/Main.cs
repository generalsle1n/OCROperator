using OCROperator;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureAppConfiguration(conf =>
    {
        //string CurrentFolder = Directory.GetCurrentDirectory();
        //conf.SetBasePath(CurrentFolder);
        //conf.AddJsonFile("")
    })
    .Build();

await host.RunAsync();
