using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.IO;
using System.Reflection;

namespace DataAccess
{
    public class DataManagerText : DataManager
    {
        private static string location = "rpg/PopRPG/";
        private static string extension = ".txt";
        private Random random;

        public DataManagerText(Random r)
        {
            random = r;
        }

        public List<Dungeon> Dungeons
        {
            get
            {
                List<Dungeon> dungeons = new List<Dungeon>();
                dungeons.Add(new Dungeon(random) { Name = "Test dungeon", RewardExp = 500 });
                return dungeons;
            }
        }

        //It requires the values to be in the correct order
        //It ignores everything after the last value
        public Player GetPlayer(ulong userId)
        {
            bool firstTimePlaying = false;
            return GetPlayerAux(userId, firstTimePlaying);
        }

        public void SetPlayer(ulong userId, Player player)
        {
            string path = location + userId + extension;
            WritePlayerFile(path, player);
        }

        private Player GetPlayerAux(ulong userId, bool firstTimePlaying)
        {
            string path = location + userId + extension;
            if (!File.Exists(path))
            {
                WritePlayerFile(path, Player.NewPlayerInstance());
                firstTimePlaying = true;
                return GetPlayerAux(userId, firstTimePlaying);
            }
            else
            {
                StreamReader sr = new StreamReader(path, true);
                Player player = new Player();
                LoadName(player, sr);
                LoadWeapon(player, sr);
                LoadArmor(player, sr);
                LoadExp(player, sr);
                LoadLevel(player, sr);
                LoadHp(player, sr);
                player.IsFirstTimePlaying = firstTimePlaying;
                sr.Close();
                return player;
            }
        }

        private void WritePlayerFile(string path, Player player)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            File.WriteAllText(path, player.ToString());
        }
        private void LoadName(Player player, StreamReader sr)
        {
            LoadStringValue(player, sr, "Name");
        }

        private void LoadWeapon(Player player, StreamReader sr)
        {
            LoadStringValue(player, sr, "Weapon");
        }
        private void LoadArmor(Player player, StreamReader sr)
        {
            LoadStringValue(player, sr, "Armor");
        }
        private void LoadExp(Player player, StreamReader sr)
        {
            LoadIntValue(player, sr, "XP");
        }
        private void LoadLevel(Player player, StreamReader sr)
        {
            LoadIntValue(player, sr, "Level");
        }
        private void LoadHp(Player player, StreamReader sr)
        {
            string[] line = sr.ReadLine().Split(':');
            if (line.Length != 2)
            {
                throw new ArgumentException("Wrong amount of parameters on the player's HP data");
            }
            if (line[0] != "HP")
            {
                throw new ArgumentException("Wrong key on the player's HP data");
            }
            double val = double.Parse(line[1].Trim().Split('/')[0]);
            player.HP = val;
        }

        private void LoadIntValue(Player player, StreamReader sr, string value)
        {
            string[] line = sr.ReadLine().Split(':');
            if (line.Length != 2)
            {
                throw new ArgumentException("Wrong amount of parameters on the player's " + value + " data");
            }
            if (line[0] != value)
            {
                throw new ArgumentException("Wrong key on the player's " + value + " data");
            }
            int val = int.Parse(line[1].Trim());
            player.GetType().GetProperty(value).SetValue(player, val);
        }

        private void LoadStringValue(Player player, StreamReader sr, string value)
        {
            string[] line = sr.ReadLine().Split(':');
            if (line.Length != 2)
            {
                throw new ArgumentException("Wrong amount of parameters on the player's " + value + " data");
            }
            if (line[0] != value)
            {
                throw new ArgumentException("Wrong key on the player's " + value + " data");
            }
            player.GetType().GetProperty(value).SetValue(player, line[1]);
        }

        //This method should filter the dungeons by the range of the player's level
        //ie: There could be a Dungeon A that only allows players of Lv 1-40 to enter it
        public Dungeon GetRandomDungeonInPlayerRange(Player p)
        {
            int pos = random.Next(0, Dungeons.Count - 1);
            return Dungeons[pos];
        }
    }
}
