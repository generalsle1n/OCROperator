using OCROperator.Factory;
using OCROperator.Models.Interface.Action;
using System.Text.Json;

namespace OCROperator.Models.Interface
{
    internal class FileSystem : IWatcher
    {
        public string Destination { get; set; }
        public string SuffixMetadata { get; set; }
        public string ActionType { get; set; }
        public string ActionSettings { get; set; }
        public IAction Action { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public MailFactory MailFactory { get; set; }
        public List<Task> AllItems { get; set; } = new List<Task>();
        public ILogger Logger { get; set; }
        public OCRFactory OCRFactory { get; set; }
        public void Setup()
        {
            string AllPath = Path.Combine(Directory.GetCurrentDirectory(), "tesseractData");
            Logger.LogInformation("Set Mailconfig");
            MailFactory = new MailFactory
            {
                SMTPServer = "smtp.wehrle-werk.internal",
                FromMail = "ocr@wehrle-werk.internal",
                Port = 25
            };
            OCRFactory = new OCRFactory()
            {
                TesseractDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tesseractData"),
                Language = Language,
                Logger = Logger
            };
            OCRFactory.Setup();
            Action.Setup(Logger, MailFactory);
        }
        public async Task Execute()
        {
            string[] AllFiles = Directory.GetFiles(Destination, SuffixMetadata);
            Logger.LogInformation($"{AllFiles.Length} found");
            foreach(string SingleFile in AllFiles)
            {
                string MetadataContent = File.ReadAllText(SingleFile);
                File.Delete(SingleFile);
                Logger.LogInformation($"File read {SingleFile} and deleted");

                PapercutItem SingleItem = JsonSerializer.Deserialize<PapercutItem>(MetadataContent) ?? new PapercutItem();
                AllItems.Add(ProcessSingleItem(SingleItem));
            }
        }

        public async Task ProcessSingleItem(PapercutItem Item)
        {
            string path = Item.GetPathWithFile();
            string result = OCRFactory.GetTextFromPDF(path);
            Action.Execute(Item, result);
        }
    }
}
