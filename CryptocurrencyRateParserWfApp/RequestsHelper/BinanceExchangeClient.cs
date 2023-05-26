using Binance.Net.Clients;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptocurrencyRateParserWfApp.Models;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    internal class BinanceExchangeClient : IExchangeClient
    {
        private readonly BinanceSocketClient binanceSocketClient;
        private readonly BinanceClient binanceClient;
        public BinanceExchangeClient(string apiKey, string apiSecret)
        {
            binanceSocketClient = new BinanceSocketClient(new BinanceSocketClientOptions
            {
                ApiCredentials = new BinanceApiCredentials(apiKey, apiSecret)
            });

            binanceClient = new BinanceClient(new BinanceClientOptions
            {
                ApiCredentials = new BinanceApiCredentials(apiKey, apiSecret)
            });
        }

        public async Task<string> GetEnterPrice(SymbolPair symbolPair)
        {
            var result = await binanceClient.SpotApi.ExchangeData.GetTickerAsync(symbolPair.BaseAsset + symbolPair.QuoteAsset);
            if(result != null && result.Success) { return result.Data.LastPrice.ToString(); }
            return "Не найдено. Ошибка.";

        }

        public async Task SubscribeToTickerUpdatesAsync<T>(SymbolPair symbolPair, Action<DataEvent<T>> handler)
        {
            await binanceSocketClient.UnsubscribeAllAsync();
            var symbol = symbolPair.BaseAsset + symbolPair.QuoteAsset;
            await binanceSocketClient.UsdFuturesStreams.SubscribeToTickerUpdatesAsync(symbol, (Action<DataEvent<IBinance24HPrice>>)handler);
        }
    }
}
