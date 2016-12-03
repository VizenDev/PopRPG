using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Player
    {

        public Player()
        {
        }
        public bool IsFirstTimePlaying { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public double HP { get; set; }
        public string Armor { get; set; }
        public string Weapon { get; set; }

        public override string ToString()
        {
            string ret = "Weapon: "+Weapon +"\n"
                + "Armor: " + Armor + "\n"
                + "XP: " + XP + "\n"
                + "Level: " + Level + "\n"
                + "HP: " + HP + "/" + MaxHpAtMyLevel(Level);
            return ret;
        }

        private double MaxHpAtMyLevel(int level)
        {
            return 20;
        }
    }
}
