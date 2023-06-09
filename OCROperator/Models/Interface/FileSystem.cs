﻿using OCROperator.Factory;
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
        public bool HoldPDF { get; set; }
        public MailFactory MailFactory { get; set; }
        public OCRAzureFactory OCRAzureFactory { get; set; }
        public List<Task> AllItems { get; set; } = new List<Task>();
        public ILogger Logger { get; set; }
        public OCRFactory OCRFactory { get; set; }
        public void Setup()
        {
            string AllPath = Path.Combine(Directory.GetCurrentDirectory(), "tesseractData");
            Logger.LogInformation("Set Mailconfig");
            OCRFactory = new OCRFactory()
            {
                TesseractDataPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "tesseractData"),
                Language = Language,
                Logger = Logger
            };
            OCRFactory.Setup();
            Action.Setup(Logger, MailFactory, OCRAzureFactory);
        }
        public async Task ExecuteAsync(CancellationToken token)
        {
            string[] AllFiles = Directory.GetFiles(Destination, SuffixMetadata);
            Logger.LogDebug($"{AllFiles.Length} found");
            foreach(string SingleFile in AllFiles)
            {
                string MetadataContent = File.ReadAllText(SingleFile);
                File.Delete(SingleFile);
                Logger.LogInformation($"File read {SingleFile} and deleted");

                PapercutItem SingleItem = JsonSerializer.Deserialize<PapercutItem>(MetadataContent) ?? new PapercutItem();
                byte[] PDFContent = await GetPDFContentAsync(SingleItem.GetPathWithFile());
                ((IWatcher)this).ProcessSingleItemAsync(PDFContent, SingleItem, token);
            }
        }

        public async Task DeletePDFAsync(PapercutItem Item)
        {
            string path = Item.GetPathWithFile();
            try
            {
                File.Delete(path);
                Logger.LogInformation($"Deleted File {path}");
            }catch(Exception ex)
            {
                Logger.LogError(ex, $"Error on Deleting File: {path}");
            }
        }

        public async Task<byte[]> GetPDFContentAsync(object Parameter)
        {
            return File.ReadAllBytes((string)Parameter);
        }
    }
}
