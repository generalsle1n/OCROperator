using OCROperator.Factory;
using OCROperator.Models.Interface.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Models.Interface
{
    internal class RawWatcher : IWatcher
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
            throw new NotImplementedException();
        }
        public async Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
        public async Task ProcessSingleItemAsync(PapercutItem Item)
        {
            throw new NotImplementedException();
        }
        public async Task DeletePDFAsync(PapercutItem Item)
        {
            throw new NotImplementedException();
        }
    }
}
