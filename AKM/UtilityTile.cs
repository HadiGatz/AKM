using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class UtilityTile : Tile
    {
        private int price;
        public Player owner = null;
        private bool DoubleOwned = false;
        public UtilityTile(int price, string name) : base(name)
        {
            this.price = price;
        }

        public void CalculateBill(Player player, int currentDice)
        {
            if (!DoubleOwned)
            {
                player.SetMoney(player.GetMoney() - 4 * currentDice);
                owner.SetMoney(owner.GetMoney() + 4 * currentDice);
            }
            else
            {
                player.SetMoney(player.GetMoney() - 10 * currentDice);
                owner.SetMoney(owner.GetMoney() + 10 * currentDice);
            }
        }

        public bool BuyProperty(Player player)
        {
            if (player.GetMoney() >= price)
            {
                player.SetMoney(player.GetMoney() - price);
                owner = player;
                return true;
            }
            return false;
        }
        public int GetPrice() 
        {
            return price;
        }
        public Player GetOwner() { return owner; }
    }
}
