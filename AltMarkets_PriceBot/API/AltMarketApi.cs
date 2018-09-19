using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AltMarkets_PriceBot.API.POCO;
using CoinDeskBitcoinPrice;
using Discord;
using Newtonsoft.Json;
using Discord.Commands;
using Discord.WebSocket;

namespace AltMarkets_PriceBot.API {
    public class AltMarketApi {
        private static readonly string _exchangeURL = "https://altmarkets.cc/api/v1/public";
        private static readonly string ExchangeLinkUrl = "https://altmarkets.cc/market/";

        public static async Task<MarketSummary.MarketResultRoot> GetMarketSummary(string market) {
            using (var httpClient = new HttpClient()) {
                string url = _exchangeURL + $"/getmarketsummary?market=BTC-{market}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode) {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketSummary.MarketResultRoot>(result);
                }
                else {
                    return null;
                }
            }
        }

        public static async Task<Embed> GetMarketEmbed(string market) {
            var embed = new EmbedBuilder();

            var price = new CoinDeskBitcoinPriceClient();
            var btc = await price.GetCurrentPrice("BTC");




            var marketResult = await GetMarketSummary(market.ToUpper());
            if (marketResult.success) {
                var btcPrice = btc.Bpi["USD"].Rate_Float;
                var myntPrice = btcPrice * (decimal)marketResult.result.Last;
                var volumeUSD = (decimal)marketResult.result.Volume * myntPrice;

                embed.WithTitle($"{marketResult.result.MarketName}");
                embed.WithThumbnailUrl(marketResult.result.coin_icon_src.Replace("%3A", ":"));
                embed.WithUrl(ExchangeLinkUrl + marketResult.result.MarketName);

                var msgText = new StringBuilder();
                msgText.AppendLine($"**Price BTC:** {marketResult.result.Last:N8}");
                msgText.AppendLine($"**Price USD:** ${myntPrice:N5}{Environment.NewLine}");
                msgText.AppendLine($"**24H High:** {marketResult.result.High:N8}");

                msgText.AppendLine($"**24H Low:** {marketResult.result.Low:N8}{Environment.NewLine}");
                msgText.AppendLine($"**Volume:** {marketResult.result.Volume:N2} {market.ToUpper()}");
                msgText.AppendLine($"**Volume (USD):** ${volumeUSD:N2}");


                embed.WithDescription(msgText.ToString());
            }
            else {
                embed.WithDescription(marketResult.message);
            }
            return embed;
        }

    }


}
