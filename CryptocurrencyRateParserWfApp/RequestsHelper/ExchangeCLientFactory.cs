using CryptocurrencyRateParserWfApp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    public class ExchangeCLientFactory : IExchangeClientFactory
    {
        public IExchangeClient CreateClient(ExchangeType exchangeType)
        {
            switch(exchangeType)
            {
                case ExchangeType.Bybit:
                    return new BybitExchangeClient(ApiConfiguration.ApiKeyBybit, ApiConfiguration.ApiSecretBybit);
                case ExchangeType.Binance:
                    return new BinanceExchangeClient(ApiConfiguration.ApiKeyBinance, ApiConfiguration.ApiSecretBinance);
                case ExchangeType.Kucoin:
                    return new KucoinExchangeClient(ApiConfiguration.ApiKeyKucoin, ApiConfiguration.ApiSecretKucoin, ApiConfiguration.ApiPassKucoin);
                default:
                    break;
            }
            return null;
        }
    }
}
