using AltMarkets_PriceBot.API;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AltMarkets_PriceBot {
    class Program {
        public static Discord.WebSocket.DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;
        private const string TickerPrefix = "$";

        private static void Main(string[] args) {
            RunBotAsync();
            Thread.Sleep(-1);
        }

        private static async void RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            _client.Log += ClientOnLog;

            RegisterCommands();
            await _client.LoginAsync(TokenType.Bot, Preferences.DiscordToken);
            await _client.StartAsync();

        }

        private static async void RegisterCommands() {
            _client.MessageReceived += ClientOnMessageReceived;
            await _commands.AddModulesAsync(typeof(AltMarketApi).Assembly);
        }

        private static async Task ClientOnMessageReceived(SocketMessage socketMessage) {
            if (!(socketMessage is SocketUserMessage message) || message.Author.IsBot) {
                return;
            }
            int argPos = 0;
            if (message.HasStringPrefix(TickerPrefix, ref argPos)) {
                var msg = message.ToString().Split('$').Last();
                var embed = await AltMarketApi.GetMarketEmbed(msg);
                await message.Channel.SendMessageAsync("", false, embed);
            }
        }

        private static async Task ClientOnLog(LogMessage arg) {
            Console.WriteLine(arg.ToString());
        }
    }
}
