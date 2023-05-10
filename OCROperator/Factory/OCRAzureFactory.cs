using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
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

        private async Task<ReadOperationResult> WaitOnResult(Guid Guid, CancellationToken token)
        {
            ReadOperationResult Result = new ReadOperationResult();

            while(Result.Status != OperationStatusCodes.Succeeded)
            {
                Result = await _client.GetReadResultAsync(Guid, token);
            }

            return Result;
        }

        internal async Task<string> ProcessPicutreZammadNumber(byte[] PDFContent, CancellationToken token)
        {
            if(_client == null)
            {
                CreateClient();
            }
            string Result = string.Empty;
            OCRFactory _ocr = new OCRFactory()
            {
                Logger = Logger,
            };
            List<Bitmap> BitMaps = _ocr.ConvertPDFToBitmap(PDFContent);
            List<byte[]> sites = _ocr.ConvertBitmapListToByte(BitMaps);
            foreach (byte[] SinglePicutre in sites)
            {
                using(MemoryStream ms = new MemoryStream(SinglePicutre))
                {
                    ReadInStreamHeaders URL = await _client.ReadInStreamAsync(ms);
                    Logger.LogInformation("Uploaded to Azure");
                    Guid guid = ExtractGuidFromURL(URL.OperationLocation);
                    Logger.LogInformation("Extracted Guid from URL");

                    ReadOperationResult ReadResult = await WaitOnResult(guid, token);

                    string ticket = GetTicketNumberFromResult(ReadResult);
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
