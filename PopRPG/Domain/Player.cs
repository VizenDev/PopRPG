using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Player : Fighter
    {

        public Player()
        {
        }
        public Dungeon CurrentDungeon { get; set; }
        public bool IsInDungeon()
        {
            return CurrentDungeon != null;
        }
        public bool IsFirstTimePlaying { get; set; }
        public void AddExp(int exp)
        {
            XP += exp;
        }

        public static Player NewPlayerInstance()
        {
            Player p = new Player();
            p.Name = "Potato player";
            p.Weapon = "Potato Sword[+1]";
            p.Armor = "Potato Armor[+0.5]";
            p.XP = 0;
            p.Level = 1;
            p.HP = 20;
            return p;
        }
    }
}
