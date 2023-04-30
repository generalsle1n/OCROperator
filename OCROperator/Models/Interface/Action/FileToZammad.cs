using OCROperator.Factory;
using System.Text.RegularExpressions;
using Zammad.Client;
using Zammad.Client.Resources;

namespace OCROperator.Models.Interface.Action
{
    internal class FileToZammad : IAction
    {
        public string Settings { get; set; }
        public ILogger Logger { get; set; }
        public MailFactory MailFactory { get; set; }
        private ZammadAccount _account;
        private TicketClient _ticketClient;
        private UserClient _userClient;
        private bool _setup = false;
        private const string _mimePDF = "application/pdf";
        private const int _openStateID = 2;
        private int _userID;
        private void SetupClient()
        {
            string[] ParsedSettings = Settings.Split(";");
            string URL = ParsedSettings[0];
            string Token = ParsedSettings[1];
            _userID = int.Parse(ParsedSettings[2]);
            _account = ZammadAccount.CreateTokenAccount(URL, Token);
            _ticketClient = _account.CreateTicketClient();
            _userClient = _account.CreateUserClient();
            _setup = true;
        }

        private async Task<Ticket> GetTicketFromNumber(string Number)
        {
            Ticket Result;
            try
            {
                IList<Ticket> AllTickets = await _ticketClient.SearchTicketAsync($"number:{Number}", 1);
                Result = AllTickets.First();
            }catch (Exception ex)
            {
                return null;
            }

            return Result;
        }

        private async Task UploadScan(Ticket DestinationTicket, PapercutItem Item, Zammad.Client.Resources.User User)
        {
            TicketAttachment pdfAttachment = TicketAttachment.CreateFromFile(
                    Item.GetPathWithFile(), 
                    _mimePDF
                );
            TicketArticle newArticel = new TicketArticle()
            {
                TicketId = DestinationTicket.Id,
                Attachments = new List<TicketAttachment>() { pdfAttachment},
                Body = $"Scan Upload\n User:{Item.User.Email}\nDate:{Item.Date}\nScannedOn:{Item.DeviceName}\nScanprofile:{Item.Name}"
            };
            try
            {
                await _ticketClient.CreateTicketArticleAsync(newArticel);
                Logger.LogInformation($"Uploaded Scan to Ticket:{DestinationTicket.Number}");
            }catch(Exception ex)
            {
                Logger.LogError(ex, "Error on upload Scan");
            }
            await SetTicketState(DestinationTicket, _openStateID);
        }
        private async Task SetTicketState(Ticket Destination, int StateID)
        {
            try
            {
                Ticket Adjusted = Destination;
                Adjusted.StateId = StateID;
                await _ticketClient.UpdateTicketAsync(Destination.Id, Adjusted);
                Logger.LogInformation($"Updated Ticketstate to open: {Destination.Number}");
            }catch(Exception ex)
            {
                Logger.LogError(ex, $"Error on Updateing State for ticket: {Destination.Number}");
            }   
        }
        private async Task<Ticket> CreateEmptyTicket(string ShouldTicket)
        {
            Ticket Result = new Ticket()
            {
                Title = $"Ticket for Scan with: {ShouldTicket}",
                OwnerId = 1,
                CustomerId = _userID,
                GroupId = 1
            };
            try
            {
                Ticket Created = await _ticketClient.CreateTicketAsync(Result, new TicketArticle()
                {
                    Body = $"This is an new Ticket --> Search please the other Ticket: {ShouldTicket} if it could be merged"
                });
                Result = Created;
                Logger.LogInformation($"Created empty Ticket: {Created.Number}, and id {Created.Id}");
            }catch( Exception ex)
            {
                Logger.LogError(ex, $"Error on creating empty ticket with shouldticket: {ShouldTicket}");
            }
            
            return Result;
        }

        private async Task<Zammad.Client.Resources.User> GetUserFromMail(string Email)
        {
            try
            {
                IList<Zammad.Client.Resources.User> SearchResult = await _userClient.SearchUserAsync($"email:{Email.ToLower()}", 1);
                return SearchResult.First();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, $"An Error occured by searching user with mail {Email}");
            }
            return null;
        }

        public async Task Execute(PapercutItem Item, string Text)
        {
            if (!_setup)
            {
                Logger.LogInformation("Setup Zammad Client");
                SetupClient();
            }
            //Search the Number in the ocr string
            string TicketNumber = ZammadFactory.SearchForTicketNumber(Text);
            Logger.LogInformation($"Ticket: {TicketNumber}");
            //Try to find Ticket in zammad
            Ticket Destination = await GetTicketFromNumber(TicketNumber.Replace("#", ""));
            Zammad.Client.Resources.User Agent = await GetUserFromMail(Item.User.Email);
            if (!TicketNumber.Equals(string.Empty) && Destination != null)
            {
                await UploadScan(Destination, Item, Agent);
            }else if(Destination == null){
                Ticket NewTicket = await CreateEmptyTicket(TicketNumber);
                await UploadScan(NewTicket, Item, Agent);
            }
        }
    }
}
