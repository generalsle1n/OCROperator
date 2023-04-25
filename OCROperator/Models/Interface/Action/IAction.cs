using OCROperator.Factory;

namespace OCROperator.Models.Interface.Action
{
    internal interface IAction
    {
        string Settings { get; set; }
        ILogger Logger { get; set; }
        MailFactory MailFactory { get; set; }
        void Setup(ILogger logger, MailFactory mailFactory)
        {
            Logger = logger;
            MailFactory = mailFactory;
        }
        bool Execute(PapercutItem Item, string Text);
    }
}
