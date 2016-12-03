using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.IO;

namespace DataAccess
{
    public class DataManagerText : DataManager
    {
        private static string location = "rpg/PopRPG/";
        private static string extension = ".txt";

        //It requires the values to be in the correct order
        //It ignores everything after the last value
        public Player GetPlayer(ulong userId)
        {
            bool firstTimePlaying = false;
            return GetPlayerAux(userId, firstTimePlaying);
        }

        private Player GetPlayerAux(ulong userId, bool firstTimePlaying)
        {
            string path = location + userId + extension;
            if (!File.Exists(path))
            {
                WritePlayerFile(path);
                firstTimePlaying = true;
                return GetPlayerAux(userId, firstTimePlaying);
            }
            else
            {
                StreamReader sr = new StreamReader(path, true);
                Player player = new Player();
                LoadWeapon(player, sr);
                LoadArmor(player, sr);
                LoadExp(player, sr);
                LoadLevel(player, sr);
                LoadHp(player, sr);
                player.IsFirstTimePlaying = firstTimePlaying;
                return player;
            }
        }

        private void WritePlayerFile(string path)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            StreamWriter sWrite = new StreamWriter(path);
            StringBuilder text = new StringBuilder(); ;
            text.AppendLine("Weapon: Potato Sword[+1]");
            text.AppendLine("Armor: Potato Armor[+0.5]");
            text.AppendLine("XP: 0");
            text.AppendLine("Level: 1");
            text.AppendLine("HP: 20");
            sWrite.Write(text);
            sWrite.Close();
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
            LoadDoubleValue(player, sr, "HP");
        }
        private void LoadDoubleValue(Player player, StreamReader sr, string value)
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
    }
}
