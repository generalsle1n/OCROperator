using OCROperator.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Models.Interface
{
    internal interface IWatcher
    {
        internal IWatcher(ILogger logger)
        {

        }
        internal readonly ILogger _logger;
        internal string Destination { get; set; }
        internal string SuffixMetadata { get; set; }
        internal string Action { get; set; }
        internal string Type { get; set; }
        internal string Language { get; set; }
        internal MailFactory MailFactory { get; set; }
        internal List<Task> AllItems { get; set; }
        internal void Setup();
        internal Task Execute();
        internal Task ProcessSingleItem(string Item);
    }
}
