using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKM
{
    public class GameManagement
    {
        Tile[] board;
        Player[] players;
        Player currentPlayer;
        int currentDice = 0;
        int dice1 = 0;
        int dice2 = 0;
        private Random rnd;
        private bool gameOver = false;

        public GameManagement(Player[] players)
        {
            this.players = players;
            currentPlayer = players[0];
            rnd = new Random();
        }
        /// <summary>
        /// Using .Next() it updates the dice members in the class. Then updates the currentDice member,
        /// and moves the player according to the results.
        /// </summary>
        /// <param name="player"></param>
        public void RollDice(Player player)
        {
            dice1 = rnd.Next(1, 7);
            dice2 = rnd.Next(1, 7);
            currentDice = dice1 + dice2;
            MovePlayer(player, currentDice);
        }
        /// <summary>
        /// Changes the index of a player, checks if the index exceeds the board limits
        /// </summary>
        /// <param name="player"></param>
        /// <param name="moveNum"></param>
        public void MovePlayer(Player player, int moveNum)
        {
            player.SetIndex((player.GetIndex() + moveNum) % 40);
        }
        public void InitializeBoard()
        {
            board = new Tile[] {
                new GoTile("Go"),
                new BuildTile(60, "Mediterranean Avenue", 2, 10, 30, 90, 50, 50, "Brown"),
                new SurpriseTile("Community Chest 1"),
                new BuildTile(60, "Baltic Avenue", 4, 20, 60, 180, 50, 50, "Brown"),
                new TaxTile(200, "Income Tax"),
                new TrainTile(200, "Reading Railroad"),
                new BuildTile(100, "Oriental Avenue", 6, 30, 90, 270, 50, 50, "LightBlue"),
                new SurpriseTile("Chance 1"),
                new BuildTile(100, "Vermont Avenue", 6, 30, 90, 270, 50, 50, "LightBlue"),
                new BuildTile(100, "Connecticut Avenue", 8, 40, 100, 300, 50, 50, "LightBlue"),
                new JailTile("Jail"),
                new BuildTile(140, "St. Charles Place", 10, 50, 150, 450, 100, 100, "Pink"),
                new UtilityTile(150, "Electric Company"),
                new BuildTile(140, "States Avenue", 10, 50, 150, 450, 100, 100, "Pink"),
                new BuildTile(160, "Virginia Avenue", 12, 60, 180, 500, 100, 100, "Pink"),
                new TrainTile(200, "Pennsylvania Railroad"),
                new BuildTile(180, "St. James Place", 14, 70, 200, 550, 100, 100, "Orange"),
                new SurpriseTile("Community Chest 2"),
                new BuildTile(180, "Tennessee Avenue", 14, 70, 200, 550, 100, 100, "Orange"),
                new BuildTile(200, "New York Avenue", 16, 80, 220, 600, 100, 100, "Orange"),
                new Tile("Free Parking"),
                new BuildTile(220, "Kentucky Avenue", 18, 90, 250, 700, 150, 150, "Red"),
                new SurpriseTile("Chance 2"),
                new BuildTile(220, "Indiana Avenue", 18, 90, 250, 700, 150, 150, "Red"),
                new BuildTile(240, "Illinois Avenue", 20, 100, 300, 750, 150, 150, "Red"),
                new TrainTile(200, "B. & O. Railroad"),
                new BuildTile(260, "Atlantic Avenue", 22, 110, 330, 800, 150, 150, "Yellow"),
                new BuildTile(260, "Ventnor Avenue", 22, 110, 330, 800, 150, 150, "Yellow"),
                new UtilityTile(150, "Water Works"),
                new BuildTile(280, "Marvin Gardens", 24, 120, 360, 850, 150, 150, "Yellow"),
                new GoToJailTile("Go To Jail"),
                new BuildTile(300, "Pacific Avenue", 26, 130, 390, 900, 200, 200, "Green"),
                new BuildTile(300, "North Carolina Avenue", 26, 130, 390, 900, 200, 200, "Green"),
                new SurpriseTile("Community Chest 3"),
                new BuildTile(320, "Pennsylvania Avenue", 28, 150, 450, 1000, 200, 200, "Green"),
                new TrainTile(200, "Short Line"),
                new SurpriseTile("Chance 3"),
                new BuildTile(350, "Park Place", 35, 175, 500, 1100, 200, 200, "Blue"),
                new TaxTile(100, "Luxury Tax"),
                new BuildTile(400, "Boardwalk", 50, 200, 600, 1400, 200, 200, "Blue") };
        }
        /// <summary>
        /// Called when the current player is in jail. Handles his turn according to the jail rules
        /// (3 rolls until freedom, double rolling)
        /// </summary>
        /// <param name="player"></param>
        public void HandleJailRoll(Player player)
        {
            if (player.GetJailRollsRemaining() > 0)
            {
                RollDice(player);
                if (dice1 == dice2) // Double rolling sets the player free.
                {
                    player.SetIsInJail(false);
                    player.SetJailRollsRemaining(0);
                }
                else
                {
                    player.SetJailRollsRemaining(player.GetJailRollsRemaining() - 1);
                }
            }
            else
            {
                player.SetMoney(player.GetMoney() - 50); // The player pays 50 to get out of jail, unwillingly
                player.SetIsInJail(false);
            }
        }
        public void Game()
        {
            InitializeBoard();

            while (!gameOver)
            {
                if (currentPlayer.GetIsInJail())
                {
                    HandleJailRoll(currentPlayer);
                    continue;
                }
                else
                {
                    RollDice(currentPlayer);;
                    Tile currentTile = board[currentPlayer.GetIndex()];

                    if (currentTile is BuildTile buildTile)
                    {
                        buildTile.SetIsAvailable(buildTile.CheckColorSet(board, currentPlayer));
                        if (buildTile.GetIsAvailable())
                        {
                            buildTile.BuyProperty(currentPlayer, buildTile);
                            buildTile.SetIsAvailable(false);
                        }
                        else if (buildTile.GetOwner() != currentPlayer)
                        {
                            int rent = buildTile.CalculateRent();
                            currentPlayer.SetMoney(currentPlayer.GetMoney() - rent);
                        }
                        if (buildTile.GetOwner() == currentPlayer)
                        {
                            buildTile.SetIsAvailable(buildTile.CheckColorSet(board, currentPlayer));
                        }
                    }
                    else if (currentTile is GoTile goTile)
                    {
                        goTile.AddGoMoney(currentPlayer);
                    }
                    else if (currentTile is SurpriseTile surpriseTile)
                    {
                      //  surpriseTile.GenerateSurprise(currentPlayer);
                    }
                    else if (currentTile is TaxTile taxTile)
                    {
                        taxTile.TakeTax(currentPlayer);
                    }
                    else if (currentTile is UtilityTile utilityTile)
                    {
                        if (utilityTile.owner == null)
                        {
                            utilityTile.BuyProperty(currentPlayer, utilityTile);
                        }
                        else if (utilityTile.owner != currentPlayer)
                        {
                            utilityTile.CalculateBill(currentPlayer, currentDice);
                        }
                    }
                    else if (currentTile is TrainTile trainTile)
                    {
                        if (trainTile.owner == null)
                        {
                            trainTile.BuyProperty(currentPlayer, trainTile);
                        }
                        else if (trainTile.owner != currentPlayer)
                        {
                            trainTile.CalculateTrainBill(currentPlayer);
                        }
                    }

                }
                GameStateCheck();
                if (!(dice1 == dice2)) 
                {
                    currentPlayer = players[(Array.IndexOf(players, currentPlayer) + 1) % players.Length];
                }
            }
        }
        public void GameStateCheck()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetMoney() <= 0)
                {
                    Player[] newPlayers = new Player[players.Length - 1];
                    players[i] = null;
                    for (int j = 0; i < newPlayers.Length; j++)
                    {
                        if (players[j] != null)
                            newPlayers[j] = players[j];
                    }
                    players = newPlayers;
                }
            }
            gameOver = players.Length == 1;
        }
        public Player GetCurrentPlayer()
        { 
            return currentPlayer; 
        }
        public int GetDice1()
        {
            return dice1;
        }
        public int GetDice2()
        {
            return dice2;
        }
        public Tile[] GetBoard()
        { 
            return board; 
        }
        public Player[] GetPlayers()
        {
            return players;
        }
        public int GetCurrentDice() { return currentDice; }
        /// <summary>
        /// Sets the current player, if the player is the last one on the array it switches to the first one.
        /// </summary>
        public void SetCurrentPlayer()
        {
            if (Array.IndexOf(players, currentPlayer) == players.Length - 1)
            {
                currentPlayer = players[0];
            }
            else
            {
                currentPlayer = players[Array.IndexOf(players, currentPlayer) + 1];
            }
        }
        
    }
}
