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
                .Parameter("command", ParameterType.Required)
                .Parameter("commandExt", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (e.GetArg("command") == "")
                        await e.Channel.SendMessage("The correct usage for this command would be ```.explain [command to explain]```(without the brackets of course)");
                    else if (e.GetArg("command") == "rpg" && e.GetArg("commandExt") == "")
                        await e.Channel.SendMessage("The command ``rpg`` is the main command for PopRPG.");
                    else if (e.GetArg("command") == "rpg" && e.GetArg("commandExt") == "stats")
                        await e.Channel.SendMessage("The command ``rpg stats`` will display your stats.");
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
            Message message = await e.Channel.SendMessage(e.User.Mention + "\n" + player.ToString());
        }

        private async Task SendWelcomeMessage(CommandEventArgs e)
        {
            Message message = await e.Channel.SendMessage("Welcome, " + 
                e.User.Mention + 
                "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **`.explain poprpg`**");
        }
    }
}