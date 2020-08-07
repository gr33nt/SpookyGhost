using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SpookyGhostBot.Modules
{
  [Group("Timer")]
  [Alias("Clock", "Stopwatch")]
  public class ProductivityTimer : ModuleBase<SocketCommandContext>
  {
    static Stopwatch stopwatch = new Stopwatch();

    [Command("Add")]
    [Alias("start", "begin")]
    public async Task Start()
    {
      await Context.Message.AddReactionAsync(new Emoji("\uD83D\uDC4D"));
      stopwatch.Start();

    }

    [Command("End")]
    [Alias("stop")]
    public async Task End([Remainder] string arg = null)
    {
      await Context.Message.AddReactionAsync(new Emoji("\uD83D\uDC4D"));
      stopwatch.Stop();
      TimeSpan elapsed = stopwatch.Elapsed;
      await ReplyAsync("", false, TimerBuild(elapsed, Context.User, arg).Build());
    }

    private EmbedBuilder TimerBuild(TimeSpan time, SocketUser user, string comment = null)
    {
      EmbedBuilder builder = new EmbedBuilder();
      builder.WithTitle($"Elapsed time: {String.Format("{0:0}h {1:0}m {2:0}s", time.Hours, time.Minutes, time.Seconds)}")
        .WithFooter(footer => { footer.WithText("Timer 1"); })
        .WithCurrentTimestamp()
        .WithAuthor(user);
      if (comment != null)
        builder.WithDescription(comment);

      return builder;
    }


  }
}
