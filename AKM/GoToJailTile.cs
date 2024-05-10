using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class GoToJailTile : Tile
    {
        public GoToJailTile(string name) : base(name)
        {
            this.name = name;
        }

        public void GoToJail(Player player)
        {
            player.SetIndex(10);
            player.SetIsInJail(true);
        }
    }
}
