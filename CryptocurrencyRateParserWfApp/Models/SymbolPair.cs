using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.Models
{
    public class SymbolPair
    {
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }

        public string PairedAsset { get; set; }

        
    }
}
