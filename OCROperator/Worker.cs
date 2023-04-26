using OCROperator.Factory;
using OCROperator.Models.Interface;

namespace OCROperator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private List<IWatcher> AllWatcher = new List<IWatcher>();
        private ReflectionFactory _rFactory = new ReflectionFactory();

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _logger.LogInformation("Init Watcher");
            AllWatcher = GetWatchers();
        }

        private List<IWatcher> GetWatchers()
        {
            List<IWatcher> AllWatcher = new List<IWatcher>();

            IWatcher[] RawWatchers = _config.GetSection("Wehrlesettings:Watchers").Get<RawWatcher[]>();
           
            foreach(IWatcher watcher in RawWatchers)
            {
                IWatcher SingleWatcher = _rFactory.CreateObjectFromRaw((RawWatcher)watcher);
                SingleWatcher.SetupLogger(_logger);
                SingleWatcher.Setup();
                AllWatcher.Add(SingleWatcher);
            }

            return AllWatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach(IWatcher watcher in AllWatcher)
                {
                    await watcher.ExecuteAsync();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}