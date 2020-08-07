using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections;
using SpookyGhostBot.Modules;
using System;

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
    [Alias("mock", "say")]
    public async Task TestingTask([Remainder] string repeat = null)
    {
      await ReplyAsync(repeat);
    }

    [Command("Purge")]
    [RequireBotPermission(GuildPermission.ManageMessages)]
    [RequireUserPermission(GuildPermission.ManageMessages)]
    [Alias("clear", "delete")]
    public async Task DeleteMessages([Remainder] int num = 0)
    {
      if (num <= 100)
      {
        var messages = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
        await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        await Context.Channel.SendMessageAsync($"{Context.User.Username} deleted {num} message(s).");
        
      }
      else
      {
        await ReplyAsync("You cannot delete more than 100 messages.");
      }
    }

    [Command("Poll")]
    public async Task Poll([Remainder] string comment)
    {
      //  delete the user message
      await Context.Message.DeleteAsync();

      EmbedBuilder embed = new EmbedBuilder();
      embed.WithTitle("Please vote for the poll below.")
        .WithColor(new Color(0xfdfd96))
        .WithCurrentTimestamp()
        .WithAuthor(Context.User)
        .WithDescription($"**{comment}**");

      var response = await ReplyAsync("", false, embed.Build());
      await response.AddReactionAsync(new Emoji("\uD83D\uDC4D"));
      await response.AddReactionAsync(new Emoji("\uD83D\uDC4E"));
      await response.AddReactionAsync(new Emoji("\uD83D\uDC49"));
    }

#if false
    [Command("Help")]
    public async Task HelpMe()
    {
      EmbedBuilder builder = new EmbedBuilder();

      builder.WithTitle("Commands:")
          .WithDescription("Test\nSpooky\nRepeat\nStory\nPurge\nBalance")
          .WithColor(Color.Blue);




      await ReplyAsync("", false, builder.Build());
    }
#endif
  }
}
