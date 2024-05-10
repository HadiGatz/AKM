using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class TaxTile : Tile
    {
        private int tax;

        public TaxTile(int tax, string name) : base(name)
        {
            this.tax = tax;
        }

        public int GetTax()
        { return tax; }
        public void TakeTax(Player player)
        {
            player.SetMoney(player.GetMoney() - tax);
        }


    }
}
