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
        private readonly static double MIN_DAMAGE_TAKEN_RUNNING = 1.5;
        private readonly static double MAX_DAMAGE_TAKEN_RUNNING = 2.5;

        private DiscordClient client;
        private ModuleManager mod;
        private RuntimeEnvironment re;
        private static Random rnd = new Random();
        private DataManager dataManager = new DataManagerSerialization(rnd);
        private Player player;
        //self.cs shouldn't have an instance of player. It should 
        //have an instance of a service of players, which would call the dataaccess
        //to modify the data file

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
                        player.IsFirstTimePlaying = false;
                        dataManager.SetPlayer(e.User.Id, player);
                    }
                    else
                    {
                        await e.Channel.SendMessage($"Oooooh. Welcome back {player.Name}. I am sure you know what to do.");
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
                    await SendStatsMessage(e);
                    Console.WriteLine($"{e.User.Name} performed the **stats** command.");
                });
                c.CreateCommand("explore").Do((e) =>
                {
                    if (player.State == Player.NOT_IN_DUNGEON_STATE)
                    {
                        EnterDungeon(e);
                    }
                    else
                    {
                        e.Channel.SendMessage("You are already in a dungeon");
                    }
                    Console.WriteLine($"{e.User.Name} performed the **explore** command.");
                });
                c.CreateCommand("attack").Do(async (e) => 
                {
                    if (player.State == Player.IN_DUNGEON_STATE)
                    {
                        Monster m = await FindMonster(e);
                        await FightMonster(e, m);
                    }
                    else if(player.State == Player.IN_FIGHT_STATE)
                    {
                        await e.Channel.SendMessage("You're already in a fight");
                    }
                    else
                    {
                        await e.Channel.SendMessage("Enter a dungeon to use this command");
                    }

                    Console.WriteLine($"{e.User.Name} performed the **attack** command.");
                });
                c.CreateCommand("run").Do(async (e) =>
                {
                    double newHP = HPTakenAfterRunningAway();
                    var takenHP = Math.Abs(newHP).ToString().Substring(0, 3);
                    if (player.State == Player.IN_DUNGEON_STATE || player.State == Player.IN_FIGHT_STATE)
                    {
                        await e.Channel.SendMessage("You decided to run away. Doing so got you hit with " + takenHP + " damage.");
                        player.RemoveHP(Convert.ToDouble(takenHP));
                        dataManager.SetPlayer(e.User.Id, player);
                        player.CurrentDungeon = null;
                        player.State = Player.NOT_IN_DUNGEON_STATE;
                        await e.Channel.SendMessage($"Your HP is now **{player.HP}/{player.MaxHp(player.Level)}**");
                    }
                    else 
                    {
                        await e.Channel.SendMessage("You can't run away from a dungeon if you aren't even in one.");
                    }
                });
                c.CreateCommand("search").Do(async (e) =>
                {
                    if (player.State == Player.IN_DUNGEON_STATE)
                    {
                        if (player.CurrentDungeon.IsCleared)
                        {
                            double chanceToFindItem = player.CurrentDungeon.ChanceToFindItem;
                            double chanceToFindMonster = player.CurrentDungeon.ChanceToFindMonster;
                            int result = rnd.Next(1, 100);
                            if (result <= chanceToFindItem)
                            {
                                await FindItem(e);
                            }
                            else if (result <= chanceToFindMonster)
                            {
                                await FindMonster(e);
                            }
                            else
                            {
                                await e.Channel.SendMessage("You didn't find anything");
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage("Clear the dungeon to use this command");
                        }
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
                            player.AddHP(player.MaxHp(player.Level));
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
        //This could change if the fight doesn't end in one turn
        private async Task FightMonster(CommandEventArgs e, Monster m)
        {
            if (player.CurrentDungeon.Bosses.Contains(m))
            {
                await e.Channel.SendMessage($"Congratulations! You have cleared this dungeon and received {player.CurrentDungeon.RewardExp} exp as reward!");
            }
            else
            {
                if (player.CurrentDungeon.RewardItem == null)
                {
                    await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received " + player.CurrentDungeon.RewardExp + " exp as reward!");
                }
                else
                {
                    player.AddItem(player.CurrentDungeon.RewardItem);
                    await e.Channel.SendMessage("Congratulations! You have cleared this dungeon and received a " + player.CurrentDungeon.RewardItem + " and " + player.CurrentDungeon.RewardExp + " exp as reward!");
                }
            }
            player.AddExp(player.CurrentDungeon.RewardExp);
            dataManager.SetPlayer(e.User.Id, player);
            player.State = Player.IN_DUNGEON_STATE;
        }
        private async Task FindItem(CommandEventArgs e)
        {
            string drop = player.CurrentDungeon.GetRandomDrop();
            await e.Channel.SendMessage($"You found {drop}");
            player.AddItem(drop);
        }
        private async Task<Monster> FindMonster(CommandEventArgs e)
        {
            player.State = Player.IN_FIGHT_STATE;
            Monster m = player.CurrentDungeon.GetRandomMonster();
            if (player.CurrentDungeon.Bosses.Contains(m))
            {
                await e.Channel.SendMessage("The boss of this dungeon has appeared! Defeat " + m.Name + " to clear the dungeon!");
                
            }
            else
            {
                await e.Channel.SendMessage($"The enemy of this dungeon has appeared! Defeat {m.Name} to clear the dungeon!");
                
            }
            return m;
        }

        private static double HPTakenAfterRunningAway()
        {
            return rnd.NextDouble() * 
                (MAX_DAMAGE_TAKEN_RUNNING - MIN_DAMAGE_TAKEN_RUNNING) 
                + MIN_DAMAGE_TAKEN_RUNNING;
        }

        private async void EnterDungeon(CommandEventArgs e)
        {
            Dungeon dungeon = dataManager.GetRandomDungeonInPlayerRange(player);
            player.CurrentDungeon = dungeon;
            player.State = Player.IN_DUNGEON_STATE;
            dataManager.SetPlayer(e.User.Id, player);
            await e.Channel.SendMessage("Welcome " + player.Name.Trim() + 
                " to the " + dungeon.Name);
        }

        private async Task SendStatsMessage(CommandEventArgs e)
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