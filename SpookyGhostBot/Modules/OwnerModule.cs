using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpookyGhostBot.Modules
{
  [RequireOwner]
  public class OwnerModule : ModuleBase<SocketCommandContext>
  {
    [Command("Embed")]
    public async Task BuildEmbed()
    {
      EmbedBuilder embed = new EmbedBuilder();
      embed.WithColor(Color.DarkOrange)
        .WithCurrentTimestamp()
        .WithFooter(footer => { footer.WithText("Hey!"); });

      await ReplyAsync("", false, embed.Build());
    }

    [Group("Config")]
    public class ConfigModule : ModuleBase<SocketCommandContext>
    {
      [Command("UserName")]
      [Alias("Name")]
      [Summary("Updates the username of the bot.")]
      public async Task Update([Remainder] string name)
      {
        await Context.Client.CurrentUser.ModifyAsync(x => { x.Username = name; });
        await ReplyAsync("Username updated.");
      }

      [Command("Status")]
      [Alias("game", "playing")]
      public async Task Status([Remainder] string status)
      {
        IActivity game = new Game(status);
        await Context.Client.SetActivityAsync(game);
      }
    }
  }
}
