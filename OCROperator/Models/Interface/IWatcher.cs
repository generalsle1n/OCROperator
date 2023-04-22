using OCROperator.Factory;
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
        internal MailFactory MailFactory { get; set; }
        internal List<Task> AllItems { get; set; }
        internal ILogger Logger { get; set; }
        internal OCRFactory OCRFactory { get; set; }
        internal void Setup();
        void SetupLogger(ILogger logger) {
            Logger = logger;
        }
        internal Task Execute();
        internal Task ProcessSingleItem(PapercutItem Item);
    }
}
