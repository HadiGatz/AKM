using AKM;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AKM
{
    public partial class GamePage : ContentPage
    {
        public static Player[] playerList;
        public static string currentTileName;
        
        public static GameManagement gameManager;
        public static string[] uploadedImageSources;



        public GamePage() //TODO FOR TOMORROW: 1. implement the methods for all buttons. 2. multiple player logic. 3. moving logic. 4. design.
            // 5. dbugging and documentation.
        {

            InitializeComponent();
            gameManager = new GameManagement(playerList);
            gameManager.InitializeBoard();
            CurrentPlayerLabel.Text = gameManager.GetCurrentPlayer().GetName();
            CreatePlayerInfo(playerList, uploadedImageSources);
            string imageName = gameManager.GetBoard()[gameManager.GetCurrentPlayer().GetIndex()].GetCardName();
            currentTileName = imageName;
            CurrentCardImage.Source = $"{imageName}.png";
            HideBuildMenu();
            HideGoMenu();
            HideSurpriseMenu();
            HideTaxMenu();
            HideTrainMenu();
            HideUtilityMenu();
            HideJailMenu();
            CreateMovingPlayerIcons();
        }

        private void RollDiceButton_Clicked(object sender, EventArgs e)
        {
            gameManager.RollDice(gameManager.GetCurrentPlayer());
            MoveAllPlayersOnBoard();
            Dice1Label.Text = gameManager.GetDice1().ToString();
            Dice2Label.Text = gameManager.GetDice2().ToString();
            string imageName = gameManager.GetBoard()[gameManager.GetCurrentPlayer().GetIndex()].GetCardName();
            CurrentCardImage.Source = $"{imageName}.png";
            UpdateTileContent(gameManager.GetBoard()[gameManager.GetCurrentPlayer().GetIndex()]);
            if (gameManager.GetDice1() != gameManager.GetDice2())
            {
                RollDiceButton.IsVisible = false;
            }
            else
            {
                gameManager.GetCurrentPlayer().SetDoubleCounter(gameManager.GetCurrentPlayer().GetDoubleCounter() + 1);
                if (gameManager.GetCurrentPlayer().GetDoubleCounter() == 3)
                {
                    gameManager.GetCurrentPlayer().SetIsInJail(true);
                }
            }
        }
        private void HandleJailTile(JailTile jailTile, Player currentPlayer)
        {
            JailLabel.Text = $"You're in jail!\nYou have {currentPlayer.GetJailRollsRemaining()} more turns \nin jail.";
            JailLabel.IsVisible = true;
        }
        private void HandleGoToJailTile(GoToJailTile goToJailTile)
        {
            JailLabel.Text = $"Good luck in Jail!";
            JailLabel.IsVisible = true;
        }
        private void HandleBuyableBuildTile(BuildTile buildTile)
        {
            BuildTileOwnerNameLabel.IsVisible = true;
            BuildTileOwnerNameLabel.Text = "No one owns this tile. You can buy it!";
            BuyPropertyButton.IsVisible = true;
            BuyHouseOrHotelButton.IsVisible = false;
            RentFineLabel.IsVisible = false;
            CanBuildLabel.IsVisible = false;
            HowMuchRentLabel.IsVisible = true;
            HousesAndHotelsLabel.IsVisible = false;
            TradeButton.IsVisible = true;
            HousesAndHotelsLabel.Text = "No Houses/Hotels";
        }
        private void HandleOwnedBuildTile(BuildTile buildTile)
        {
            BuildTileOwnerNameLabel.IsVisible = true;
            BuildTileOwnerNameLabel.Text = $"The Owner is {buildTile.GetOwner().GetName()}.";
            BuyPropertyButton.IsVisible = false;
            BuyHouseOrHotelButton.IsVisible = false;
            RentFineLabel.IsVisible = true;
            CanBuildLabel.IsVisible = false;
            HowMuchRentLabel.IsVisible = false;
            HousesAndHotelsLabel.IsVisible = true;
            RentFineLabel.Text = $"Your rent is {buildTile.CalculateRent()}";
        }
        private void HandleOwnedByPlayerBuildTile(BuildTile buildTile)
        {
            BuildTileOwnerNameLabel.IsVisible = true;
            BuildTileOwnerNameLabel.Text = $"You are the owner!";
            BuyPropertyButton.IsVisible = false;
            BuyHouseOrHotelButton.IsVisible = true;
            BuyHouseOrHotelButton.BackgroundColor = buildTile.GetCanBuild() ? Color.FromRgba("FFC300") : Color.FromRgba("#808080");
            BuyHouseOrHotelButton.Text = buildTile.GetHouses() == 4 ? "Buy Hotel" : "Buy House";
            RentFineLabel.IsVisible = false;
            CanBuildLabel.IsVisible = true;
            CanBuildLabel.Text = $"Can Build: {buildTile.GetCanBuild().ToString()}";
            HousesAndHotelsLabel.IsVisible = true;
            HousesAndHotelsLabel.Text = $"{buildTile.GetHouses()} / {buildTile.GetHotels()}";
            HowMuchRentLabel.IsVisible = true;
        }
        private void HandleGoTile()
        {
            GoTileLabel.IsVisible = true;
        }
        private void HandleSurpriseTile(SurpriseTile surpriseTile)
        {
            string surpriseText = surpriseTile.RandomSurpriseString();
            int surpriseMoney = surpriseTile.GenerateSurpriseMoney(gameManager.GetCurrentPlayer());
            SurpriseTileLabel.IsVisible= true;
            SurpriseTileLabel.Text = surpriseText;
            SurpriseMoneyLabel.IsVisible = true;
            SurpriseMoneyLabel.Text = surpriseMoney > 0 ? $"+{surpriseMoney}$" : $"-{surpriseMoney}$";
            SurpriseMoneyLabel.TextColor = surpriseMoney > 0 ? Color.FromRgba("#00FF00") : Color.FromRgba("#8B0000");
        }
        private void HandleTaxTile(TaxTile taxTile)
        {
            TaxTextLabel.IsVisible = true;
            TaxMoneyLabel.IsVisible = true;
            taxTile.TakeTax(gameManager.GetCurrentPlayer());
            TaxMoneyLabel.Text = $"-{taxTile.GetTax()}";
        }
        private void HandleBuyableTrainTile(TrainTile trainTile)
        {
            TrainTileOwnerLabel.IsVisible = true;
            TrainTileOwnerLabel.Text = $"Owner: No one! You can buy it\nThe price is {trainTile.GetPrice()}";
            BuyTrainPropertyButton.IsVisible = true;
            TrainRentLabel.IsVisible = false;
        }
        private void HandleOwnedTrainTile(TrainTile trainTile)
        {
            TrainTileOwnerLabel.IsVisible = true;
            TrainTileOwnerLabel.Text = $"Owner: {trainTile.GetOwner().GetName()}";
            TrainRentLabel.IsVisible = true;
            TrainRentLabel.Text = $"Hope you enjoyed the ride!\nThe bill is {trainTile.CalculateTrainBill(gameManager.GetCurrentPlayer())}";
            BuyTrainPropertyButton.IsVisible = false;
        }
        private void HandleOwnedByPlayerTrainTile()
        {
            TrainTileOwnerLabel.IsVisible = true;
            TrainTileOwnerLabel.Text = $"You're the owner!";
            BuyTrainPropertyButton.IsVisible = false;
            TrainRentLabel.IsVisible = false;
        }
        private void HandleBuyableUtilityTile(UtilityTile utilityTile)
        {
            UtilityTileOwnerLabel.IsVisible = true;
            UtilityTileOwnerLabel.Text = $"Owner: No one! You can buy it\nThe price is {utilityTile.GetPrice()}";
            BuyUtilityPropertyButton.IsVisible = true;
            UtilityRentLabel.IsVisible = false;
        }
        private void HandleOwnedUtilityTile(UtilityTile utilityTile)
        {
            UtilityTileOwnerLabel.IsVisible = true;
            UtilityTileOwnerLabel.Text = $"Owner: {utilityTile.GetOwner().GetName()}";
            UtilityRentLabel.IsVisible = true;
            //UtilityRentLabel.Text = $"Utilities aren't free, you know.\nThe bill is {utilityTile.CalculateBill(gameManager.GetCurrentPlayer(), gameManager.GetCurrentDice())}";

        }
        private void HandleOwnedByPlayerUtilityTile(UtilityTile utilityTile)
        {
            UtilityTileOwnerLabel.IsVisible = true;
            UtilityTileOwnerLabel.Text = $"You're the owner!";
            BuyUtilityPropertyButton.IsVisible = false;
            UtilityRentLabel.IsVisible = false;
        }
        private void HideBuildMenu()
        {  
            BuildTileOwnerNameLabel.IsVisible = false;
            BuildTileOwnerNameLabel.IsVisible = false;
            BuyPropertyButton.IsVisible = false;
            BuyHouseOrHotelButton.IsVisible = false;
            RentFineLabel.IsVisible = false;
            CanBuildLabel.IsVisible = false;
            HowMuchRentLabel.IsVisible = false;
            HousesAndHotelsLabel.IsVisible = false;
            BoughtPropertyTileLabel.IsVisible = false;
            TradeButton.IsVisible = false;
        }
        private void HideGoMenu()
        {
            GoTileLabel.IsVisible = false;
        }
        private void HideSurpriseMenu()
        {
            SurpriseMoneyLabel.IsVisible = false;
            SurpriseTileLabel.IsVisible = false;
        }
        private void HideTaxMenu()
        {
            TaxMoneyLabel.IsVisible = false;
            TaxTextLabel.IsVisible= false;
        }
        private void HideTrainMenu()
        {
            TrainTileOwnerLabel.IsVisible = false;
            BuyTrainPropertyButton.IsVisible = false;
            TrainRentLabel.IsVisible= false;
            BoughtPropertyTileLabel.IsVisible = false;
        }
        private void HideUtilityMenu()
        {
            UtilityTileOwnerLabel.IsVisible = false;
            UtilityRentLabel.IsVisible = false;
            BuyUtilityPropertyButton.IsVisible = false;
            BoughtPropertyTileLabel.IsVisible = false;
        }
        private void HideJailMenu()
        {
            JailLabel.IsVisible = false;
        }
       
        private void UpdateTileContent(Tile currentTile)
        {
            if (currentTile is BuildTile buildTile)
            {
                HideGoMenu();
                HideSurpriseMenu();
                HideTaxMenu();
                HideTrainMenu();
                HideUtilityMenu();
                HideJailMenu();
                HowMuchRentLabel.Text = $"Rent: {buildTile.CalculateRent()}";
                if (buildTile.GetOwner() == null)
                {
                    HandleBuyableBuildTile(buildTile);
                }
                else if (buildTile.GetOwner() != gameManager.GetCurrentPlayer())
                {
                    HandleOwnedBuildTile(buildTile);
                }
                else
                {
                    HandleOwnedByPlayerBuildTile(buildTile);
                }
            }
            else if (currentTile is JailTile jailTile && gameManager.GetCurrentPlayer().GetIsInJail())
            {
                HideGoMenu();
                HideSurpriseMenu();
                HideTaxMenu();
                HideTrainMenu();
                HideBuildMenu();
                HideUtilityMenu();
                HandleJailTile(jailTile, gameManager.GetCurrentPlayer());
            }
            else if (currentTile is GoToJailTile goToJailTile)
            {
                HideGoMenu();
                HideSurpriseMenu();
                HideTaxMenu();
                HideTrainMenu();
                HideBuildMenu();
                HideUtilityMenu();
                HandleGoToJailTile(goToJailTile);
            }

            else if (currentTile is GoTile goTile)
            {
                HideBuildMenu();
                HideSurpriseMenu();
                HideTaxMenu();
                HideTrainMenu();
                HandleGoTile();
                HideUtilityMenu();
                HideJailMenu();
            }
            else if (currentTile is SurpriseTile surpriseTile)
            {
                HideBuildMenu();
                HideGoMenu();
                HideTaxMenu();
                HideTrainMenu();
                HideUtilityMenu();
                HideJailMenu();
                HandleSurpriseTile(surpriseTile);
            }
            else if (currentTile is TaxTile taxTile)
            {
                HideBuildMenu();
                HideGoMenu();
                HideTrainMenu();
                HideUtilityMenu();
                HideSurpriseMenu();
                HideJailMenu();
                HandleTaxTile(taxTile);
            }
            else if (currentTile is UtilityTile utilityTile)
            {
                HideBuildMenu();
                HideGoMenu();
                HideTrainMenu();
                HideTaxMenu();
                HideJailMenu();
                HideSurpriseMenu();
                if (utilityTile.owner == null)
                {
                    HandleBuyableUtilityTile(utilityTile);
                }
                else if (utilityTile.owner != gameManager.GetCurrentPlayer())
                {
                    HandleOwnedUtilityTile(utilityTile);
                }
                else
                {
                    HandleOwnedByPlayerUtilityTile(utilityTile);
                }
            }
            else if (currentTile is TrainTile trainTile)
            {
                HideBuildMenu();
                HideGoMenu();
                HideSurpriseMenu();
                HideTaxMenu();
                HideJailMenu();
                HideUtilityMenu();
                if (trainTile.owner == null)
                {
                    HandleBuyableTrainTile(trainTile);
                }
                else if (trainTile.owner != gameManager.GetCurrentPlayer())
                {
                    HandleOwnedTrainTile(trainTile);
                }
                else
                {
                    HandleOwnedByPlayerTrainTile();
                }
            }
        }

        private void BuyHouseOrHotelButton_Clicked(object sender, EventArgs e) 
        {
            HideBuildMenu();
            if (gameManager.GetBoard()[gameManager.GetCurrentPlayer().GetIndex()] is BuildTile buildTile)
            {
                buildTile.BuildHouseOrHotel();
                if (!buildTile.BuildHouseOrHotel())
                {
                    DisplayAlert("Buying Cancelled", "You don't have the whole color set, or enough money. Maybe you don't have 4 houses yet.", "Thank you, IRS");
                }
                else
                {
                    HideBuildMenu();
                    BoughtPropertyTileLabel.IsVisible = true;
                    BoughtPropertyTileLabel.Text = $"Congratulations!\nYou now have {buildTile.GetHouses()} houses and {buildTile.GetHotels()} hotels";
                }   
            }
            UpdatePlayerInfo();
        }
        private void HidePlayerInfo()
        {
            Player3Info.IsVisible = false;
            Player4Info.IsVisible = false;
            PlayerInfo.IsVisible = true;
            Player2Info.IsVisible = true;
            if (gameManager.GetPlayers().Length > 2)
            {
                Player3Info.IsVisible = true;
            }
            if (gameManager.GetPlayers().Length > 3)
            { 
                Player4Info.IsVisible = true;
            }

        }
        private void CreatePlayerInfo(Player[] playerList, string[] uploadedImageSources)
        {
            HidePlayerInfo();

            PlayerNameLabel.IsVisible = true;
            PlayerNameLabel.Text = playerList[0].GetName();
            PlayerMoneyLabel.IsVisible = true;
            PlayerMoneyLabel.Text = playerList[0].GetMoney().ToString();
            SetPlayerIcon(PlayerIconImage, uploadedImageSources[0]); // Set player icon for player 1

            if (playerList.Length > 1)
            {
                Player2Info.IsVisible = true;
                Player2NameLabel.IsVisible = true;
                Player2NameLabel.Text = playerList[1].GetName();
                Player2MoneyLabel.IsVisible = true;
                Player2MoneyLabel.Text = playerList[1].GetMoney().ToString();
                SetPlayerIcon(Player2IconImage, uploadedImageSources[1]); // Set player icon for player 2
            }

            if (playerList.Length > 2)
            {
                Player3Info.IsVisible = true;
                Player3NameLabel.IsVisible = true;
                Player3NameLabel.Text = playerList[2].GetName();
                Player3MoneyLabel.IsVisible = true;
                Player3MoneyLabel.Text = playerList[2].GetMoney().ToString();
                SetPlayerIcon(Player3IconImage, uploadedImageSources[2]); // Set player icon for player 3
            }

            if (playerList.Length > 3)
            {
                Player4Info.IsVisible = true;
                Player4NameLabel.IsVisible = true;
                Player4NameLabel.Text = playerList[3].GetName();
                Player4MoneyLabel.IsVisible = true;
                Player4MoneyLabel.Text = playerList[3].GetMoney().ToString();
                SetPlayerIcon(Player4IconImage, uploadedImageSources[3]); // Set player icon for player 4
            }
        }
        private void CreateMovingPlayerIcons()
        {
            SetPlayerIcon(MovingPlayer1, uploadedImageSources[0]);
            SetPlayerIcon(MovingPlayer2, uploadedImageSources[1]);
            if (playerList.Length > 2)
            {
                SetPlayerIcon(MovingPlayer3, uploadedImageSources[2]);
            }
            if (playerList.Length > 3)
            {
                SetPlayerIcon(MovingPlayer4, uploadedImageSources[3]);
            }
            
        }

        private void SetPlayerIcon(Image playerIconImage, string imageSource)
        {
            if (string.IsNullOrEmpty(imageSource))
                return;

            playerIconImage.Source = ImageSource.FromFile(imageSource);
        }

        private void UpdatePlayerInfo() 
        {
            PlayerMoneyLabel.Text = gameManager.GetPlayers()[0].GetMoney().ToString();
            Player2MoneyLabel.Text = gameManager.GetPlayers()[1].GetMoney().ToString();
            if (gameManager.GetPlayers().Length > 2)
            {
                Player3MoneyLabel.Text = gameManager.GetPlayers()[2].GetMoney().ToString();
            }
            if (gameManager.GetPlayers().Length > 3)
            {
                Player4MoneyLabel.Text = gameManager.GetPlayers()[3].GetMoney().ToString();
            }
            CurrentPlayerLabel.Text = $"Current Player: {gameManager.GetCurrentPlayer().GetName()}";
        }
        private void BuyPropertyButton_Clicked(object sender, EventArgs e)
        {
            var currentPlayer = gameManager.GetCurrentPlayer();
            var currentTile = gameManager.GetBoard()[currentPlayer.GetIndex()];

            if (currentTile is BuildTile buildTile)
            {
                bool boughtProperty = buildTile.BuyProperty(currentPlayer, buildTile);

                if (!boughtProperty)
                {
                    DisplayAlert("Buying Cancelled", "Try to get more money, Dingus", "I love Capitalism");
                }
                else
                {
                    HideBuildMenu();
                    BoughtPropertyTileLabel.IsVisible = true;
                    BoughtPropertyTileLabel.Text = $"Congratulations!\nYou have bought {buildTile.GetName()}";
                }
            }
            else if (currentTile is TrainTile trainTile)
            {
                bool boughtProperty = trainTile.BuyProperty(currentPlayer, trainTile);

                if (!boughtProperty)
                {
                    DisplayAlert("Buying Cancelled", "Try to get more money, Dingus", "I love Capitalism");
                }
                else
                {
                    HideTrainMenu();
                    BoughtPropertyTileLabel.IsVisible = true;
                    BoughtPropertyTileLabel.Text = $"Congratulations!\nYou have bought {trainTile.GetName()}";
                }
            }
            else if (currentTile is UtilityTile utilityTile)
            {
                bool boughtProperty = utilityTile.BuyProperty(currentPlayer, utilityTile);

                if (!boughtProperty)
                {
                    DisplayAlert("Buying Cancelled", "Try to get more money, Dingus", "I love Capitalism");
                }
                else
                {
                    HideUtilityMenu();
                    BoughtPropertyTileLabel.IsVisible = true;
                    BoughtPropertyTileLabel.Text = $"Congratulations!\nYou have bought {utilityTile.GetName()}";
                }
            }

            UpdatePlayerInfo();
        }

        private void SkipTurnButton_Clicked(object sender, EventArgs e)
        {

            gameManager.SetCurrentPlayer();
            if (gameManager.GetCurrentPlayer().GetIsInJail())
            {
                gameManager.HandleJailRoll(gameManager.GetCurrentPlayer());
            }
            gameManager.GameStateCheck();


            UpdatePlayerInfo();
            HideBuildMenu();
            HideGoMenu();
            HideSurpriseMenu();
            HideTaxMenu();
            HideTrainMenu();
            HideUtilityMenu();
            RollDiceButton.IsVisible = true;
        }
        /// <summary>
        /// Moves the player according to the Pixels on the board image. 
        /// </summary>
        /// <param name="playerIcon"></param>
        /// <param name="player"></param>
        /// <param name="gameManager"></param>
        private void MovePlayerOnBoard(Image playerIcon, Player player, GameManagement gameManager)
        {
            int StartingY = -160 + (10 * (Array.IndexOf(gameManager.GetPlayers(), player)));
            int StartingX = -135;
            if (player.GetIndex() >= 0 && player.GetIndex() < 4)
            {
                playerIcon.TranslationX = StartingX + (27 * (player.GetIndex()));
                playerIcon.TranslationY = StartingY;
            }
            else if (player.GetIndex() >= 4 && player.GetIndex() <= 9)
            {
                playerIcon.TranslationX = StartingX + (28 * player.GetIndex());
                playerIcon.TranslationY = StartingY;
            }
            else if (player.GetIndex() == 10)
            {
                playerIcon.TranslationY = -160;
                playerIcon.TranslationX = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() == 11)
            {
                playerIcon.TranslationY = -110;
                playerIcon.TranslationX = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() >= 11 && player.GetIndex() <= 14)
            {
                playerIcon.TranslationY = -110 + 28 * (player.GetIndex() - 9);
                playerIcon.TranslationX = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() >= 15 && player.GetIndex() < 20)
            {
                playerIcon.TranslationY = 27 * (player.GetIndex() - 9);
                playerIcon.TranslationX = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() == 20)
            {
                playerIcon.TranslationX = 130;
                playerIcon.TranslationY = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() >= 20 && player.GetIndex() <= 24)
            {
                playerIcon.TranslationX = 130 - 27 * (player.GetIndex() - 19);
                playerIcon.TranslationY = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() >= 25 && player.GetIndex() < 30)
            {
                playerIcon.TranslationX = -28 * (player.GetIndex() - 25);
                playerIcon.TranslationY = 130 + 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() == 30)
            {
                TranslationY = 130;
                TranslationX = -130 - 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() > 29 && player.GetIndex() <= 34)
            {
                TranslationY = 130 - 28 * (player.GetIndex() - 29);
                TranslationX = -130 - 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            else if (player.GetIndex() >= 35 && player.GetIndex() < 39)
            {
                TranslationY = 130 - 27 * (player.GetIndex() - 29);
                TranslationX = -130 - 10 * (Array.IndexOf(gameManager.GetPlayers(), player));
            }
            
        }
        private void MoveAllPlayersOnBoard()
        {
            MovePlayerOnBoard(MovingPlayer1, gameManager.GetPlayers()[0], gameManager);
            MovePlayerOnBoard(MovingPlayer2, gameManager.GetPlayers()[1], gameManager);
            if (gameManager.GetPlayers().Length > 2)
            {
                MovePlayerOnBoard(MovingPlayer3, gameManager.GetPlayers()[2], gameManager);
            }
            if (gameManager.GetPlayers().Length > 3)
            {
                MovePlayerOnBoard(MovingPlayer4, gameManager.GetPlayers()[3], gameManager);
            }
            
        }

        private void PlayerInventoryButton_Clicked(object sender, EventArgs e)
        {
            if (sender == PlayerInventoryButton)
            {
                DisplayAlert("Inventory", gameManager.GetPlayers()[0].PrintInventory(), "OK");
            }
            if (sender == Player2InventoryButton)
            {
                DisplayAlert("Inventory", gameManager.GetPlayers()[1].PrintInventory(), "OK");
            }
            if (sender == Player3InventoryButton)
            {
                DisplayAlert("Inventory", gameManager.GetPlayers()[2].PrintInventory(), "OK");
            }
            if (sender == Player4InventoryButton)
            {
                DisplayAlert("Inventory", gameManager.GetPlayers()[3].PrintInventory(), "OK");
            }
        }

        private void TradeButton_Clicked(object sender, EventArgs e)
        {
            TradeWindow tradeWindow = new TradeWindow(this, gameManager.GetCurrentPlayer());
            
            Navigation.PushAsync(tradeWindow);
        }
    }
}