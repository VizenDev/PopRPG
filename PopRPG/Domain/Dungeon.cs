using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class Dungeon : ISerializable
    {
        private static double chanceToFindBoss = 5; //chance in percent
        private List<string> drops;//this could/should be a new class
        private Random random;

        public Dungeon(SerializationInfo info, StreamingContext context)
        {
            chanceToFindBoss = (double)info.GetValue("ChanceToFindBoss", typeof(double));
            ChanceToFindItem = (double)info.GetValue("ChanceToFindItem", typeof(double));
            ChanceToFindMonster = (double)info.GetValue("ChanceToFindMonster", typeof(double));
            Bosses = (List<Monster>)info.GetValue("Bosses", typeof(List<Monster>));
            Enemies = (List<Monster>)info.GetValue("Enemies", typeof(List<Monster>));
            Name = (string)info.GetValue("Name", typeof(string));
            RewardExp = (int)info.GetValue("RewardExp", typeof(int));
            RewardItem = (string)info.GetValue("RewardItem", typeof(string));
            random = new Random();
        }

        public Dungeon(Random random)
        {
            ChanceToFindMonster = 20;
            ChanceToFindItem = 50;
            drops = new List<string>();
            Bosses = new List<Monster>();
            Bosses.Add(new Monster { Name = "Slime Boss" });
            Bosses.Add(new Monster { Name = "Venompod Boss" });
            Bosses.Add(new Monster { Name = "Banemorph Boss" });
            Bosses.Add(new Monster { Name = "Stinktooth Boss" });

            Enemies = new List<Monster>();
            Enemies.Add(new Monster { Name = "Slime", XP = 10, Level = 1 });
            Enemies.Add(new Monster { Name = "Venompod", XP = 25, Level = 2 });
            Enemies.Add(new Monster { Name = "Banemorph", XP = 50, Level = 5 });
            Enemies.Add(new Monster { Name = "Stinktooth", XP = 100, Level = 15 });
            this.random = random;
        }

        public bool IsCleared { get; }
        public List<Monster> Bosses { get; }
        public List<Monster> Enemies { get; }
        public string Name { get; set; }
        public int RewardExp { get; set; }
        public string RewardItem { get; set; }
        public double ChanceToFindMonster { get; }
        public double ChanceToFindItem { get; }

        public Monster GetRandomMonster()
        {
            int dice = random.Next(1, 100);
            if (dice <= chanceToFindBoss)
            {
                int pos = random.Next(0, Bosses.Count - 1);
                return Bosses[pos];
            }
            int pos2 = random.Next(0, Enemies.Count - 1);
            return Enemies[pos2];
        }
        public string GetRandomDrop()
        {
            int pos = random.Next(0, drops.Count - 1);
            return drops[pos];
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ChanceToFindBoss", chanceToFindBoss);
            info.AddValue("ChanceToFindItem", ChanceToFindItem); 
            info.AddValue("ChanceToFindMonster", ChanceToFindMonster); 
            info.AddValue("Bosses", Bosses); 
            info.AddValue("Enemies", Enemies); 
            info.AddValue("Name", Name);
            info.AddValue("RewardExp", RewardExp); 
            info.AddValue("RewardItem", RewardItem); 
        }
    }
}
