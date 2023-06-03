using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    /// <summary>
    /// интерфейс фабрики, выбор типа и создание соотв. класса биржи
    /// </summary>
    public interface IExchangeClientFactory
    {
        IExchangeClient CreateClient(ExchangeType exchangeType);
    }
    /// <summary>
    /// Перечисления бирж
    /// </summary>
    public enum ExchangeType
    {
        Bybit, 
        Kucoin, 
        Binance
    }
}
