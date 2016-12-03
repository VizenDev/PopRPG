using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Player
    {
        private bool firstTimePlaying;

        public Player()
        {
            firstTimePlaying = true;
        }
        public bool IsFirstTimePlaying()
        {
            if (firstTimePlaying)
            {
                firstTimePlaying = false;
                return true;
            }
            return false;
        }
        public int Level { get; set; }
        public int Exp { get; set; }
        public double Hp { get; set; }
        public string Armor { get; set; }
        public string Weapon { get; set; }

        public override string ToString()
        {
            string ret = "Weapon: "+Weapon +"\n"
                + "Armor: " + Armor + "\n"
                + "XP: " + Exp + "\n"
                + "Level: " + Level
                + "HP: " + Hp + "/" + MaxHpAtMyLevel(Level);
            return ret;
        }

        private double MaxHpAtMyLevel(int level)
        {
            return 20;
        }
    }
}
