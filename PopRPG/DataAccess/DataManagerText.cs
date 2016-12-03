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
            string path = location + userId + extension;
            if (!File.Exists(path))
            {
                return new Player();
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
                return player;
            }
        }

        private void LoadWeapon(Player player, StreamReader sr)
        {
            LoadValue(player, sr, "Weapon");
        }
        private void LoadArmor(Player player, StreamReader sr)
        {
            LoadValue(player, sr, "Armor");
        }
        private void LoadExp(Player player, StreamReader sr)
        {
            LoadValue(player, sr, "Exp");
        }
        private void LoadLevel(Player player, StreamReader sr)
        {
            LoadValue(player, sr, "Level");
        }
        private void LoadHp(Player player, StreamReader sr)
        {
            LoadValue(player, sr, "Hp");
        }

        private void LoadValue(Player player, StreamReader sr, string value)
        {
            string[] line = sr.ReadLine().Split(':');
            if (line.Length != 2)
            {
                throw new ArgumentException("Wrong amount of parameters on the player's "+value +" data");
            }
            if (line[0] != value)
            {
                throw new ArgumentException("Wrong key on the player's " + value + " data");
            }
            player.GetType().GetProperty(value).SetValue(player, line[1]);
        }
    }
}
