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
    /// <summary>
    /// Класс - помогатор для парсинга данных валют и присоединения к серверу через сокеты. 
    /// </summary>
    internal class BinanceExchangeClient : IExchangeClient
    {
        private readonly BinanceSocketClient binanceSocketClient;
        private readonly BinanceClient binanceClient;
        /// <summary>
        /// принимают ключи - credentials для использования апишки, инициализирует сразу два клиента:
        /// socketClient - для подключения и отслеживания изменения валют
        /// binanceClient - для получения курса валюты перед первым входом, т.к socketClient отслеживает изменения, и если валюта не будет изменять в цене, будет пустая строка и ожидание
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
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
        /// <summary>
        /// Первое получение данных, чтобы зафиксировать текущий курс до его изменения.
        /// </summary>
        /// <param name="symbolPair"></param>
        /// <returns></returns>
        public async Task<string> GetEnterPrice(SymbolPair symbolPair)
        {
            var result = await binanceClient.SpotApi.ExchangeData.GetTickerAsync(symbolPair.BaseAsset + symbolPair.QuoteAsset);
            if(result != null && result.Success) { return result.Data.LastPrice.ToString(); }
            return "Не найдено. Ошибка.";

        }
        /// <summary>
        /// Принимает пару-связку валют, и действие - обработчик события. В этом участке устанавливается прямое соединение с сервером через socket
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="symbolPair"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task SubscribeToTickerUpdatesAsync<T>(SymbolPair symbolPair, Action<DataEvent<T>> handler)
        {
            await binanceSocketClient.UnsubscribeAllAsync();
            var symbol = symbolPair.BaseAsset + symbolPair.QuoteAsset;
            await binanceSocketClient.UsdFuturesStreams.SubscribeToTickerUpdatesAsync(symbol, (Action<DataEvent<IBinance24HPrice>>)handler);
        }
    }
}
