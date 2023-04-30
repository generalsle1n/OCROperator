using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Factory
{
    public class OCRAzureFactory
    {
        internal bool Enabled { get; init; }
        internal string ApiSecret { get; init; }
        internal string ApiEndpoint { get; init; }
    }
}
