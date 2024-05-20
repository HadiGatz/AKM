using Microsoft.Maui.Controls;

namespace AKM
{
    public partial class TradeWindow : ContentPage
    {
        int highestOffer;
        string currentTileName;
        Player highestOfferPlayer;
        Player clickedPlayer;
        private GamePage gamePage;
        public TradeWindow(GamePage gamePage, Player clickedPlayer)
        {
            InitializeComponent();
            highestOffer = 0;
            currentTileName = GamePage.gameManager.GetBoard()[GamePage.gameManager.GetCurrentPlayer().GetIndex()].GetCardName();
            TileImage.Source = $"{currentTileName}.png";
            CreateUI();
            this.gamePage = gamePage;
            this.clickedPlayer = clickedPlayer;
        }
        /// <summary>
        /// Creates the UI for the trade window, according to the number of players
        /// </summary>
        private void CreateUI()
        {
            Player3Menu.IsVisible = false;
            Player4Menu.IsVisible = false;
            Player1Name.Text = GamePage.playerList[0].GetName();
            Player1Money.Text = $"Money: {GamePage.playerList[0].GetMoney()}$";

            Player2Name.Text = GamePage.playerList[1].GetName();
            Player2Money.Text = $"Money: {GamePage.playerList[1].GetMoney()}";

            
            if (GamePage.playerList.Length >= 3)
            {
                Player3Menu.IsVisible = true;
                Player3Name.Text = GamePage.playerList[2].GetName();
                Player3Money.Text = $"Money: {GamePage.playerList[2].GetMoney()}";
            }
            if (GamePage.playerList.Length >= 4)
            {
                Player4Menu.IsVisible = true;
                Player4Name.Text = GamePage.playerList[3].GetName();
                Player4Money.Text = $"Money: {GamePage.playerList[3].GetMoney()}";
            }
        }
        /// <summary>
        /// Updates the highest offer label
        /// </summary>
        private void UpdateUI()
        {
            HighestOfferLabel.Text = $"The highest Offer\nIs {highestOffer}$";
        }
        /// <summary>
        /// When the offer button clicked, a display allert is shown
        /// asking the player to enter their offer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OfferButton_Clicked(object sender, EventArgs e)
        {
            Player clickedPlayer = null;
            string offer = await DisplayPromptAsync("Your offer", "How much would you pay for the property?");
            if (offer == null)
            {
                return;
            }
            int intOffer = int.Parse(offer);
            if (sender == Player1Button)
            {
                clickedPlayer = GamePage.playerList[0];
                if(intOffer > GamePage.playerList[0].GetMoney())
                {
                    await DisplayAlert("Offer Cancelled", "You don't have enough money!", "Dingus");
                }
                else
                    Player1Money.Text = $"Money: {(GamePage.playerList[0].GetMoney() - intOffer).ToString()}$";
            }
            if (sender == Player2Button)
            {
                clickedPlayer = GamePage.playerList[1];
                if (intOffer > GamePage.playerList[1].GetMoney())
                {
                    await DisplayAlert("Offer Cancelled", "You don't have enough money!", "Dingus");
                }
                else
                     Player2Money.Text = $"Money: {(GamePage.playerList[0].GetMoney() - intOffer).ToString()}$";
            }
            if (sender == Player3Button)
            {
                clickedPlayer = GamePage.playerList[2];
                if (intOffer > GamePage.playerList[2].GetMoney())
                {
                    await DisplayAlert("Offer Cancelled", "You don't have enough money!", "Dingus");
                }
                else
                     Player3Money.Text = $"Money: {(GamePage.playerList[0].GetMoney() - intOffer).ToString()}$";
            }
            if (sender == Player4Button)
            {
                clickedPlayer = GamePage.playerList[3];
                if (intOffer > GamePage.playerList[3].GetMoney())
                {
                    await DisplayAlert("Offer Cancelled", "You don't have enough money!", "Dingus");
                }
                else
                      Player4Money.Text = $"Money: {(GamePage.playerList[0].GetMoney() - intOffer).ToString()}$";
            }
            if (intOffer >  highestOffer)
            {
                highestOffer = intOffer;
                highestOfferPlayer = clickedPlayer;
                UpdateUI();
            }
        }
        
        private void ReturnToGameButton_Clicked(object sender, EventArgs e)
        {

            if (GamePage.gameManager.GetBoard()[GamePage.gameManager.GetCurrentPlayer().GetIndex()] is BuildTile buildTile)
            {
                buildTile.SetPrice(highestOffer);
                buildTile.BuyProperty(highestOfferPlayer, buildTile);
            }
            clickedPlayer.SetMoney(clickedPlayer.GetMoney() + highestOffer);
            Navigation.PopAsync();
            gamePage.Focus();
            //TODO: find a way to open the game page, fix bug with card picture and bug with incorrect money on player menu
        }

    }
}