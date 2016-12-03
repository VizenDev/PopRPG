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
        private RuntimeEnvironment re;
        private static Random rnd = new Random();
        private DataManager dataManager = new DataManagerText(rnd);
        private Player player;

        void IModule.Install(ModuleManager manager)
        {

            mod = manager;
            client = manager.Client;

            

            manager.CreateCommands("", c =>
            {
                c.CreateCommand("poprpg").Alias("poprpg", "rpg")
                .Do(async (e) =>
                {
                    player = player ?? dataManager.GetPlayer(e.User.Id);
                    if (player.IsFirstTimePlaying)
                    {
                        await SendWelcomeMessage(e);
                    }
                    else
                    {
                        //do nothing
                        #region old code
                        /*var param = e.GetArg("text");
                        if (param == "stats")
                        {
                            await SendStatsMessage(e, player);
                        }
                        else if (param == "enterDungeon")
                        {
                            EnterDungeon(e, player);
                        }
                        else if (param == "attack")
                        {
                            if (player.IsInDungeon())
                            {
                                Monster m = player.CurrentDungeon.GetRandomMonster();
                                if (m.Equals(player.CurrentDungeon.Boss))
                                {
                                    await e.Channel.SendMessage("The boss of this dungeon has appeared! Defeat "+ m.Name + " to clear the dungeon!");
                                    //enter fight command
                                    player.AddExp(player.CurrentDungeon.RewardExp);
                                    dataManager.SetPlayer(e.User.Id, player);
                                    await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received "+ player.CurrentDungeon.RewardExp +" exp as reward!");
                                    //self.cs shouldn't have an instance of player. It should 
                                    //have an instance of a service of players, which would call the dataaccess
                                    //to modify the data file
                                }
                            }
                            else
                            {
                                await e.Channel.SendMessage("Enter a dungeon to use this command");
                            }
                        }
                        else if (param == "search")
                        {
                            if (player.IsInDungeon())
                            {

                            }
                            else
                            {
                                await e.Channel.SendMessage("Enter a dungeon to use this command");
                            }
                        }*/
                        #endregion
                    }
                });
                c.CreateCommand("stats").Do(async (e) =>
                {
                    await SendStatsMessage(e, player);
                    Console.WriteLine($"{e.User.Name} performed the **stats** command.");
                });
                c.CreateCommand("enterDungeon").Do((e) =>
                {
                    EnterDungeon(e, player);
                    Console.WriteLine($"{e.User.Name} performed the **enterDungeon** command.");
                });
                c.CreateCommand("attack").Do(async (e) => 
                {
                    if (player.IsInDungeon())
                    {
                        Monster m = player.CurrentDungeon.GetRandomMonster();
                        if (m.Equals(player.CurrentDungeon.Boss))
                        {
                            await e.Channel.SendMessage("The boss of this dungeon has appeared! Defeat " + m.Name + " to clear the dungeon!");
                            //enter fight command
                            player.AddExp(player.CurrentDungeon.RewardExp);
                            dataManager.SetPlayer(e.User.Id, player);
                            await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received " + player.CurrentDungeon.RewardExp + " exp as reward!");
                            //self.cs shouldn't have an instance of player. It should 
                            //have an instance of a service of players, which would call the dataaccess
                            //to modify the data file
                        }
                    }
                    else
                        await e.Channel.SendMessage("Enter a dungeon to use this command");

                    Console.WriteLine($"{e.User.Name} performed the **attack** command.");
                });
                c.CreateCommand("search").Do(async (e) =>
                {
                    if (player.IsInDungeon())
                    {
                    }
                    else
                        await e.Channel.SendMessage("Enter a dungeon to use this command");

                    Console.WriteLine($"{e.User.Name} performed the **search** command.");
                });
            });
        }

        private async void EnterDungeon(CommandEventArgs e, Player player)
        {
            Dungeon dungeon = dataManager.GetRandomDungeonInPlayerRange(player);
            player.CurrentDungeon = dungeon;
            await e.Channel.SendMessage("Welcome " + player.Name + 
                " to the " + dungeon.Name);
        }

        private async Task SendStatsMessage(CommandEventArgs e, Player player)
        {
            await e.Channel.SendMessage(player.ToString());
        }

        private async Task SendWelcomeMessage(CommandEventArgs e)
        {
            await e.Channel.SendMessage("Welcome, " + 
                e.User.Mention + 
                "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **`*explain poprpg`**");
        }
    }
}