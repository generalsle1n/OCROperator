using OCROperator.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Models.Interface
{
    public class RawWatcher : IWatcher
    {
        public string Destination { get; set; }
        public string SuffixMetadata { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public MailFactory MailFactory { get; set; }
        public List<Task> AllItems { get; set; } = new List<Task>();
        public void Setup()
        {
            throw new NotImplementedException();
        }
        public async Task Execute()
        {
            throw new NotImplementedException();
        }
        public async Task ProcessSingleItem(string Item)
        {
            throw new NotImplementedException();
        }
    }
}
