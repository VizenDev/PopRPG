using DataAccess;
using Discord;
using Discord.Commands;
using Discord.Modules;
using Domain;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PopRPG
{
    internal class self : IModule
    {
        private DiscordClient client;
        private ModuleManager mod;
        private Random rnd = new Random();
        private DataManager dataManager = new DataManagerText();

        void IModule.Install(ModuleManager manager)
        {
            mod = manager;
            client = manager.Client;

            manager.CreateCommands("", c =>
            {
                c.CreateCommand("explain")
                .Parameter("command", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (e.GetArg("command") == "")
                        await e.Channel.SendMessage("The correct usage for this command would be ```.explain [command to explain]```(without the brackets of course)");
                    else if (e.GetArg("command") == "rpg")
                        await e.Channel.SendMessage("potato");
                });

                c.CreateCommand("poprpg").Alias("poprpg", "rpg")
                .Parameter("text", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    Player player = dataManager.GetPlayer(e.User.Id);
                    if (player.IsFirstTimePlaying)
                    {
                        await SendWelcomeMessage(e);
                    }
                    else
                    {
                        var param = e.GetArg("text");
                        string enemy = "";
                        if (param == "stats")
                        {
                            await SendStatsMessage(e, player);
                        }
                        if (param == "attack" && enemy == "empty")
                        {

                        }
                        if (param == "attack" && enemy != "empty")
                        {

                        }
                    }
                });
            });
        }

        private async Task SendStatsMessage(CommandEventArgs e, Player player)
        {
            Message message = await e.Channel.SendMessage(player.ToString());
        }

        private async Task SendWelcomeMessage(CommandEventArgs e)
        {
            Message message = await e.Channel.SendMessage("Welcome, " + 
                e.User.Mention + 
                "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **`*explain poprpg`**");
        }

        /*void IModule.Install(ModuleManager manager)
        {
            mod = manager;
            client = manager.Client;

            manager.CreateCommands("", c =>
            {
                c.CreateCommand("poprpg").Alias("poprpg", "rpg")
                .Parameter("text", ParameterType.Unparsed)
                .Do(async (e) =>
               {
                   string enemy = "";
                   string location = "rpg/PopRPG/" + e.User.Id + ".txt";
                   var param = e.GetArg("text");
                   StringBuilder startInv = new StringBuilder();
                   StringBuilder stats = new StringBuilder();
                   if (!File.Exists(location))
                   {
                       Message message = await e.Channel.SendMessage("Welcome, " + e.User.Mention + "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **`.explain poprpg`**");
                       startInv.AppendLine("Weapon: Potato Sword[+1]");
                       startInv.AppendLine("Armor: Potato Armor[+0.5]");
                       startInv.AppendLine("XP: 0");
                       startInv.AppendLine("Level: 1");
                       startInv.AppendLine("HP: 20/20");
                       writeFile(location, startInv.ToString());
                   }
                   else if (File.Exists(location))
                   {
                       if (param == "stats")
                       {
                           stats.AppendLine(readLine(location, 1));
                           stats.AppendLine(readLine(location, 2));
                           stats.AppendLine(readLine(location, 3));
                           stats.AppendLine(readLine(location, 4));
                           stats.AppendLine(readLine(location, 5));
                           Message message2 = await e.Channel.SendMessage(e.User.Mention + "\n" + stats.ToString());
                       }
                       if (param == "attack" && enemy == "empty")
                       {

                       }
                       if (param == "attack" && enemy != "empty")
                       {

                       }
                   }
               });
            });
        }
        private string readLine(string fileName, int lineNumber)
        {
            using (Stream stream = File.Open($"{fileName}", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = null;
                    for (int i = 0; i < lineNumber; ++i)
                    {
                        line = reader.ReadLine();
                    }
                    return line;
                }
            }
        }

        private void writeFile(string fileName, string text)
        {
            StreamWriter sWrite = new StreamWriter($"{fileName}");
            sWrite.Write(text);
            sWrite.Close();
        }*/
    }
}