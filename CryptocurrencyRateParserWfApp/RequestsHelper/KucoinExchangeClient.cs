using Bybit.Net.Objects.Models.Socket.Spot;
using CryptocurrencyRateParserWfApp.Models;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects;
using Kucoin.Net.Objects.Models.Spot;
using Kucoin.Net.Objects.Models.Spot.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    internal class KucoinExchangeClient : IExchangeClient
    {
        private readonly KucoinSocketClient socketClient;
        private readonly KucoinClient kucoinClient;
        public KucoinExchangeClient(string apiKey, string apiSecret, string apiPass)
        {
            socketClient = new KucoinSocketClient(new KucoinSocketClientOptions
            {
                ApiCredentials = new KucoinApiCredentials(apiKey, apiSecret, apiPass)
            });
            kucoinClient = new KucoinClient(new KucoinClientOptions
            {
                ApiCredentials = new KucoinApiCredentials(apiKey, apiSecret, apiPass)
            });
        }

        public async Task<string> GetEnterPrice(SymbolPair symbolPair)
        {
            var symbol = symbolPair.BaseAsset + "-" + symbolPair.QuoteAsset;
            var result = await kucoinClient.SpotApi.ExchangeData.GetTickerAsync(symbol);
            if (result != null && result.Success && result.Data != null) { return result.Data.LastPrice.ToString(); }
            return "Не найдено. Ошибка.";
        }

        public async Task SubscribeToTickerUpdatesAsync<T>(SymbolPair symbolPair, Action<DataEvent<T>> handler)
        {
            await socketClient.SpotStreams.UnsubscribeAllAsync();
            var symbol = symbolPair.BaseAsset +"-"+ symbolPair.QuoteAsset;
            var result = await socketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbol, (Action<DataEvent<KucoinStreamTick>>)handler);
        }
    }
}
