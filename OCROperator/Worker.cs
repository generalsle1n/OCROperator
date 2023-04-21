using OCROperator.Factory;
using OCROperator.Models;
using OCROperator.Models.Interface;
using System.Net;

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

            AllWatcher = GetWatchers();
        }

        private List<IWatcher> GetWatchers()
        {
            List<IWatcher> AllWatcher = new List<IWatcher>();

            IWatcher[] RawWatchers = _config.GetSection("Wehrlesettings:Watchers").Get<RawWatcher[]>();
           
            foreach(IWatcher watcher in RawWatchers)
            {
                IWatcher SingleWatcher = _rFactory.CreateObjectFromRaw((RawWatcher)watcher);
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
                    _logger.LogInformation($"Skipped Watcher{DateTime.Now}");
                    watcher.Execute();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}