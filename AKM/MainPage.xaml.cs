namespace AKM
{
    public partial class MainPage : ContentPage
    {
        Player[] playerList = new Player[0];
        public static int playerCount = 0;
        string[] uploadedImageSources = new string[4]; // Array to store uploaded image sources

        public MainPage()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// Updates the Player Array and adds a new player there.
        /// </summary>
        /// <param name="player">The added player</param>
        private void UpdatePlayerList(Player player)
        {
            Player[] newPlayerList = new Player[playerList.Length + 1];
            for (int i = 0; i < playerList.Length; i++)
            {
                newPlayerList[i] = playerList[i];
            }
            newPlayerList[newPlayerList.Length - 1] = player;
            playerList = newPlayerList;
        }
        /// <summary>
        /// Adds a new player to the Player Array, and displays his name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePlayerBtn_Clicked(object sender, EventArgs e)
        {
            if (playerList.Length < 4)
            {
                WelcomeLbl.Text = NameEnt.Text;
                playerCount++;
                Player player = new Player(NameEnt.Text);
                UpdatePlayerList(player);
            }
        }

        
        /// <summary>
        /// When clicked, it opens the file explorer and lets you pick an icon picture for your player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UploadButton_Clicked(object sender, EventArgs e)
        {
            var pickedFile = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select an Image"
            });

            if (pickedFile != null)
            {
                var stream = await pickedFile.OpenReadAsync();
                if (playerCount <= 4)
                {
                    uploadedImageSources[playerCount] = pickedFile.FullPath; // Assign the uploaded image source to the corresponding index
                }
            }
        }
        /// <summary>
        /// When clicked, checks if the game has enough players. If not, sends an alert message. 
        /// if There are enough players, it sends the array of players to the Game Page code-behind,
        /// as well as the image sources for the player icons. Then opens the Game Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Clicked(object sender, EventArgs e)
        {
            if (playerCount < 2)
            {
                DisplayAlert("Starting Cancelled", "Please add more players. The game won't start until\n there's 2!", "OK");
            }
            else
            {
                GamePage.playerList = playerList;
                GamePage.uploadedImageSources = uploadedImageSources; // Assign the array of image sources
                Application.Current.MainPage = new NavigationPage(new GamePage());
            }
        }
    }
}