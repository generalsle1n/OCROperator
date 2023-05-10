using OCROperator.Factory;

namespace OCROperator.Models.Interface.Action
{
    internal interface IAction
    {
        string Settings { get; set; }
        ILogger Logger { get; set; }
        MailFactory MailFactory { get; set; }
        OCRAzureFactory OCRAzureFactory { get; set; }
        void Setup(ILogger logger, MailFactory mailFactory, OCRAzureFactory Factory)
        {
            Logger = logger;
            MailFactory = mailFactory;
            OCRAzureFactory = Factory;
        }
        Task Execute(string Text, PapercutItem Item, byte[] PDFContent, CancellationToken token);
    }
}
