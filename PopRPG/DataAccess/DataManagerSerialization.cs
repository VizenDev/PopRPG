using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccess
{
    public class DataManagerSerialization : DataManager
    {
        private Random random;
        public DataManagerSerialization(Random random)
        {
            this.random = random;
            SetDungeons();
        }
        //This should be 2 methods called GetDungeons and SetDungeons
        private void SetDungeons()
        {
            Stream stream = File.Open("rpg/PopRPG/DungeonInfo.osl", FileMode.OpenOrCreate);
            BinaryFormatter bformatter = new BinaryFormatter();
            List<Dungeon> dungeons = new List<Dungeon>();

            dungeons.Add(new Dungeon(random) { Name = "Potato dungeon", RewardExp = 25, RewardItem = "Healing Potion" });
            dungeons.Add(new Dungeon(random) { Name = "Iron dungeon", RewardExp = 100 });
            dungeons.Add(new Dungeon(random) { Name = "Steel dungeon", RewardExp = 150 });
            dungeons.Add(new Dungeon(random) { Name = "The Secret Pits", RewardExp = 500 });
            bformatter.Serialize(stream, dungeons);
            stream.Close();

            Stream stream2 = File.Open("rpg/PopRPG/DungeonInfo.osl", FileMode.Open);
            BinaryFormatter bformatter2 = new BinaryFormatter();
            List<Dungeon> dungeons2 = new List<Dungeon>();

            dungeons2.Add(new Dungeon(random) { Name = "Potato dungeon", RewardExp = 25, RewardItem = "Healing Potion" });
            dungeons2.Add(new Dungeon(random) { Name = "Iron dungeon", RewardExp = 100 });
            dungeons2.Add(new Dungeon(random) { Name = "Steel dungeon", RewardExp = 150 });
            dungeons2.Add(new Dungeon(random) { Name = "The Secret Pits", RewardExp = 500 });
            bformatter.Serialize(stream2, dungeons2);
            stream2.Close();
        }
        public List<Dungeon> Dungeons
        {
            get
            {
                Stream stream = File.Open("rpg/PopRPG/DungeonInfo.osl", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                List<Dungeon> ret = (List<Dungeon>)bformatter.Deserialize(stream);
                stream.Close();
                return ret;
            }
            set
            {
            }
        }

        public Player GetPlayer(ulong userId)
        {
            if (!File.Exists("rpg/PopRPG/PlayerInfo.osl"))
            {
                SetPlayer(userId, Player.NewPlayerInstance());
            }
            Stream stream = File.Open("rpg/PopRPG/PlayerInfo.osl", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            Player ret = (Player)bformatter.Deserialize(stream);
            stream.Close();
            return ret;
        }

        public Dungeon GetRandomDungeonInPlayerRange(Player p)
        {
            int pos = random.Next(0, Dungeons.Count - 1);
            return Dungeons[pos];
        }

        public void SetPlayer(ulong userId, Player player)
        {
            Stream stream = File.Open("rpg/PopRPG/PlayerInfo.osl", FileMode.OpenOrCreate);
            BinaryFormatter bformatter = new BinaryFormatter();
            try
            {

                bformatter.Serialize(stream, player);
            }
            catch (Exception e)
            {
                var a = 0;
            }
            stream.Close();
        }
    }
}
