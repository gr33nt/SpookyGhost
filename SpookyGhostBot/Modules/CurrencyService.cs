using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpookyGhostBot.Modules
{
    public class CurrencyService
    {

        public CurrencyService(DiscordSocketClient client)
        {
            client.UserJoined += AddBankBalance;
        }

        private Task AddBankBalance(SocketGuildUser arg)
        {

            return Task.CompletedTask;
        }
    }
}
