using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections;
using SpookyGhostBot.Modules;


namespace SpookyGhostBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("Test")]
        public async Task Testing()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Welcome!")
                .WithDescription($"_Enjoy your stay {Context.User.Mention}!_")
                .WithColor(Color.DarkerGrey);

            


            await ReplyAsync("", false, builder.Build());
        }

        [Command("Spooky")]
        public async Task Spooky()
        {
            await ReplyAsync($"***You're spooky!*** {Context.User.Mention}");
        }


        [Command("Repeat")]
        [Alias ("mock", "say")]
        public async Task TestingTask([Remainder] string repeat = null)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(repeat);            
        }


        [Command("Debug Balance")]
        public async Task Debug()
        {
            Hashtable test = new Hashtable();
            test.Add(Context.User.Id, "test");

            if (test.ContainsKey(Context.User.Id))
            {
                await ReplyAsync("Debug Passed");
            }
            else
            {
                await ReplyAsync("Debug Failed");
            }
        }



        [Command("Purge")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Alias ("clear", "delete")]
        public async Task DeleteMessages([Remainder] int num = 0)
        {
            if (num <= 100)
            {
                var messages = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
                if (num == 1)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Username + " deleted 1 message.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{Context.User.Username} deleted {num} messages.");
                }
            }
            else
            {
                await ReplyAsync("You cannot delete more than 100 messages.");
            }
        }
        
        [Command("Help")]
        public async Task HelpMe()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Commands:")
                .WithDescription("Test\nSpooky\nRepeat\nStory\nPurge\nBalance")
                .WithColor(Color.Blue);




            await ReplyAsync("", false, builder.Build());
        }

    }
}
