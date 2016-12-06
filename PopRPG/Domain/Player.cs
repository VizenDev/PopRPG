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
        public bool IsNotInDungeon()
        {
            return CurrentDungeon == null;
        }
        public bool IsFirstTimePlaying { get; set; }
        public void ChangeName(string name)
        {
            Name = name;
        }
        public void ChangeWeapon(string weapon)
        {
            Name = weapon;
        }
        public void ChangeArmor(string armor)
        {
            Name = armor;
        }
        public void AddItem(string item)
        {
            Items = item;
        }
        public void RemoveItem(string item)
        {
            Items = "";
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
            HP = hp;
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
            p.Items = "No Items";
            p.XP = 0;
            p.Level = 1;
            p.HP = 10;
            return p;
        }
    }
}
