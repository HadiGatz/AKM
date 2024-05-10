using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class TrainTile : Tile
    {
        private int price;
        public Player owner = null;

        public TrainTile(int price, string name) : base(name)
        {
            this.price = price;
        }
        public bool BuyProperty(Player player) // Checks if the player owns all train tiles before buying
        {
            if (player.GetMoney() >= price && player.GetTrainsOwned() <= 4)
            {
                player.SetMoney(player.GetMoney() - price);
                owner = player;
                owner.SetTrainsOwned(owner.GetTrainsOwned() + 1);
                return true;
            }
            return false;
        }

        public int CalculateTrainBill(Player player) // the formula is amount of trains * 50
        {
            int bill;
            if (player.GetTrainsOwned() <= 1)
                bill = 50;
            else
                bill = 50 * player.GetTrainsOwned();
            player.SetMoney(player.GetMoney() - bill);
            owner.SetMoney(owner.GetMoney() + bill);
            return bill;
        }
        public int GetPrice() { return price; }
        public Player GetOwner() { return owner; }
    }
}
