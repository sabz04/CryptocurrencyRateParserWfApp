using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    public interface IExchangeClientFactory
    {
        IExchangeClient CreateClient(ExchangeType exchangeType);
    }
    public enum ExchangeType
    {
        Bybit, 
        Kucoin, 
        Binance
    }
}
