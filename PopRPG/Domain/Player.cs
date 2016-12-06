using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class Player : Fighter, ISerializable
    {
        public readonly static int IN_DUNGEON_STATE = 0;
        public readonly static int IN_FIGHT_STATE = 1;
        public readonly static int NOT_IN_DUNGEON_STATE = 2;
        
        public Player()
        {
            State = NOT_IN_DUNGEON_STATE;
            IsFirstTimePlaying = true;
        }
        public Player(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("name", typeof(string));
            Weapon = (string)info.GetValue("weapon", typeof(string));
            Armor = (string)info.GetValue("armor", typeof(string));
            Items = (Dictionary<string, int>)info.GetValue("items", typeof(Dictionary<string, int>));
            XP = (int)info.GetValue("xp", typeof(int));
            Level = (int)info.GetValue("level", typeof(int));
            HP = (double)info.GetValue("hp", typeof(double));
            IsFirstTimePlaying = (bool)info.GetValue("isFirstTimePlaying", typeof(bool));
            State = (int)info.GetValue("state", typeof(int));
        }
        public Dungeon CurrentDungeon { get; set; }
        public int State { get; set; }
        public bool IsFirstTimePlaying { get; set; }
        public void AddItem(string item)
        {
            if (Items.ContainsKey(item))
            {
                Items[item]++;
            }
            else
            {
                Items.Add(item, 1);
            }
        }
        public void RemoveItem(string item)
        {
            Items.Remove(item);
        }
        public void AddExp(int exp)
        {
            XP += exp;
        }
        public void RemoveExp(int exp)
        {
            XP -= exp;
        }
        public void AddLevel(int level)
        {
            Level += level;
        }
        public void RemoveLevel(int level)
        {
            Level -= level;
        }
        public void AddHP(double hp)
        {
            HP += hp;
        }
        public void RemoveHP(double hp)
        {
            HP -= hp;
        }
        public static Player NewPlayerInstance()
        {
            Player p = new Player();
            p.Name = "Potato player";
            p.Weapon = "Potato Sword[+1]";
            p.Armor = "Potato Armor[+0.5]";
            p.Items = new Dictionary<string, int>();
            p.XP = 0;
            p.Level = 1;
            p.HP = 10;
            return p;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("weapon", Weapon);
            info.AddValue("armor", Armor);
            info.AddValue("items", Items);
            info.AddValue("xp", XP);
            info.AddValue("level", Level);
            info.AddValue("hp", HP);
            info.AddValue("isFirstTimePlaying", IsFirstTimePlaying);
            info.AddValue("state", State);
        }
    }
}
