using System;
using System.Collections;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SpookyGhostBot.Modules;

namespace SpookyGhostBot
{
    public class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private IServiceProvider _services;
        private CommandService _commands = new CommandService();
        //public Currency currency = new Currency();


        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();

            

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            //event subscriptions
            _client.Log += Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, "MzY1MjU0MzEyMjQ0MjgxMzQ0.Xgh2Xg.O8tdKJsbkdsOP2eJbISG_3uLCjY");

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;

            // If you get an error saying 'CompletedTask' doesn't exist,
            // your project is targeting .NET 4.5.2 or lower. You'll need
            // to adjust your project's target framework to 4.6 or higher
            // (instructions for this are easily Googled).
            // If you *need* to run on .NET 4.5 for compat/other reasons,
            // the alternative is to 'return Task.Delay(0);' instead.
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("##", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
