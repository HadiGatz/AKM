using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    class BuildTile : Tile
    {
        public int price;
        private int startingRent;
        private int rentWithHouse;
        private int rentWith4Houses;
        private int rentWithHotel;
        private int housePrice;
        private int hotelPrice;
        private Player owner;
        private bool isAvailable;
        private string Color;
        private bool canBuild = false;
        private int houses = 0;
        private int hotels = 0;


        public BuildTile(int price, string name, int startingRent, int rentWithHouse, int rentWith4Houses, int rentWithHotel,
            int housePrice, int hotelPrice, string Color)
            : base(name)
        {
            this.price = price;
            this.startingRent = startingRent;
            this.rentWithHouse = rentWithHouse;
            this.rentWithHotel = rentWithHotel;
            this.rentWith4Houses = rentWith4Houses;
            this.housePrice = housePrice;
            this.hotelPrice = hotelPrice;
            this.Color = Color;
        }
        public int CalculateRent()
        {
            int rent;
            if (houses > 0 && houses < 4)
            {
                rent = houses * rentWithHouse;
                owner.SetMoney(owner.GetMoney() + rent);
                return rent;
            }

            else if (houses == 4)
            {
                rent = houses * rentWith4Houses;
                owner.SetMoney(owner.GetMoney() + rent);
                return rent;
            }

            else if (hotels > 0)
            {
                rent = houses * rentWithHotel;
                owner.SetMoney(owner.GetMoney() + rent);
                return rent;
            }
            owner.SetMoney(owner.GetMoney() + startingRent);
            return startingRent;
        }

        public bool GetIsAvailable()
        {
            return isAvailable;
        }
        public void SetIsAvailable(bool newValue)
        {
            isAvailable = newValue;
        }
        


        /// <summary>
        /// This method checks the house, hotel and price members, then builds a house/hotel.
        /// </summary>
        /// <returns> Whether the build was successful </returns>
        public bool BuildHouseOrHotel()

        {
            if (canBuild && owner.GetMoney() >= housePrice && houses < 3)
            {
                owner.SetMoney(owner.GetMoney() - housePrice);
                houses++;
                return true;
            }
            else if (canBuild && owner.GetMoney() >= hotelPrice && houses == 4)
            {
                owner.SetMoney(owner.GetMoney() - hotelPrice);
                hotels++;
                houses = 0;
                canBuild = false;
                return true;
            }
            return false;
        }
        public bool BuildHotel()
        {
            if (canBuild && owner.GetMoney() >= hotelPrice && houses == 4)
            {
                owner.SetMoney(owner.GetMoney() - hotelPrice);
                hotels++;
                houses = 0;
                return true;
            }
            return false;
        }
        public string GetColor()
        {
            return Color;
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
        public bool CheckColorSet(Tile[] board, Player player)
        {
            for (int i = player.GetIndex() - 2; i <=  player.GetIndex() + 2; i++) 
            { 
                if (board[i] is BuildTile buildTile)
                {
                    if (this.Color == buildTile.Color)
                    {
                        if (this.owner != buildTile.owner)
                            return false;
                    }
                }
            }
            return true;
        }
        public Player GetOwner()
        {
            return owner;
        }
        public bool GetCanBuild() 
        { 
            return canBuild; 
        }
        public int GetHouses()
        { return houses; }
        public int GetHotels()
        { return hotels; }
        public int GetPrice() { return price; }

    }
}
