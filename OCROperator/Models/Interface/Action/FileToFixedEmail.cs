﻿using OCROperator.Factory;

namespace OCROperator.Models.Interface.Action
{
    internal class FileToFixedEmail : IAction
    {
        public string Settings { get; set; }
        public ILogger Logger { get; set; }
        public MailFactory MailFactory { get; set; }
        public async Task Execute(PapercutItem Item, string Text)
        {
            MailFactory.SendMail("OCRScan2Text", Text, new string[] { Settings.Split(";")[0] });
            Logger.LogInformation($"Mail send to {Item.User.Email}");
        }
    }
}
