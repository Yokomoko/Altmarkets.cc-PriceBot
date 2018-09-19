using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMarkets_PriceBot.API.POCO {
    public class MarketSummary {
        public class MarketResult {
            public string MarketName { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Volume { get; set; }
            public double Last { get; set; }
            public double Bid { get; set; }
            public double Ask { get; set; }
            public int OpenBuyOrders { get; set; }
            public int OpenSellOrders { get; set; }
            public string coin_icon_src { get; set; }
            public CoinInfo coin_info { get; set; }
        }

        public class MarketResultRoot {
            public bool success { get; set; }
            public string message { get; set; }
            public MarketResult result { get; set; }
        }

        public class CoinInfo {
            public double hold { get; set; }
            public int minconf { get; set; }
            public string page { get; set; }
            public bool active { get; set; }
            public string withdraw { get; set; }
            public string orders { get; set; }
        }
    }
}
