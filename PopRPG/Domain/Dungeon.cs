using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Dungeon
    {
        private static double chanceToFindBoss = 100; //chance in percent
        private List<string> drops;//this could/should be a new class
        private List<Monster> enemies;
        private Random random;

        public Dungeon(Random random)
        {
            Boss = new Monster { Name = "Slime Boss" };
            enemies = new List<Monster>();
            enemies.Add(new Monster { Name = "Slime" });
            this.random = random;
        }

        public bool IsCleared { get;}
        public Monster Boss { get; }
        public string Name { get; set; }
        public int RewardExp { get; set; }
        
        public Monster GetRandomMonster()
        {
            int dice = random.Next(1, 100);
            if(dice <= chanceToFindBoss)
            {
                return Boss;
            }
            int pos = random.Next(0, enemies.Count - 1);
            return enemies[pos];
        }
    }
}
