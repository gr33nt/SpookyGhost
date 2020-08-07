using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

    [Command("Join", RunMode = RunMode.Async)]
    public async Task JoinVoice(IVoiceChannel channel = null)
    {
      channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
      if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

      // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
      var audioClient = await channel.ConnectAsync();
    }


    #region Config
    [Group("Config")]
    public class ConfigModule : ModuleBase<SocketCommandContext>
    {
      [Command("UserName")]
      [Alias("Name")]
      [Summary("Updates the username of the bot.")]
      public async Task Update([Remainder] string name)
      {
        await Context.Client.CurrentUser.ModifyAsync(x => { x.Username = name; });
        await Context.Message.AddReactionAsync(new Emoji("\uD83D\uDC4D"));
      }

      [Command("Status")]
      [Alias("game", "playing")]
      public async Task Status([Remainder] string status)
      {
        await Context.Client.SetActivityAsync(new Game(status));
        await Context.Message.AddReactionAsync(new Emoji("\uD83D\uDC4D"));
      }
    }
    #endregion

    [Group("Database")]
    [Alias("db")]
    public class DatabaseModule : ModuleBase<SocketCommandContext>
    {


#if false
      public async Task doThing()
      {
        
      }
#endif
    }
  }
}
