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
        public string Items { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name: " + Name);
            sb.AppendLine("Weapon: " + Weapon);
            sb.AppendLine("Armor: " + Armor);
            sb.AppendLine("Items: " + Items);
            sb.AppendLine("XP: " + XP);
            sb.AppendLine("Level: " + Level);
            sb.AppendLine("HP: " + HP + "/" + hp(Level));
            return sb.ToString();
        }

        public double hp(int level)
        {
            return 10 * level;
        }
    }
}
