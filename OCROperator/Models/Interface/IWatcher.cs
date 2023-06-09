﻿using OCROperator.Factory;
using OCROperator.Models.Interface.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Models.Interface
{
    internal interface IWatcher
    {
        internal string Destination { get; set; }
        internal string SuffixMetadata { get; set; }
        internal string ActionType { get;set; }
        internal string ActionSettings { get;set; }
        internal IAction Action { get;set; }
        internal string Type { get; set; }
        internal string Language { get; set; }
        internal bool HoldPDF { get; set; }
        internal MailFactory MailFactory { get; set; }
        internal OCRAzureFactory OCRAzureFactory { get; set; }
        internal List<Task> AllItems { get; set; }
        internal ILogger Logger { get; set; }
        internal OCRFactory OCRFactory { get; set; }
        internal void Setup();
        void SetupLogger(ILogger logger) {
            Logger = logger;
        }
        internal Task ExecuteAsync(CancellationToken token);
        internal Task DeletePDFAsync(PapercutItem Item);
        public async Task ProcessSingleItemAsync(byte[] PDFContent, PapercutItem Item, CancellationToken token)
        {
            string result = OCRFactory.GetTextFromPDF(PDFContent);
            Logger.LogInformation("OCR finished");
            await Action.Execute(result, Item, PDFContent, token);
        }

        internal Task<byte[]> GetPDFContentAsync(object Parameter);
    }
}
