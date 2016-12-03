using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface DataManager
    {
        List<Dungeon> Dungeons { get; }
        Dungeon GetRandomDungeonInPlayerRange(Player p);
        Player GetPlayer(ulong userId);
        void SetPlayer(ulong userId, Player player);
    }
}
