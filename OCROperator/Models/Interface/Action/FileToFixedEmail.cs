﻿using OCROperator.Factory;
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
            MailFactory.SendMail("OCRScan2Text", Text, new string[] { Settings.Split(";")[0] });
            Logger.LogInformation($"Mail send to {Item.User.Email}");
            return true;
        }
    }
}
