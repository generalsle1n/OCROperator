using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCROperator.Factory
{
    internal class ZammadFactory
    {
        private const string _ticketSearch = "#[0-9]{7}";

        internal static string SearchForTicketNumber(string Text)
        {
            string Result = string.Empty;
            Regex TicketSearch = new Regex(_ticketSearch);
            Match Match = TicketSearch.Match(Text);
            if (Match.Success)
            {
                Result = Match.Value;
            }
            return Result;
        }
    }
}
