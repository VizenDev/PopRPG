using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Dungeon
    {
        private static double chanceToFindBoss = 5; //chance in percent
        private List<string> drops;//this could/should be a new class
        private Random random;

        public Dungeon(Random random)
        {
            Boss = new List<Monster>();
            Boss.Add(new Monster { Name= "Slime Boss"} );
            Boss.Add(new Monster { Name= "Venompod Boss" } );
            Boss.Add(new Monster { Name= "Banemorph Boss" } );
            Boss.Add(new Monster { Name= "Stinktooth Boss" } );

            Enemy = new List<Monster>();
            Enemy.Add(new Monster { Name = "Slime", XP = 10, Level = 1 });
            Enemy.Add(new Monster { Name = "Venompod", XP = 25, Level = 2 });
            Enemy.Add(new Monster { Name = "Banemorph", XP = 50, Level = 5 });
            Enemy.Add(new Monster { Name = "Stinktooth", XP = 100, Level = 15 });
            this.random = random;
        }

        public bool IsCleared { get;}
        public List<Monster> Boss { get; }
        public List<Monster> Enemy { get; }
        public string Name { get; set; }
        public int RewardExp { get; set; }
        public int EnemyExp { get; set; }
        public string RewardItem { get; set; }
        
        public Monster GetRandomMonster()
        {
            int dice = random.Next(1, 100);
            if (dice <= chanceToFindBoss)
            {
                int pos = random.Next(0, Boss.Count - 1);
                return Boss[pos];
            }
            int pos2 = random.Next(0, Enemy.Count - 1);
            return Enemy[pos2];
        }
    }
}
