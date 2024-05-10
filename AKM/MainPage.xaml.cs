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
        private void StartBtn_Clicked(object sender, EventArgs e)
        {
            GamePage.playerList = playerList;
            GamePage.uploadedImageSources = uploadedImageSources; // Assign the array of image sources
            Application.Current.MainPage = new NavigationPage(new GamePage());
        }
    }
}