using Discord;
using Discord.Commands;
using Discord.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PopRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        private DiscordClient client;
        private string botToken = Console.ReadLine();
        public void Start()
        {
            client = new DiscordClient(c =>
            {
                c.AppName = "PopRPG";
                c.LogLevel = LogSeverity.Error;
                c.LogHandler = Log;
            });
            try
            {
                client.ExecuteAndWait(async () =>
                {
                    await client.Connect(botToken, TokenType.Bot);
                    Console.Clear();
                    Console.Title = string.Format($"Currently connected as a {"BOT"}.");
                    client.UsingCommands(c =>
                    {
                        c.PrefixChar = '.';
                        c.AllowMentionPrefix = false;
                        c.IsSelfBot = false;
                        c.HelpMode = HelpMode.Public;
                    });
                    client.UsingModules();
                    client.AddModule<self>("self", ModuleFilter.None);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Something went wrong while trying to connect. Please close and re-open the application.");
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
            }
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Exception}] [{e.Source}] {e.Message}");
        }
    }
}
