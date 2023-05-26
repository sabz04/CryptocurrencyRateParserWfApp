using Binance.Net.Clients;
using Bybit.Net.Clients;
using Bybit.Net.Objects;
using Bybit.Net.Objects.Models.Socket.Spot;
using CryptocurrencyRateParserWfApp.Models;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    internal class BybitExchangeClient : IExchangeClient
    {
        private readonly BybitSocketClient socketClient;
        private readonly BybitClient bybitClient;

        public BybitExchangeClient(string apiKey, string apiSecret)
        {
            socketClient = new BybitSocketClient(new BybitSocketClientOptions
            {
                ApiCredentials = new ApiCredentials(apiKey, apiSecret)
            });
            bybitClient = new BybitClient(new BybitClientOptions
            {
                ApiCredentials = new ApiCredentials(apiKey, apiSecret)
            });
        }

        public async Task<string> GetEnterPrice(SymbolPair symbolPair)
        {
            var result = await bybitClient.SpotApiV1.ExchangeData.GetTickerAsync(symbolPair.BaseAsset + symbolPair.QuoteAsset);
            if (result != null && result.Success) { return result.Data.LastPrice.ToString(); }
            return "Не найдено. Ошибка.";
        }

        public async Task SubscribeToTickerUpdatesAsync<T>(SymbolPair symbolPair, Action<DataEvent<T>> handler)
        {
            await socketClient.SpotStreamsV1.UnsubscribeAllAsync();
            var sumbolBybit = symbolPair.BaseAsset + symbolPair.QuoteAsset;
            await socketClient.SpotStreamsV1.SubscribeToTickerUpdatesAsync(sumbolBybit, (Action<DataEvent<BybitSpotTickerUpdate>>)handler);
        }
    }
}
