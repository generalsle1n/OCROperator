using OCROperator.Factory;

namespace OCROperator.Models.Interface.Action
{
    internal class FileToUserEmail : IAction
    {
        public string Settings { get; set; }
        public ILogger Logger { get; set; }
        public MailFactory MailFactory { get; set; }

        public async Task Execute(PapercutItem Item, string Text)
        {
            MailFactory.SendMail("OCRScan2Text", Text, new string[] {Item.User.Email});
            Logger.LogInformation($"Mail send to {Item.User.Email}");
            //return true;
        }
    }
}
