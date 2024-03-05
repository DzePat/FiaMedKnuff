using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FiaMedKnuff
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Dictionary<int, (int, int)> boardPath = new Dictionary<int, (int, int)>();
        public Dictionary<string, (int, int)> goalPath = new Dictionary<string, (int, int)>();
        public Dictionary<string, (int, int)> goalStartTile = new Dictionary<string, (int, int)>();
        public Dictionary<string, (int, int)> goalReached = new Dictionary<string, (int, int)>();
        public Dictionary<string, (int, int)> spawnTiles = new Dictionary<string, (int, int)>();
        public Dictionary<int, (string, int)> Players = new Dictionary<int, (string, int)>();
        private string[] colors = { "Gul", "Blå", "Röd", "Grön" };
        private DispatcherTimer _animationTimer;
        private Random random = new Random();
        private int stepCount;
        private bool isSoundOn = true; //sound is on by default
        private bool isMusicOn = true;
        private MediaElement musicPlayer = new MediaElement();
        private int playerturn = 1;

        public static MainPage Instance { get; private set; }
        public Image ImageSource { get { return imageSource; } }

        public MainPage()
        {
            this.InitializeComponent();
            Instance = this;
            populateBoard();
            generatePath();
            generateGoalPath();
            generateGoalStartTiles();
            generateSpawnTiles();
            InitializeAnimationTimer();
            initMusicPlayer();
        }

        public void initializePlayers()
        {
            for (int a = 1; a < SelectPlayersPage.Instance.Players.Count + 1; a++)
            {
                string identity = SelectPlayersPage.Instance.Players[a];
                Players.Add(a, (identity, 0));
            }
            switch (Players.Count)
            {
                case 2:
                    //player 1
                    addPlayerPawns(11, 0, 1, "Gul");
                    //player 2
                    addPlayerPawns(0, 0, 2, "Blå");
                    break;
                case 3:
                    //player 1
                    addPlayerPawns(11, 0, 1, "Gul");
                    //player 2
                    addPlayerPawns(0, 0, 2, "Blå");
                    //player 3
                    addPlayerPawns(0, 11, 3, "Röd");
                    break;
                case 4:
                    //player 1
                    addPlayerPawns(11, 0, 1, "Gul");
                    //player 2
                    addPlayerPawns(0, 0, 2, "Blå");
                    //player 3
                    addPlayerPawns(0, 11, 3, "Röd");
                    //player 4
                    addPlayerPawns(11, 11, 4, "Grön");
                    break;
                default:
                    var dialog = new MessageDialog($"player Amount {Players.Count}");
                    dialog.ShowAsync();
                    break;
            }
        }

        /// <summary>
        /// Initializes the musicplayer to play the background music on permanent loop
        /// </summary>
        private async void initMusicPlayer()
        {
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file;
            file = await folder.GetFileAsync("backgroundMusic.mp3");
            musicPlayer.IsLooping = true;
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            musicPlayer.SetSource(stream, file.ContentType);
            musicPlayer.Play();
        }

        /// <summary>
        /// Start dispatch timer
        /// </summary>
        private void InitializeAnimationTimer()
        {
            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromSeconds(2);
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        /// <summary>
        /// Populate the board with tiles and pawns
        /// </summary>
        private void populateBoard()
        {
            Board.RowDefinitions.Clear();
            Board.ColumnDefinitions.Clear();

            //add rows and columns to the Board grid
            for (int i = 0; i < 13; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                Board.RowDefinitions.Add(rowDefinition);
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(50);
                Board.ColumnDefinitions.Add(columnDefinition);
            }

            //add top tiles
            for (int i = 2; i < 5; i++)
            {
                addellipse(i, 5, Colors.White);
                addellipse(i, 6, Colors.Red);
                addellipse(i, 7, Colors.White);
            }

            //add bottom tiles
            for (int i = 8; i < 11; i++)
            {
                addellipse(i, 5, Colors.White);
                addellipse(i, 6, Colors.Yellow);
                addellipse(i, 7, Colors.White);
            }
            //add left tiles
            for (int i = 2; i < 5; i++)
            {
                addellipse(5, i, Colors.White);
                addellipse(6, i, Colors.Blue);
                addellipse(7, i, Colors.White);
            }
            //add right tiles
            for (int i = 8; i < 11; i++)
            {
                addellipse(5, i, Colors.White);
                addellipse(6, i, Colors.Green);
                addellipse(7, i, Colors.White);
            }
            //add end tiles and start positions
            for (int i = 5; i < 8; i++)
            {
                if (i == 5)
                {
                    addellipse(1, i, Colors.White);
                    addellipse(11, i, Windows.UI.Color.FromArgb(100, 255, 255, 0));
                    addellipse(i, 1, Windows.UI.Color.FromArgb(100, 0, 0, 255));
                    addellipse(i, 11, Colors.White);
                }
                else if (i == 7)
                {
                    addellipse(1, i, Windows.UI.Color.FromArgb(100, 255, 0, 0));
                    addellipse(11, i, Colors.White);
                    addellipse(i, 1, Colors.White);
                    addellipse(i, 11, Windows.UI.Color.FromArgb(100, 0, 255, 0));
                }
                else
                {
                    addellipse(1, i, Colors.White);
                    addellipse(11, i, Colors.White);
                    addellipse(i, 1, Colors.White);
                    addellipse(i, 11, Colors.White);
                }
            }
            //add middle tiles
            addellipse(5, 5, Colors.White);
            addellipse(5, 7, Colors.White);
            addellipse(7, 5, Colors.White);
            addellipse(7, 7, Colors.White);
            addellipse(6, 6, Colors.Black);
            addellipse(5, 6, Colors.Red);
            addellipse(6, 5, Colors.Blue);
            addellipse(6, 7, Colors.Green);
            addellipse(7, 6, Colors.Yellow);
            //add Player Pawn Spawn tiles
            addspawntile(0, 0, Colors.Blue);
            addspawntile(0, 11, Colors.Red);
            addspawntile(11, 0, Colors.Yellow);
            addspawntile(11, 11, Colors.Green);

            imageSource.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Generate path for the board
        /// </summary>
        private void generatePath()
        {
            //Yellow Start to left
            boardPath.Add(0, (11, 5));
            boardPath.Add(1, (10, 5));
            boardPath.Add(2, (9, 5));
            boardPath.Add(3, (8, 5));
            boardPath.Add(4, (7, 5));
            // Left to top
            boardPath.Add(5, (7, 4));
            boardPath.Add(6, (7, 3));
            boardPath.Add(7, (7, 2));
            boardPath.Add(8, (7, 1));
            boardPath.Add(9, (6, 1));
            boardPath.Add(10, (5, 1));
            boardPath.Add(11, (5, 2));
            boardPath.Add(12, (5, 3));
            boardPath.Add(13, (5, 4));
            boardPath.Add(14, (5, 5));
            //Top to right
            boardPath.Add(15, (4, 5));
            boardPath.Add(16, (3, 5));
            boardPath.Add(17, (2, 5));
            boardPath.Add(18, (1, 5));
            boardPath.Add(19, (1, 6));
            boardPath.Add(20, (1, 7));
            boardPath.Add(21, (2, 7));
            boardPath.Add(22, (3, 7));
            boardPath.Add(23, (4, 7));
            boardPath.Add(24, (5, 7));
            //Right to bottom
            boardPath.Add(25, (5, 8));
            boardPath.Add(26, (5, 9));
            boardPath.Add(27, (5, 10));
            boardPath.Add(28, (5, 11));
            boardPath.Add(29, (6, 11));
            boardPath.Add(30, (7, 11));
            boardPath.Add(31, (7, 10));
            boardPath.Add(32, (7, 9));
            boardPath.Add(33, (7, 8));
            boardPath.Add(34, (7, 7));
            //Bottom to yellow start
            boardPath.Add(35, (8, 7));
            boardPath.Add(36, (9, 7));
            boardPath.Add(37, (10, 7));
            boardPath.Add(38, (11, 7));
            boardPath.Add(39, (11, 6));

        }

        /// <summary>
        /// generate a placeholder tiles for spawn tiles
        /// </summary>
        private void generateSpawnTiles()
        {
            //yellow spawn tiles
            spawnTiles.Add("Gul-1", (11, 0));
            spawnTiles.Add("Gul-2", (11, 1));
            spawnTiles.Add("Gul-3", (12, 0));
            spawnTiles.Add("Gul-4", (12, 1));
            //Blue spawn tiles
            spawnTiles.Add("Blå-1", (0, 0));
            spawnTiles.Add("Blå-2", (0, 1));
            spawnTiles.Add("Blå-3", (1, 0));
            spawnTiles.Add("Blå-4", (1, 1));
            //Red spawn tiles 
            spawnTiles.Add("Röd-1", (0, 11));
            spawnTiles.Add("Röd-2", (0, 12));
            spawnTiles.Add("Röd-3", (1, 11));
            spawnTiles.Add("Röd-4", (1, 12));
            //Green spawn tiles
            spawnTiles.Add("Grön-1", (11, 11));
            spawnTiles.Add("Grön-2", (11, 12));
            spawnTiles.Add("Grön-3", (12, 11));
            spawnTiles.Add("Grön-4", (12, 12));
        }

        /// <summary>
        /// Generate path for the Goal tiles of corresponding colors
        /// </summary>
        private void generateGoalPath()
        {
            //yellow goal path
            goalPath.Add("Gul-2", (10, 6));
            goalPath.Add("Gul-3", (9, 6));
            goalPath.Add("Gul-4", (8, 6));
            goalPath.Add("Gul-5", (7, 6));
            //blue goal path 
            goalPath.Add("Blå-2", (6, 2));
            goalPath.Add("Blå-3", (6, 3));
            goalPath.Add("Blå-4", (6, 4));
            goalPath.Add("Blå-5", (6, 5));
            //red goal path  
            goalPath.Add("Röd-2", (2, 6));
            goalPath.Add("Röd-3", (3, 6));
            goalPath.Add("Röd-4", (4, 6));
            goalPath.Add("Röd-5", (5, 6));
            //green goal path
            goalPath.Add("Grön-2", (6, 10));
            goalPath.Add("Grön-3", (6, 9));
            goalPath.Add("Grön-4", (6, 8));
            goalPath.Add("Grön-5", (6, 7));
        }

        /// <summary>
        /// Generate 4 tiles for transition between boardpath and goalpath
        /// </summary>
        private void generateGoalStartTiles()
        {
            goalStartTile.Add("Gul-1", (11, 6));
            goalStartTile.Add("Blå-1", (6, 1));
            goalStartTile.Add("Röd-1", (1, 6));
            goalStartTile.Add("Grön-1", (6, 11));
        }

        /// <summary>
        /// Adds all 4 player pawns from the top left position
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="playerID"></param>
        /// <param name="nameID"></param>
        private void addPlayerPawns(int row, int column, int playerID, string nameID)
        {
            string[] pawnPaths = new string[] {
                "/Assets/Gul.png",
                "/Assets/Blå.png",
                "/Assets/Röd.png",
                "/Assets/Grön.png",
            };

            string workingdirectory = Directory.GetCurrentDirectory();
            addPawn(row, column, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Right, VerticalAlignment.Bottom, nameID);
            addPawn(row, column + 1, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Left, VerticalAlignment.Bottom, nameID);
            addPawn(row + 1, column, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Right, VerticalAlignment.Top, nameID);
            addPawn(row + 1, column + 1, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Left, VerticalAlignment.Top, nameID);
        }

        /// <summary>
        /// adds a Pawn to the Board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="imagePath"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="NameID"></param>
        private void addPawn(int row, int column, string imagePath, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, string NameID)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = 30,
                Height = 40
            };

            rectangle.PointerPressed += Pawn_Clicked;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath));

            rectangle.Fill = imageBrush;

            rectangle.HorizontalAlignment = horizontalAlignment;
            rectangle.VerticalAlignment = verticalAlignment;
            rectangle.Name = NameID;
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            Board.Children.Add(rectangle);
        }

        /// <summary>
        /// Checks if there is an enemy pawns on the tile your pawn stands
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private async Task checkForEnemyPawns(int row, int column, string color)
        {
            foreach (object child in Board.Children)
            {
                if (child is Rectangle pawn)
                {
                    if (pawn.Name != color && Grid.GetRow(pawn) == row && Grid.GetColumn(pawn) == column)
                    {
                        resetPawn(pawn);
                    }
                }

            }
        }

        /// <summary>
        /// Move a pawn from its current position to a corresponding color spawn position
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void resetPawn(Rectangle rect)
        {
            int index = 1;
            while (index < 5)
            {
                (int row, int column) = spawnTiles[rect.Name + $"-{index}"];
                if (tileIsEmpty(row, column))
                {
                    switch (index)
                    {
                        case 1:
                            rect.HorizontalAlignment = HorizontalAlignment.Right;
                            rect.VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case 2:
                            rect.HorizontalAlignment = HorizontalAlignment.Left;
                            rect.VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case 3:
                            rect.HorizontalAlignment = HorizontalAlignment.Right;
                            rect.VerticalAlignment = VerticalAlignment.Top;
                            break;
                        case 4:
                            rect.HorizontalAlignment = HorizontalAlignment.Left;
                            rect.VerticalAlignment = VerticalAlignment.Top;
                            break;
                    }
                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, column);
                    PlaySound("eat");
                    break;
                }
                index++;
            }
        }

        /// <summary>
        /// checks if there is any pawns on the tile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool tileIsEmpty(int row, int column)
        {
            foreach (object child in Board.Children)
            {
                if (child is Rectangle pawn)
                {
                    if (Grid.GetRow(pawn) == row && Grid.GetColumn(pawn) == column)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Click event handler for pawns on the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Pawn_Clicked(object sender, PointerRoutedEventArgs e)
        {
            disableAllPawns();
            if (sender is Rectangle pawn)
            {
                int currentRow = Grid.GetRow(pawn);
                int currentColumn = Grid.GetColumn(pawn);
                int foundKey;
                // if the position of the pawn exists in the gameboard
                if (boardPath.ContainsValue((currentRow, currentColumn)) | goalPath.ContainsValue((currentRow, currentColumn)))
                {
                    while (stepCount != null & stepCount != 0)
                    {
                        // get the pawn position
                        currentRow = Grid.GetRow(pawn);
                        currentColumn = Grid.GetColumn(pawn);
                        // 'foundKey' is the current position number on the board of the clicked pawn
                        foundKey = boardPath.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key;
                        // if the pawn is on the last tile of the boardpath
                        //Ljud
                        PlaySound("walk");
                        await Task.Delay(300);

                        if (goalStartTile[pawn.Name + "-1"] == (currentRow, currentColumn))
                        {
                            // move the pawn to the next position in the goalpath
                            moveToGoalTile(pawn);
                        }
                        // if the position of the clicked pawn is in the goalpath the pawn is moved within the goalpath
                        else if (goalPath.ContainsValue((currentRow, currentColumn)))
                        {
                            moveOneGoalTile(pawn);
                        }
                        // if the boardpath contains the next position of the clicked pawn
                        else if (boardPath.ContainsKey(foundKey + 1))
                        {
                            // move the pawn to the next position in the boardpath
                            (int row, int column) = boardPath[foundKey + 1];
                            Grid.SetRow(pawn, row);
                            Grid.SetColumn(pawn, column);
                            stepCount -= 1;
                            // update 'foundKey' to the new current position number
                            foundKey += 1;
                            if (stepCount == 0)
                            {
                                await checkForEnemyPawns(row, column, pawn.Name);
                                imageSource.IsHitTestVisible = true;
                            }
                        }
                        else
                        {
                            await linkEndToStartPath(pawn);
                        }
                    }
                }
                // place the pawn on the board if the clicked pawn is in the nest
                else if (stepCount == 6 || stepCount == 1 && !goalPath.ContainsValue((currentRow, currentColumn)))
                {
                    await placepawnOnTheBoardAsync(pawn);
                }
                else if (stepCount != 0)
                {
                    stepCount = 0;
                    imageSource.IsHitTestVisible = true;
                }
            }
        }

        /// <summary>
        /// move a pawn from boardpath to goalpath
        /// </summary>
        /// <param name="rectangle"></param>
        private void moveToGoalTile(Rectangle rectangle)
        {
            (int row, int column) = goalPath[rectangle.Name + "-2"];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            stepCount -= 1;
        }

        /// <summary>
        /// sets rectangle position to first tile of the path dictionary
        /// </summary>
        /// <param name="rectangle"></param>
        private async Task linkEndToStartPath(Rectangle rectangle)
        {
            (int row, int column) = boardPath[0];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            stepCount -= 1;
            if (stepCount == 0)
            {
                await checkForEnemyPawns(row, column, rectangle.Name);
                imageSource.IsHitTestVisible = true;
            }
        }

        /// <summary>
        /// Places a pawn from the spawn point to the starting position depending on its color
        /// </summary>
        /// <param name="rectangle"></param>
        private async Task placepawnOnTheBoardAsync(Rectangle rectangle)
        {
            int startingposition;
            if (rectangle.Name.Contains("Gul"))
            {
                startingposition = 0;
            }
            else if (rectangle.Name.Contains("Blå"))
            {
                startingposition = 10;
            }
            else if (rectangle.Name.Contains("Röd"))
            {
                startingposition = 20;
            }
            else
            {
                startingposition = 30;
            }
            (int row, int column) = boardPath[startingposition];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle.VerticalAlignment = VerticalAlignment.Center;
            stepCount = 0;
            await checkForEnemyPawns(row, column, rectangle.Name);
            imageSource.IsHitTestVisible = true;
        }

        /// <summary>
        /// Moves a single tile in goalpath if all tiles infront is occupied then it occupies the current tile
        /// </summary>
        /// <param name="rectangle"></param>
        private async void moveOneGoalTile(Rectangle rectangle)
        {
            int currentRow = Grid.GetRow(rectangle);
            int currentColumn = Grid.GetColumn(rectangle);
            // get the current position of the clicked pawn
            string currentKey = goalPath.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key;
            string[] currentKeySplit = currentKey.Split('-'); //returns "[color,index]"
            // get the next position of the clicked pawn
            string newkey = $"{currentKeySplit[0]}-{int.Parse(currentKeySplit[1]) + 1}";
            // if the next position of the clicked pawn is not occupied
            if (goalPath.ContainsKey(newkey) & goalReached.ContainsKey(newkey) == false)
            {
                (int newRow, int newColumn) = goalPath[newkey];
                Grid.SetRow(rectangle, newRow);
                Grid.SetColumn(rectangle, newColumn);
                stepCount -= 1;
                //check if the current position is last in the goal tiles
                if (goalReached.ContainsKey($"{currentKeySplit[0]}-{int.Parse(currentKeySplit[1]) + 2}"))
                {
                    goalReached.Add(newkey, goalPath[newkey]);
                    rectangle.PointerPressed -= Pawn_Clicked;
                    stepCount = 0;
                    imageSource.IsHitTestVisible = true;
                }
            }
            else
            {
                goalReached.Add(currentKey, goalPath[currentKey]);
                rectangle.PointerPressed -= Pawn_Clicked;
                stepCount = 0;
            }
        }

        /// <summary>
        /// add ellipse to the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        private void addellipse(int row, int column, Color color)
        {
            Ellipse ellipse = createElipse(color, 40);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            Board.Children.Add(ellipse);
        }

        /// <summary>
        /// add player pawn Spawn tiles on the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        private void addspawntile(int row, int column, Color color)
        {
            Ellipse ellipse = createElipse(color, 100);
            Grid.SetRowSpan(ellipse, 2);
            Grid.SetColumnSpan(ellipse, 2);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            Board.Children.Add(ellipse);

        }

        /// <summary>
        /// Create Ellipse UI element of specific size and color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Ellipse createElipse(Color color, int size)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color),
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Black),
            };
            return ellipse;
        }

        private void disableAllPawns()
        {
            foreach (object obj in Board.Children)
            {
                if (obj is Rectangle pawn)
                {
                    pawn.IsHitTestVisible = false;
                }
            }
        }

        private void enablePlayerPawns(string color)
        {
            foreach (object obj in Board.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color))
                {
                    pawn.IsHitTestVisible = true;
                }
            }
        }

        /// <summary>
        /// Disables the automatic playback of GIF animations for a BitmapImage, if the AutoPlay property is available.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data that provides information about the event.</param>
        /// <remarks>
        /// This method checks if the AutoPlay property is present for the BitmapImage class. If present, it attempts to get
        /// the current image source as a BitmapImage and disables its AutoPlay functionality. This is useful for controlling
        /// the playback of GIF animations manually.
        /// </remarks>
        private void gifDice(object sender, RoutedEventArgs e)
        {

            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Media.Imaging.BitmapImage", "AutoPlay"))
            {
                var bitmapImage = imageSource.Source as BitmapImage;
                if (bitmapImage != null) bitmapImage.AutoPlay = false;
            }
        }

        /// <summary>
        /// Stops an animation timer and sets a new GIF image source with AutoPlay disabled.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data that provides information about the tick event.</param>
        /// <remarks>
        /// This method is typically called on a timer tick to stop the timer and update the image source to a new GIF file.
        /// The new GIF image has its AutoPlay property set to false to allow for manual control of the animation playback.
        /// This method assumes that the new GIF image is located in the application's Assets folder.
        /// </remarks>
        private void AnimationTimer_Tick(object sender, object e)
        {

            _animationTimer.Stop();


            var newImageSource = new BitmapImage(new Uri("ms-appx:///Assets/dice-despeed.gif")) { AutoPlay = false };
            imageSource.Source = newImageSource;
        }

        /// <summary>
        /// Handles the tap event on an image to start a GIF animation, play a sound, simulate a dice roll, and display the result.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data that provides information about the tap event.</param>
        /// <remarks>
        /// This method performs the following actions:
        /// - Starts a GIF animation of a dice roll.
        /// - Plays a dice rolling sound.
        /// - Waits briefly to simulate the dice roll.
        /// - Randomly selects a dice result between 1 and 6.
        /// - Displays a static image corresponding to the dice result.
        /// - Shows a message dialog with the dice result.
        /// Note: This method assumes the presence of specific assets in the application's Assets folder.
        /// </remarks>
        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            disableAllPawns();
            // Start the GIF animation
            var gifSource = new BitmapImage(new Uri("ms-appx:///Assets/dice-despeed.gif"));
            imageSource.Source = gifSource;
            ((BitmapImage)imageSource.Source).AutoPlay = true;
            ((BitmapImage)imageSource.Source).Play();

            // Add a sound when dice is rolled
            if (isSoundOn == true)
            {
                await PlaySound("dice");

            }

            // Wait a bit to simulate "spinning"
            await Task.Delay(1000);

            // Randomly generate a dice result and display the static image
            int result = random.Next(1, 7);
            stepCount = result;
            var staticImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/dice-{result}.png"));
            imageSource.Source = staticImageSource;
            enablePlayerPawns(colors[playerturn - 1]);
            if (stepCount == 6)
            {
                //go again
                MarkPlayerSpawns();
            }
            else if (playerturn == Players.Count)
            {
                playerturn = 1;
                MarkPlayerSpawns();
            }
            else
            {
                playerturn++;
                MarkPlayerSpawns();
            }
            imageSource.IsHitTestVisible = false;
            //Test of random and correct image display
            //MessageDialog dialog = new MessageDialog($"Du slog {result}");
            //await dialog.ShowAsync();
        }
        public void MarkPlayerSpawns()
        {

            foreach (object obj in Board.Children)
            {
                if (obj is Ellipse ellipse)
                {
                    ellipse.StrokeThickness = 1;
                }
            }

            foreach (object obj in Board.Children)
            {
                if (obj is Ellipse ellipse)
                {

                    var fill = ellipse.Fill as SolidColorBrush;

                    if (fill != null)
                    {
                        bool isCurrentPlayerColor = false;

                        switch (playerturn)
                        {
                            case 1:
                                isCurrentPlayerColor = fill.Color.Equals(Colors.Yellow);
                                break;
                            case 2:
                                isCurrentPlayerColor = fill.Color.Equals(Colors.Blue);
                                break;
                            case 3:
                                isCurrentPlayerColor = fill.Color.Equals(Colors.Red);
                                break;
                            case 4:
                                isCurrentPlayerColor = fill.Color.Equals(Colors.Green);
                                break;
                        }

                        if (isCurrentPlayerColor)
                        {
                            ellipse.StrokeThickness = 4;
                            ellipse.Stroke = new SolidColorBrush(Colors.Black);
                        }
                    }
                }
            }
        }
        //private static async Task PlaySound(string sound)
        //{
        //    var element = new MediaElement();
        //    var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
        //    var file = await folder.GetFileAsync("dice-sound.mp3");
        //    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
        //    element.SetSource(stream, "");

        //    element.Play();
        //}
        private async Task PlaySound(string sound)
        {
            var element = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file;

            switch (sound)
            {
                case "dice":
                    file = await folder.GetFileAsync("dice-sound.mp3");
                    break;
                case "win":
                    file = await folder.GetFileAsync("winSound.mp3");
                    break;
                case "eat":
                    file = await folder.GetFileAsync("eatPlayerSound.mp3");
                    break;
                case "walk":
                    file = await folder.GetFileAsync("walkSound.mp3");
                    break;

                default:
                    throw new ArgumentException($"Unsupported sound: {sound}");
            }
            if (isSoundOn == true)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                element.SetSource(stream, file.ContentType);

                element.Play();
            }

        }



        /// <summary>
        /// Handles the toggling of the sound icon based on user interaction.
        /// This method toggles the image resource for an Image control between sound-on and sound-off icons,
        /// depending on the current sound state. The sound state is tracked by a boolean variable <c>isSoundOn</c>.
        /// </summary>
        /// <param name="sender">The source of the event, typically an Image control.</param>
        /// <param name="e">Event data that contains information about the event that was triggered.</param>
        /// <remarks>
        /// This method uses isSoundOn to track and toggle the sound state.
        /// The visual state of the sound (on/off) is represented by switching the icon on the Image control.
        /// </remarks>


        private void soundImageSource_Tapped(object sender, TappedRoutedEventArgs e)
        {


            // Sedan, i din händelsehanterare:
            if (!isSoundOn)
            {
                soundImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/soundon.png"));
                isSoundOn = true;
                if (isMusicOn) musicPlayer.Play();
            }
            else
            {
                soundImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/soundoff.png"));
                isSoundOn = false;
                musicPlayer.Pause();
            }
        }
        /// <summary>
        /// Toggles the background music and music icon on and off
        /// </summary>
        /// <param name="sender">The source of the event, typically an Image control.</param>
        /// <param name="e">Event data that contains information about the event that was triggered.</param>
        private void musicImageSource_Tapped(object sender, TappedRoutedEventArgs e)
        {


            // Sedan, i din händelsehanterare:
            if (!isMusicOn)
            {
                musicImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/music-icon.png"));
                isMusicOn = true;
                if (isSoundOn) musicPlayer.Play();
            }
            else
            {
                musicImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/music-off-icon.png"));
                isMusicOn = false;
                musicPlayer.Pause();
            }
        }

        /// <summary>
        /// Changes the visibility of the about view when the user clicks on the questionmark
        /// </summary>
        //private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{

        //    aboutIn.Begin();
        //    aboutView.Visibility = (aboutView.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        //    mainMenu.Visibility = (mainMenu.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        //    imageSource.Visibility = Visibility.Collapsed;


        //}

        private bool isAboutVisible = false; // Lägg till denna medlemsvariabel i din klass
        ///<summary>
        ///Handles the PointerReleased event on the Grid.This method toggles the visibility of the 
        ///aboutView depending on its current state.If the aboutView is visible, it starts the aboutOut animation 
        ///to hide it.If the aboutView is not visible, it makes it visible and starts the aboutIn animation.Additionally, 
        ///it updates the visibility of the mainMenu and imageSource elements.
        ///</summary>
        ///<param name = "sender" > The source of the event.</param>
        ///<param name="e">A PointerRoutedEventArgs that contains the event data.</param>
        ///<remarks>
        ///The method checks if isAboutVisible is true. If so, it starts the aboutOut animation and sets the visibility of aboutView to Collapsed once the animation completes. If isAboutVisible is false, it sets the visibility of aboutView to Visible, starts the aboutIn animation, and updates isAboutVisible to true. It also toggles the visibility of the mainMenu and sets the visibility of imageSource to Collapsed.
        ///</remarks>

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (isAboutVisible)
            {
                // Start aboutOut animation
                aboutOut.Begin();
                aboutOut.Completed += (s, args) =>
                {
                    // Hide aboutView when animation is complete
                    aboutView.Visibility = Visibility.Collapsed;
                };
                isAboutVisible = false;
            }
            else
            {
                // Show aboutView and start aboutIn animation
                aboutView.Visibility = Visibility.Visible;
                aboutIn.Begin();
                isAboutVisible = true;
            }

            // Update visability for mainMenu and imageSource
            mainMenu.Visibility = (mainMenu.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            FadeinMainMenu.Begin();
            imageSource.Visibility = (imageSource.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
