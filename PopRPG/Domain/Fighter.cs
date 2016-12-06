using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public abstract class Fighter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public double HP { get; set; }
        public string Armor { get; set; }
        public string Weapon { get; set; }
        public Dictionary<string, int> Items { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name: " + Name);
            sb.AppendLine("Weapon: " + Weapon);
            sb.AppendLine("Armor: " + Armor);
            sb.AppendLine("Items: " + Items);
            sb.AppendLine("XP: " + XP);
            sb.AppendLine("Level: " + Level);
            sb.AppendLine("HP: " + HP + "/" + MaxHp(Level));
            return sb.ToString();
        }

        public double MaxHp(int level)
        {
            return 10 * level;
        }
    }
}
