using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Data.SQLite;

namespace SpookyGhostBot
{
  public class Program
  {
    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IConfiguration _config;

    private IServiceProvider _services;

    public Program()
    {
      _client = new DiscordSocketClient();
      _commands = new CommandService();
      _services = new ServiceCollection()
          .AddSingleton(_client)
          .AddSingleton(_commands)
          .BuildServiceProvider();

      //event subscriptions
      _client.Log += Log;

      var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
      _config = _builder.Build();
    }

    public async Task RunBotAsync()
    {
      await RegisterCommandsAsync();

      await _client.LoginAsync(TokenType.Bot, _config["Token"]);

      await _client.StartAsync();

      _client.Ready += () =>
      {

        Console.WriteLine("Bot is connected!");
        //UpdateDB(_client);
        return Task.CompletedTask;
      };


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

      if (message.HasStringPrefix(_config["Prefix"], ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
      {
        var context = new SocketCommandContext(_client, message);

        //IDisposable typing = context.Channel.EnterTypingState();
        var result = await _commands.ExecuteAsync(context, argPos, _services);
        //typing.Dispose();

        if (!result.IsSuccess)
        {
          Console.WriteLine(result.ErrorReason);
        }
      }
    }
    
    private void UpdateDB(DiscordSocketClient client)
    {
      
      using (SQLiteConnection conn = new SQLiteConnection("data source=SpookyGhostBot.db"))
      {
        using (SQLiteCommand tableCommand = new SQLiteCommand())
        {
          string tableSql = "Create Table If Not Exists Guild (ID integer Primary Key AutoIncrement, GuildID text Not Null, Name text Not Null);";
          tableCommand.CommandText = tableSql;
          tableCommand.Connection = conn;
          conn.Open();
          tableCommand.ExecuteNonQuery();
          conn.Close();
        }
        using (SQLiteCommand cmd = new SQLiteCommand())
        {
          foreach (var guild in client.Guilds)
          {
            string strSql = $"Insert Into Guild (GuildID, Name) Select '{guild.Id}', '{guild.Name}' Where Not Exists (Select ID From Guild Where GuildID = '{guild.Id}');";
            cmd.CommandText = strSql;
            cmd.Connection = conn;
            conn.Open();
            //            cmd.Connection = conn;
            /*
INSERT INTO EVENTTYPE (EventTypeName)
SELECT 'ANI Received'
WHERE NOT EXISTS (SELECT 1 FROM EVENTTYPE WHERE EventTypeName = 'ANI Received');
            https://www.tutorialspoint.com/sqlite/sqlite_date_time.htm
566685754000932872
Create Table [Guild]
(
    [ID] integer Primary Key AutoIncrement,
    [GuildID] text Not Null,
    [Name] text Not Null
);
Insert Into Guild (GuildID, Name) Values ({guild.Id}, '{guild.Name}')
Create Table [PersonAddress]
(
    [ID] int Not Null Identity(1, 1),
    [PersonID] int Not Null,
    [AddressTypeID] int Not Null,
    Constraint [PK_PersonAddress] Primary Key Clustered
    (
        [ID] Asc
    )
);


            */
            try
            {
              cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
              Console.WriteLine(exc.Message);
            }
            finally
            {
              conn.Close();
            }

          }

          // do something
          
        }
      }
    }
  }
}
