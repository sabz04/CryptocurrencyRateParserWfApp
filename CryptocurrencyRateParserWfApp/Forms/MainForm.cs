using Binance.Net.Interfaces;
using Bybit.Net.Clients;
using Bybit.Net.Objects;
using Bybit.Net.Objects.Models.Socket.Spot;
using CryptocurrencyRateParserWfApp.Models;
using CryptocurrencyRateParserWfApp.RequestsHelper;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Sockets;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects;
using Kucoin.Net.Objects.Models.Spot.Socket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptocurrencyRateParserWfApp.Forms
{
    public partial class MainForm : Form
    {
      
        private readonly string apiKeyBybit = "ParBUwDl29ElWljUzO";
        private readonly string apiSecretBybit = "GjQVkBbWs8GKaJj8XpNRv1DH5ee2XIEZSwUu";

        private readonly string apiKeyKucoin = "646f973a96076a0001c4bcae";
        private readonly string apiSecretKucoin = "c6b110f4-3025-46dd-874c-30021e09c1b0";
        private readonly string apiPassKucoin = "878278";


        private readonly string apiKeyBinance= "bUdoFsQ05U9yOcp7APkAvQE9S54cuNTWt1jMjDMZxKSiNNeeKivmkzulPYnpY6DB";
        private readonly string apiSecretBinance = "URU6lMV5m42eoHfF8Oo41n5rwKG2CIaR3DGkB9DXNzVvwQjwbM0pjZxYGQJ73IP2";
        

        private readonly BybitExchangeClient bybitExchageClient;
        private readonly KucoinExchangeClient kucoinExchageClient;
        private readonly BinanceExchangeClient binanceExchangeClient;

        private List<SymbolPair> pairedSymbols = new List<SymbolPair>();
        public MainForm()
        {
            InitializeComponent();

            bybitExchageClient = new BybitExchangeClient(apiKeyBybit, apiSecretBybit);
            kucoinExchageClient = new KucoinExchangeClient(apiKeyKucoin, apiSecretKucoin, apiPassKucoin);
            binanceExchangeClient = new BinanceExchangeClient(apiKeyBinance, apiSecretBinance);

            symbolsPairsComboBox.SelectedIndexChanged += SymbolsPairsComboBox_SelectedIndexChanged;

            SymbolsPairsComboBox_SelectedIndexChanged(null, null);

        }

        

        private async void SymbolsPairsComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if(pairedSymbols.Count < 1) { await LoadSymbols(); }
           
            if (symbolsPairsComboBox.SelectedItem == null)
                return;
            var selectedItem = symbolsPairsComboBox.SelectedItem.ToString();
            var currentPair = pairedSymbols.FirstOrDefault(x=>x.PairedAsset == selectedItem);

            bybitRateLabel.Text = "Загрузка..";
            binanceRateLabel.Text = "Загрузка..";
            kucoinRateLabel.Text = "Загрузка..";
            if(currentPair == null) return;
            await LoadRates(currentPair);
            
        }

        private async Task LoadSymbols()
        {
            var client = new BybitClient(new BybitClientOptions { ApiCredentials = new ApiCredentials(apiKeyBybit, apiSecretBybit) });

            var res = await client.SpotApiV1.ExchangeData.GetSymbolsAsync();

            res.Data.ToList().ForEach(x =>
            {
                pairedSymbols.Add(new SymbolPair
                {
                    QuoteAsset = x.QuoteAsset,
                    BaseAsset = x.BaseAsset,
                    PairedAsset = x.BaseAsset + x.QuoteAsset
                });
                symbolsPairsComboBox.Items.Add(x.BaseAsset + x.QuoteAsset);
            });
        }

        private async Task LoadRates(SymbolPair selectedSymbolPair)
        {
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    var firstPriceBeforeSell = await bybitExchageClient.GetEnterPrice(selectedSymbolPair);
                    SetLabelData(bybitRateLabel, "Последняя цена продажи: " + firstPriceBeforeSell);
                    await bybitExchageClient.SubscribeToTickerUpdatesAsync(selectedSymbolPair, new Action<DataEvent<BybitSpotTickerUpdate>>(BybitTickerResult));
                }),
                Task.Run(async () =>
                {
                    var firstPriceBeforeSell = await kucoinExchageClient.GetEnterPrice(selectedSymbolPair);
                    SetLabelData(kucoinRateLabel, "Последняя цена продажи: " + firstPriceBeforeSell);
                    await kucoinExchageClient.SubscribeToTickerUpdatesAsync(selectedSymbolPair, new Action<DataEvent<KucoinStreamTick>>(KucoinRatesresult));
                }),
                Task.Run(async () =>
                {
                    var firstPriceBeforeSell = await binanceExchangeClient.GetEnterPrice(selectedSymbolPair);
                    SetLabelData(binanceRateLabel, "Последняя цена продажи: " + firstPriceBeforeSell);
                    await binanceExchangeClient.SubscribeToTickerUpdatesAsync(selectedSymbolPair, new Action<DataEvent<IBinance24HPrice>>(BinanceRatesResult));
                })
            );
        }

        

        private void BinanceRatesResult(DataEvent<IBinance24HPrice> data)
        {
            var currentPrice = data.Data.LastPrice;
            SetLabelData(binanceRateLabel, $"Текущий курс {data.Data.Symbol} на Binance: {currentPrice}");

        }
        private void BybitTickerResult(DataEvent<BybitSpotTickerUpdate> data)
        {
            var currentPrice = data.Data.LastPrice;
           
            SetLabelData(bybitRateLabel, $"Текущий курс {data.Data.Symbol} на Bybit: {currentPrice}");
        }

        private void KucoinRatesresult(DataEvent<KucoinStreamTick> data)
        {
            var currentPrice = data.Data.LastPrice;
            SetLabelData(kucoinRateLabel, $"Текущий курс {data.Data.Symbol} на Kucoin: {currentPrice}");
        }
        private void SetLabelData(Label label, string data)
        {
            label.BeginInvoke(() =>
            {
                label.Text = data;
            });
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }

    enum Exchange {
        Binance, 
        Kucoin,
        Bybit 
    }

}
