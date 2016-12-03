using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class Fighter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public double HP { get; set; }
        public string Armor { get; set; }
        public string Weapon { get; set; }
        public override string ToString()
        {
            string ret = "Name: "+ Name + "\r\n"
                + "Weapon: " + Weapon + "\r\n"
                + "Armor: " + Armor + "\r\n"
                + "XP: " + XP + "\r\n"
                + "Level: " + Level + "\r\n"
                + "HP: " + HP + "/" + MaxHpAtMyLevel(Level);
            return ret;
        }

        private double MaxHpAtMyLevel(int level)
        {
            return 10;
        }
    }
}
