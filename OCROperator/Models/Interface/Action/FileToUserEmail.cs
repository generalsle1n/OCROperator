using OCROperator.Factory;

namespace OCROperator.Models.Interface.Action
{
    internal class FileToUserEmail : IAction
    {
        public string Settings { get; set; }
        public ILogger Logger { get; set; }
        public MailFactory MailFactory { get; set; }
        public OCRAzureFactory OCRAzureFactory { get; set; }


        public async Task Execute(string Text, PapercutItem Item, byte[] PDFContent, CancellationToken token)
        {
            MailFactory.SendMail("OCRScan2Text", Text, new string[] {Item.User.Email});
            Logger.LogInformation($"Mail send to {Item.User.Email}");
            //return true;
        }
    }
}
