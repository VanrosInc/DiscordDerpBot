using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using DiscordDerpBot.Logging;
using System.IO;
using System.Threading;

namespace DiscordDerpBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                new Program().Start("").GetAwaiter().GetResult();
            else
                new Program().Start(args[0]).GetAwaiter().GetResult();
        }

        private static string Token = "";

        private DiscordSocketClient _client;

        private IServiceCollection _map = new ServiceCollection();
        private CommandService _commands = new CommandService();

        private static Logger _logger;

        private Task Logger(LogMessage message)
        {
            _logger.SetSender(message.Source);
            switch (message.Severity)
            {
                case (LogSeverity.Info):
                    {
                        _logger.Log(LogStatus.INFO, message.Message);
                        break;
                    }
                case (LogSeverity.Warning):
                    {
                        _logger.Log(LogStatus.WARNING, message.Message);
                        break;
                    }
                case (LogSeverity.Error):
                    {
                        _logger.Log(LogStatus.ERROR, message.Message);
                        break;
                    }
                case (LogSeverity.Critical):
                    {
                        _logger.Log(LogStatus.SEVERE, message.Message);
                        break;
                    }
                case (LogSeverity.Debug):
                    {
                        _logger.Log(LogStatus.DEBUG_INFO, message.Message);
                        break;
                    }
            }
            _logger.SetSender("DerpBot");
            return Task.CompletedTask;
        }

        private async Task Start(string token)
        {
            //await InitCommands();
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
            });
            _logger = new Logger("DerpBot", true, $@"{Directory.GetCurrentDirectory()}\Logs");
            _client.Log += Logger;
            if (Token != null)
            {
                _logger.Log(LogStatus.ERROR, "The token was found hard-coded into the bot, this is VERY unsafe, please consiter removing it from the code!");
            }
            else
            {
                if (token.StartsWith("-"))
                {
                    token = token.Replace("-", "");
                }
                Token = token;
            }
            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task InitCommands()
        {
            
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            
        }
    }
}
