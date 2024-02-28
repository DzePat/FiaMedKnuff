using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
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
        private DispatcherTimer _animationTimer;
        private Random random = new Random();
        private int DiceRoll;
        private bool isSoundOn = true; //sound is on by default

        public MainPage()
        {
            this.InitializeComponent();
            populateBoard();
            generatePath();
            generateGoalPath();
            generateGoalStartTiles();
            InitializeAnimationTimer();
        }
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
            //add Player Pawns
            //player 1
            addPlayerPawns(11, 0, 1, "Gul");
            //player 2
            addPlayerPawns(0, 0, 2, "Blå");
            //player 3
            addPlayerPawns(0, 11, 3, "Röd");
            //player 4
            addPlayerPawns(11, 11, 4, "Grön");

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
        /// Click event handler for pawns on the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Pawn_Clicked(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                int currentRow = Grid.GetRow(rectangle);
                int currentColumn = Grid.GetColumn(rectangle);
                int foundKey;
                if (boardPath.ContainsValue((currentRow, currentColumn)) | goalPath.ContainsValue((currentRow, currentColumn)))
                {

                    while (DiceRoll != null & DiceRoll != 0)
                    {
                        currentRow = Grid.GetRow(rectangle);
                        currentColumn = Grid.GetColumn(rectangle);
                        foundKey = boardPath.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key;
                        if (goalStartTile[rectangle.Name + "-1"] == (currentRow, currentColumn))
                        {
                            (int row, int column) = goalPath[rectangle.Name + "-2"];
                            Grid.SetRow(rectangle, row);
                            Grid.SetColumn(rectangle, column);
                            DiceRoll -= 1;
                        }
                        else if (goalPath.ContainsValue((currentRow, currentColumn)))
                        {
                            moveOneGoalTile(rectangle);
                        }
                        else if (boardPath.ContainsKey((int)foundKey + 1))
                        {
                            (int row, int column) = boardPath[foundKey + 1];
                            Grid.SetRow(rectangle, row);
                            Grid.SetColumn(rectangle, column);
                            foundKey += 1;
                            DiceRoll -= 1;
                        }
                        else
                        {
                            linkEndToStartPath(rectangle);
                        }
                    }
                }
                else if (DiceRoll == 6 || DiceRoll == 1 && !goalPath.ContainsValue((currentRow, currentColumn)))
                {
                    placepawnOnTheBoard(rectangle);
                }

            }
        }

        /// <summary>
        /// sets rectangle position to first tile of the path dictionary
        /// </summary>
        /// <param name="rectangle"></param>
        private void linkEndToStartPath(Rectangle rectangle)
        {
            (int row, int column) = boardPath[0];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            DiceRoll -= 1;
        }

        /// <summary>
        /// Places a pawn from the spawn point to the starting position depending on its color
        /// </summary>
        /// <param name="rectangle"></param>
        private void placepawnOnTheBoard(Rectangle rectangle)
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
            DiceRoll = 0;
        }

        /// <summary>
        /// Moves a single tile in goalpath if all tiles infront is occupied then it occupies the current tile
        /// </summary>
        /// <param name="rectangle"></param>
        private async void moveOneGoalTile(Rectangle rectangle)
        {
            int currentRow = Grid.GetRow(rectangle);
            int currentColumn = Grid.GetColumn(rectangle);
            string currentKey = goalPath.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key;
            string[] currentKeySplit = currentKey.Split('-'); //returns "[color,index]"
            string newkey = $"{currentKeySplit[0]}-{int.Parse(currentKeySplit[1]) + 1}";
            if (goalPath.ContainsKey(newkey) & goalReached.ContainsKey(newkey) == false)
            {
                (int newRow, int newColumn) = goalPath[newkey];
                Grid.SetRow(rectangle, newRow);
                Grid.SetColumn(rectangle, newColumn);
                DiceRoll -= 1;
                //check if the current position is last in the goal tiles
                if (goalReached.ContainsKey($"{currentKeySplit[0]}-{int.Parse(currentKeySplit[1]) + 2}"))
                {
                    goalReached.Add(newkey, goalPath[newkey]);
                    rectangle.PointerPressed -= Pawn_Clicked;
                    DiceRoll = 0;
                }
            }
            else
            {
                goalReached.Add(currentKey, goalPath[currentKey]);
                rectangle.PointerPressed -= Pawn_Clicked;
                DiceRoll = 0;
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
            // Start the GIF animation
            var gifSource = new BitmapImage(new Uri("ms-appx:///Assets/dice-despeed.gif"));
            imageSource.Source = gifSource;
            ((BitmapImage)imageSource.Source).AutoPlay = true;
            ((BitmapImage)imageSource.Source).Play();

            // Add a sound when dice is rolled
            if (isSoundOn == true)
            {
                var element = new MediaElement();
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var file = await folder.GetFileAsync("dice-sound.mp3");
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                element.SetSource(stream, "");

                element.Play();
                //MessageDialog dialog = new MessageDialog("Ljudet är på");
            }

            // Wait a bit to simulate "spinning"
            await Task.Delay(1000);

            // Randomly generate a dice result and display the static image
            int result = random.Next(1, 7);
            DiceRoll = result;
            var staticImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/dice-{result}.png"));
            imageSource.Source = staticImageSource;

            //Test of random and correct image display
            //MessageDialog dialog = new MessageDialog($"Du slog {result}");
            //await dialog.ShowAsync();
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
            }
            else
            {
                soundImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/soundoff.png"));
                isSoundOn = false;
            }

        }
    }
}
