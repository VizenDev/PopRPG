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
        private Dungeon dg;

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
                        await e.Channel.SendMessage($"Oooooh. Welcome back{player.Name}. I am sure you know what to do.");
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
                c.CreateCommand("explore").Do((e) =>
                {
                    player = player ?? dataManager.GetPlayer(e.User.Id);
                    if (player.CurrentDungeon == null)
                    {
                        EnterDungeon(e, player);
                    }
                    else if (player.CurrentDungeon != null)
                    {
                        e.Channel.SendMessage("You are already in a dungeon");
                    }
                    Console.WriteLine($"{e.User.Name} performed the **explore** command.");
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
                            player.CurrentDungeon = null;
                            //self.cs shouldn't have an instance of player. It should 
                            //have an instance of a service of players, which would call the dataaccess
                            //to modify the data file
                        }
                        else if (!m.Equals(player.CurrentDungeon.Boss))
                        {
                            await e.Channel.SendMessage($"The enemy of this dungeon has appeared! Defeat {m.Name} to clear the dungeon!");
                            if (player.CurrentDungeon.RewardItem == null)
                            {
                                await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received " + player.CurrentDungeon.RewardExp + " exp as reward!");
                            }
                            else if (player.CurrentDungeon.RewardItem != null)
                            {
                                player.AddItem(player.CurrentDungeon.RewardItem);
                                await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received a " + player.CurrentDungeon.RewardItem + " and " + player.CurrentDungeon.RewardExp +" exp as reward!");
                            }
                            player.AddExp(player.CurrentDungeon.RewardExp);
                            dataManager.SetPlayer(e.User.Id, player);
                            player.CurrentDungeon = null;
                        }
                    }
                    else
                        await e.Channel.SendMessage("Enter a dungeon to use this command");

                    Console.WriteLine($"{e.User.Name} performed the **attack** command.");
                });
                c.CreateCommand("run").Do(async (e) =>
                {
                    double newHP = rnd.NextDouble() * (1.5 - 2.5) + 1.5;
                    var takenHP = Math.Abs(newHP).ToString().Substring(0, 3);
                    if (player.IsInDungeon())
                    {
                        await e.Channel.SendMessage("You decided to run away. Doing so got you hit with " + takenHP + " damage.");
                        player.RemoveHP(Convert.ToDouble(takenHP));
                        dataManager.SetPlayer(e.User.Id, player);
                        player.CurrentDungeon = null;
                        await e.Channel.SendMessage($"Your HP is now **{player.HP}/{player.hp(player.Level)}**");
                    }
                    else if (!player.IsInDungeon())
                    {
                        await e.Channel.SendMessage("You can't run away from a dungeon if you aren't even in one.");
                    }
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
                c.CreateCommand("store").Do(async (e) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("                                 STORE");
                    sb.AppendLine("------------------------------------------------------------------------------");
                    sb.AppendLine();
                    sb.AppendLine("* < Levels:> 500xp for each level.");
                    sb.AppendLine("* < Healing Potion:> For 1000xp you can fully restore your HP.");
                    sb.AppendLine("* < Slime Sword:> For 1500xp you can buy a Slime Sword[+2.5].");
                    sb.AppendLine();
                    sb.AppendLine("                             END OF STORE");
                    sb.AppendLine("------------------------------------------------------------------------------");
                    await e.Channel.SendMessage($"```md\n{sb.ToString()}```");
                });
                c.CreateCommand("buy").Parameter("item", ParameterType.Unparsed).Do(async (e) => 
                {
                    if (e.GetArg("item") == "level")
                    {
                        if (player.XP >= 500)
                        {
                            player.AddLevel(1);
                            player.RemoveExp(500);
                            player.AddHP(player.hp(player.Level));
                            dataManager.SetPlayer(e.User.Id, player);
                            await e.Channel.SendMessage($"Woo Hoo! You are now at level **{player.Level}.**");
                        }
                        else if (player.XP <= 500)
                        {
                            await e.Channel.SendMessage("You tried buying a level but failed because you don't have enough xp.");
                            await e.Channel.SendMessage("Try exploring and attacking to gain xp.");
                        }
                    }
                });
            });
        }

        private async void EnterDungeon(CommandEventArgs e, Player player)
        {
            Dungeon dungeon = dataManager.GetRandomDungeonInPlayerRange(player);
            player.CurrentDungeon = dungeon;
            await e.Channel.SendMessage("Welcome " + player.Name.Trim() + 
                " to the " + dungeon.Name);
        }

        private async Task SendStatsMessage(CommandEventArgs e, Player player)
        {
            await e.Channel.SendMessage(e.User.Mention + "```" + player.ToString() + "```");
        }

        private async Task SendWelcomeMessage(CommandEventArgs e)
        {
            await e.Channel.SendMessage("Welcome, " + 
                e.User.Mention + 
                "! PopRPG is a little side project, but I hope you will enjoy it. For instructions, type: **`*explain poprpg`**");
        }
    }
}