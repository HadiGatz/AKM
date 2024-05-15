using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    public class Player
    {
        private string name; // is going to be used in the UI
        private int money = 1500;
        private int index; // place of player
        private bool IsInJail = false; // Is in Jail
        private int trainsOwned; // how many tiles of type TrainTile are owned
        private int jailRollsRemaining; // Remaining rolls to get out of jail
        private int doubleCounter = 0;
        List<Tile> ownedTiles = new List<Tile>();

        public Player(string name)
        {
            this.name = name;
        }

        public void SetMoney(int newMoney)
        {
            money = newMoney;
        }
        public int GetMoney()
        {
            return money;
        }

        public int GetIndex()
        {
            return index;
        }
        public void SetIndex(int newIndex)
        {
            index = (index + newIndex) % 40;
        }
        public bool GetIsInJail()
        {
            return IsInJail;
        }
        public void SetIsInJail(bool newValue)
        {
            IsInJail = newValue;
        }
        public int GetTrainsOwned()
        {
            return trainsOwned;
        }
        public void SetTrainsOwned(int newAmount)
        {
            trainsOwned = newAmount;
        }
        public int GetJailRollsRemaining()
        {
            return jailRollsRemaining;
        }
        public void SetJailRollsRemaining(int newAmount)
        {
            jailRollsRemaining = newAmount;
        }
        public string GetName()
        {
            return name;
        }
        public int GetDoubleCounter() { return doubleCounter; }
        public void SetDoubleCounter(int value) { doubleCounter = value; }
        public void AddOwnedTile(Tile tile)
        {
            if (tile is BuildTile buildTile)
            {
                ownedTiles.Add(buildTile);
            }
            if (tile is TrainTile trainTile)
            {
                ownedTiles.Add(trainTile);
            }
            if (tile is UtilityTile utilTile)
            {
                ownedTiles.Add(utilTile);
            }
        }
        public string PrintInventory()
        {
            string inventory = $"{name} Owns: \n";
            for (int i = 0; i < ownedTiles.Count; i++)
            {
                inventory += $"{ownedTiles[i].GetName()},\n";
            }
            inventory += $"His total balance is {money}$";
            return inventory;
        }
    }
}
