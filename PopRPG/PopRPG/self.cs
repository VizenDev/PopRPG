using Discord;
using Discord.Commands;
using Discord.Modules;
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
        private RuntimeEnvironment re;
        private Random rnd = new Random();

        void IModule.Install(ModuleManager manager)
        {
            mod = manager;
            client = manager.Client;

            manager.CreateCommands("", c =>
            {
                c.CreateCommand("poprpg").Alias("poprpg", "rpg")
                .Parameter("text", ParameterType.Unparsed)
                .Do(async (e) =>
               {
                   string location = "rpg/PopRPG/" + e.User.Id + ".txt";
                   if (!File.Exists(location))
                   {
                       Message message = await e.Channel.SendMessage("Welcome, " + e.User.Mention + "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **```*explain poprpg```**");
                       StringBuilder sb = new StringBuilder();
                       sb.AppendLine("Weapon: Potato Sword[+1]");
                       sb.AppendLine("Armor: Potato Armor[+0.5]");
                       sb.AppendLine("XP:0");
                       sb.AppendLine("Level: 1");
                       sb.AppendLine("20/20");
                       writeFile(location, sb.ToString());
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
        }
    }
}