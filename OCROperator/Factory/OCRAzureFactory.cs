using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using System.Drawing;
namespace OCROperator.Factory
{
    public class OCRAzureFactory
    {
        public bool Enabled { get; init; }
        public string ApiSecret { get; init; }
        public string ApiEndpoint { get; init; }
        public ILogger Logger;
        private ComputerVisionClient _client;
        private const string Seperator = "/";
        private const int GuidIndex = 7;

        private void CreateClient()
        {
            ApiKeyServiceClientCredentials Secret = new ApiKeyServiceClientCredentials(ApiSecret);
            _client = new ComputerVisionClient(Secret)
            {
                Endpoint = ApiEndpoint
            };
            Logger.LogInformation("Setup Azure Client");
        }

        private Guid ExtractGuidFromURL(string URL)
        {
            string[] Splitted = URL.Split(Seperator);
            Guid guid = new Guid(Splitted[GuidIndex]);
            return guid;
        }

        private string GetTicketNumberFromResult(ReadOperationResult Result)
        {
            string TicketNumber = string.Empty;
            foreach(ReadResult SingleResult in Result.AnalyzeResult.ReadResults)
            {
                foreach(Line SingleLine in SingleResult.Lines)
                {
                    string Search = ZammadFactory.SearchForTicketNumber(SingleLine.Text);
                    if (!Search.Equals(string.Empty))
                    {
                        TicketNumber = Search;
                        break;
                    }
                }
                if(TicketNumber != null)
                {
                    break;
                }
            }
            return TicketNumber;
        }

        internal async Task<string> ProcessPicutreZammadNumber(List<byte[]> Picutre, CancellationToken token)
        {
            if(_client == null)
            {
                CreateClient();
            }
            string Result = string.Empty;
            foreach (byte[] SinglePicutre in Picutre)
            {
                using(MemoryStream ms = new MemoryStream(SinglePicutre))
                {
                    ReadInStreamHeaders URL = await _client.ReadInStreamAsync(ms);
                    Logger.LogInformation("Uploaded to Azure");
                    Guid guid = ExtractGuidFromURL(URL.OperationLocation);
                    Logger.LogInformation("Extracted Guid from URL");

                    Task<ReadOperationResult> ReadResult = _client.GetReadResultAsync(guid, token);

                    string ticket = GetTicketNumberFromResult(await ReadResult);
                    if(!ticket.Equals(string.Empty))
                    {
                        Result = ticket;
                        break;
                    }
                }
            }

            return Result;
        }
    }
}
