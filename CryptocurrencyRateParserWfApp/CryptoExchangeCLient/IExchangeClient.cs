﻿using Bybit.Net.Objects.Models.Socket.Spot;
using CryptocurrencyRateParserWfApp.Models;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRateParserWfApp.RequestsHelper
{
    /// <summary>
    /// Общий интерфейс для реализации классов Бирж
    /// </summary>
    public interface IExchangeClient
    {
        Task SubscribeToTickerUpdatesAsync<T>(SymbolPair symbolPair, Action<DataEvent<T>> handler);

        Task<string> GetEnterPrice(SymbolPair symbolPair);
    }
}
