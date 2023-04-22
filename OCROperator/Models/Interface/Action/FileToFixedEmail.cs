using OCROperator.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Models.Interface.Action
{
    internal class FileToFixedEmail : IAction
    {
        public string Settings { get; set; }
        public ILogger Logger { get; set; }
        public MailFactory MailFactory { get; set; }
        public bool Execute(PapercutItem Item, string Text)
        {
            return true;
        }
    }
}
