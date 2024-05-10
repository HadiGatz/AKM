using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class GoTile : Tile
    {
        public GoTile(string name) : base(name)
        {
            this.name = name;
        }

        public void AddGoMoney(Player player)
        {
            player.SetMoney(player.GetMoney() + 200);
        }
    }
}
