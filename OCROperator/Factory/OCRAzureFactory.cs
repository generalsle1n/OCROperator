using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using System.Drawing;
namespace OCROperator.Factory
{
    public class OCRAzureFactory
    {
        internal bool Enabled { get; init; }
        internal string ApiSecret { get; init; }
        internal string ApiEndpoint { get; init; }
    }
}
