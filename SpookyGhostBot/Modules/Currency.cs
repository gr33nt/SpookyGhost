using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace SpookyGhostBot.Modules
{
    public class Currency : ModuleBase<SocketCommandContext>
    {
        private Hashtable bones;

        public Currency()
        {
            bones = new Hashtable();
        }

        [Command("AddUser")]
        public async Task AddUser()
        {
            bones.Add(Context.User.Id, 2500);
            await ReplyAsync($"Added {Context.User.Id}");
        }


        [Command("Balance")]
        public async Task Balance()
        { 
            if (!bones.ContainsKey(Context.User.Id))
            {
                var reply = ReplyAsync("Debug: User does not exist.");
                await reply;
            }
            else
            {
                await ReplyAsync("Debug: User exists.");
            }
        }



    }
}
