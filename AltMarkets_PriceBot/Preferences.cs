using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltMarkets_PriceBot.Properties;

namespace AltMarkets_PriceBot {
    public class Preferences {
        public static string DiscordToken => Settings.Default.DiscordToken;


        #region Channels
        public static ulong PriceCheckChannel => Settings.Default.PriceCheckChannel;
        #endregion



    }
}
