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
#if false
  public class ProductivityTimer : ModuleBase<SocketCommandContext>
  {
    static Stopwatch stopwatch = new Stopwatch();
    [Command("Timer")]
    [Alias("Clock", "Stopwatch", "Time")]
    public async Task Timer([Remainder] string arg = null)
    {
      if (arg == null)
      {
        await ReplyAsync("No arg specified. Arg types: start, end."); return;
      }

      string[] strs = arg.Split(new string[] { " " }, 2, StringSplitOptions.None);
      switch (strs[0].ToLower())
      {
        case "start":
          await StartTime(); break;
        case "end":
          await EndTime(strs.Length == 2 ? strs[1] : null); break;
        default:
          await ReplyAsync("Arg unrecognized. Try Start or End."); break;
      }
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

    private async Task StartTime()
    {
      await Context.Message.DeleteAsync();
      stopwatch.Start();

      await Context.User.SendMessageAsync($"Timer started at {DateTime.Now}. Message deleted to limit spam.");

    }

    private async Task EndTime(string comment = null)
    {
      await Context.Message.DeleteAsync();
      stopwatch.Stop();
      TimeSpan elapsed = stopwatch.Elapsed;
      await ReplyAsync("", false, TimerBuild(elapsed, Context.User, comment).Build());
    }

  }

#endif


  [Group("Timer")]
  [Alias("Clock", "Stopwatch")]
  public class ProductivityTimer : ModuleBase<SocketCommandContext>
  {
    static Stopwatch stopwatch = new Stopwatch();

    [Command("Add")]
    [Alias("start", "begin")]
    public async Task Start()
    {
      await Context.Message.DeleteAsync();
      stopwatch.Start();

      await Context.User.SendMessageAsync($"Timer started at {DateTime.Now}. Message deleted to limit spam.");
    }

    [Command("End")]
    [Alias("stop")]
    public async Task End([Remainder] string arg = null)
    {
      await Context.Message.DeleteAsync();
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
