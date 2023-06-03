using Binance.Net.Interfaces;
using Bybit.Net.Clients;
using Bybit.Net.Objects;
using Bybit.Net.Objects.Models.Socket.Spot;
using CryptocurrencyRateParserWfApp.Configuration;
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
      
        private readonly IExchangeClient bybitExchageClient;
        private readonly IExchangeClient kucoinExchageClient;
        private readonly IExchangeClient binanceExchangeClient;

        private ExchangeCLientFactory exchangeClientFactory;
        private List<SymbolPair> pairedSymbols = new List<SymbolPair>();

        /// <summary>
        /// создание всех необходимых типов и упаковка в интерфейс, использовал фабрику, т.к посчитал возможным и нужным обобщение всех типов в интерфейс,
        /// а также возможность расширения фабрики новыми классами бирж от того же разработчика J.Korf
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            exchangeClientFactory = new ExchangeCLientFactory();

            bybitExchageClient = exchangeClientFactory.CreateClient(ExchangeType.Bybit);
            binanceExchangeClient = exchangeClientFactory.CreateClient(ExchangeType.Binance);
            kucoinExchageClient = exchangeClientFactory.CreateClient(ExchangeType.Kucoin);

            symbolsPairsComboBox.SelectedIndexChanged += SymbolsPairsComboBox_SelectedIndexChanged;

            SymbolsPairsComboBox_SelectedIndexChanged(null, null);

        }

        
        /// <summary>
        /// Метод - обработчик события, который срабатывает при выборе пары валют.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Метод для подгрузки всех пар валют.
        /// </summary>
        /// <returns></returns>
        private async Task LoadSymbols()
        {
            var client = new BybitClient(new BybitClientOptions { ApiCredentials = new ApiCredentials(ApiConfiguration.ApiKeyBybit, ApiConfiguration.ApiSecretBybit) });

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
        /// <summary>
        /// Асинхронный метод, параллельно запускающий несколько тасков, принимает выбранную пару валют для отслеживания.
        /// Запускает отслеживание валют. С помощью сокетов присоединяется напрямую к серверу, и подписываетя к конкретному событию.
        /// </summary>
        /// <param name="selectedSymbolPair"></param>
        /// <returns></returns>
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

        
        /// <summary>
        /// Принимает объект-результат получаемых валют, срабатывают каждый раз при изменении курса валют
        /// </summary>
        /// <param name="data"></param>
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

  

}
