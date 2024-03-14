﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
        public Dictionary<string, (int, int)> goalTiles = new Dictionary<string, (int, int)>();
        public Dictionary<string, (int, int)> pawnsOnGoalTiles = new Dictionary<string, (int, int)>();
        public Dictionary<string, (int, int)> spawnTiles = new Dictionary<string, (int, int)>();
        public Dictionary<int, (string, int)> Players = new Dictionary<int, (string, int)>();
        /// <summary>
        /// A list of all the ellipses on the board with a string of the color and boolean if it is a goaltile or not
        /// </summary>
        public Dictionary<Ellipse, string> listOfAllGoalTileEllipses = new Dictionary<Ellipse, string>();
        public List<int> Winners = new List<int>();
        public string[] colors = { "Gul", "Blå", "Röd", "Grön" };
        private DispatcherTimer _animationTimer;
        private Random random = new Random();
        public int stepCount;
        public int currentDiceResult;
        public bool isSoundOn = true; //sound is on by default
        private bool isMusicOn = true;
        private MediaElement musicPlayer = new MediaElement();
        public int playerturn = 1;
        public bool AITurn;
        public int numberOfSixInARow = 0;
        private double minWinSizeWidth = 1400; //minimum width for the window
        private double minWinSizeHeight = 800; //minimum height for the window

        /// <summary>
        /// for access to objects from another files
        /// </summary>
        public static MainPage Instance { get; private set; }
        public Image ImageSource { get { return imageSource; } }
        public Image BombImage { get { return bombImage; } }
        public Image ExplosionImage { get { return explosionImage; } }
        public Grid BoardInstance { get { return Board; } }
        public StackPanel ScoreBoard { get { return scoreBoard; } }
        public Grid VictoryScreen { get { return victoryView; } }
        public Grid yellowScore { get { return yellowPlayerScore; } }
        public Grid blueScore { get { return bluePlayerScore; } }
        public Grid redScore { get { return redPlayerScore; } }
        public Grid greenScore { get { return greenPlayerScore; } }
        public Grid BlurGrid { get { return blurGrid; } }
        public Grid BackButton { get { return backButton; } }
        //public Storyboard BlurdGridFadeIn { get { return blurGridFadeIn; } }


        public pawnHandler PawnHandler = new pawnHandler();
        private createBoard createBoard = new createBoard();
        private AIHandler AI_Handler = new AIHandler();

        public MainPage()
        {
            this.InitializeComponent();
            Instance = this;
            createBoard.generateAllPaths();
            createBoard.generateBoard();
            InitializeAnimationTimer();
            initMusicPlayer();
            Window.Current.SizeChanged += Current_SizeChanged; //force the window size to be big enough
            Window.Current.Activated += Current_Activated; //force the window size to be big enough
        }

        /// <summary>
        /// Initializes the musicplayer to play the background music on permanent loop
        /// </summary>
        private async void initMusicPlayer()
        {
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("backgroundMusic.mp3");
            musicPlayer.IsLooping = true;

            musicPlayer.Volume = 0.1;
            //currently not playing music on start to save developer sanity
            musicPlayer.AutoPlay = false;
            TurnOffMusic();

            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            musicPlayer.SetSource(stream, file.ContentType);
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
        /// Click event handler for pawns on the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Pawn_Clicked(object sender, PointerRoutedEventArgs e)
        {
            ClearPreviousPlayerChoiceIndications();
            if (sender is Rectangle pawn)
            {
                await pawn_Event(pawn);
            }
        }

        /// <summary>
        /// Pawn movment event for both AI and player
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        private async Task pawn_Event(Rectangle pawn)
        {
            disableDiceClick();
            pawn.IsHitTestVisible = false;
            int currentRow = Grid.GetRow(pawn);
            int currentColumn = Grid.GetColumn(pawn);
            int foundKey;
            // if the position of the pawn exists in the gameboard
            if (boardPath.ContainsValue((currentRow, currentColumn)) | goalTiles.ContainsValue((currentRow, currentColumn)))
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
                    await PlaySound("walk");
                    await Task.Delay(300);
                    AnimatePawnLift(pawn);
                    if (goalTiles.ContainsValue((currentRow, currentColumn)) && goalTiles.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key.Contains(pawn.Name))
                    {
                        int targetTile = PawnHandler.checkNextGoalTileIndex(pawn.Name, currentRow, currentColumn);
                        int currentTile = int.Parse((goalTiles.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key).Split('-')[1]);
                        int next = PawnHandler.checkNextAvailablePosition(pawn.Name, targetTile, currentTile);
                        if (next == currentTile)
                        {
                            stepCount = 0;
                            imageSource.IsHitTestVisible = true;
                        }
                        else
                        {
                            stepCount--;
                            imageSource.IsHitTestVisible = true;
                            (int row, int column) = goalTiles[pawn.Name + "-" + (currentTile + 1)];
                            Grid.SetRow(pawn, row);
                            Grid.SetColumn(pawn, column);
                            currentTile += 1;
                        }
                        if (currentTile == targetTile)
                        {
                            stepCount = 0;
                            pawn.IsHitTestVisible = false;
                            imageSource.IsHitTestVisible = true;
                            pawnsOnGoalTiles.Add(pawn.Name + "-" + targetTile, (Grid.GetRow(pawn), Grid.GetColumn(pawn)));
                            if (playerHasWon(pawn.Name) == true)
                            {
                                Winners.Add(Array.IndexOf(colors, pawn.Name) + 1);
                            }
                            MarkPlayerSpawns(playerturn);
                            if (Winners.Count == Players.Count - 1)
                            {
                                int index = 0;
                                string result = "";
                                foreach (int player in Winners)
                                {
                                    (string identity, int score) = Players[playerturn - 1];
                                    result += $"Player {player} won with {score}\n";
                                    showVictoryView(identity, score);
                                }
                                var dialog = new MessageDialog(result);
                                await dialog.ShowAsync();
                            }
                            if (colors[playerturn - 1] != pawn.Name && isAiTurn(playerturn))
                            {
                                AITurn = true;
                            }
                        }
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
                            await PawnHandler.checkForEnemyPawns(row, column, pawn.Name);
                            MarkPlayerSpawns(playerturn);
                            changePawnMargin(pawn);
                            if (colors[playerturn - 1] != pawn.Name && isAiTurn(playerturn))
                            {
                                AITurn = true;
                            }
                            else
                            {
                                imageSource.IsHitTestVisible = true;
                            }
                        }
                    }
                    else
                    {
                        await PawnHandler.linkEndToStartPath(pawn);
                    }
                }
            }
            // place the pawn on the board if the clicked pawn is in the nest
            else if (stepCount == 6 || stepCount == 1 && !goalTiles.ContainsValue((currentRow, currentColumn)))
            {
                if (colors[playerturn - 1] != pawn.Name && isAiTurn(playerturn) && stepCount == 1)
                {
                    AITurn = true;
                }
                else
                {
                    imageSource.IsHitTestVisible = true;
                }
                await PawnHandler.placepawnOnTheBoard(pawn);
                MarkPlayerSpawns(playerturn);
                changePawnMargin(pawn);
            }
            await RunAi();
        }

        private void changePawnMargin(Rectangle pawn)
        {
            int row = Grid.GetRow(pawn);
            int column = Grid.GetColumn(pawn);
            string color = pawn.Name;
            int numberOfPawns = PawnHandler.CountOwnPawnsOnTile(row, column, color);
            PawnHandler.AdjustPawnPositions(row, column, color);
        }

        /// <summary>
        /// Explosion animation for when player rolls 6 three times in a row
        /// </summary>
        public async void bigboom()
        {
            bigExplosion.Begin();
            blurGrid.Visibility = Visibility.Visible;
            BlurdGridFadeIn.Begin();
            await Task.Delay(800);

            blurGrid.Visibility = Visibility.Collapsed;


        }

        /// <summary>
        /// checks if bitmapimage has propery "autoplay" and if it does disables it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        private async void Dice_Clicked(object sender, TappedRoutedEventArgs e)
        {
            await Dice_Event();
            await turnHandler();
            await RunAi();
        }

        /// <summary>
        /// Executes diceroll and turnhandler for AI
        /// </summary>
        /// <returns></returns>
        private async Task RunAi()
        {
            while (AITurn == true)
            {
                await Task.Delay(500);
                await Dice_Event();
                await turnHandler();
                MarkPlayerSpawns(playerturn);
                ClearPreviousPlayerChoiceIndications();
            }
        }

        /// <summary>
        /// rolls the dice if its Ai turn then it even moves the pawn
        /// </summary>
        /// <returns></returns>
        private async Task Dice_Event()
        {
            AITurn = false;
            disableDiceClick();
            imageSource.IsHitTestVisible = false;
            PawnHandler.disableAllPawns();
            ClearPreviousPlayerChoiceIndications();
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
            currentDiceResult = result;

            var staticImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/dice-{result}.png"));
            imageSource.Source = staticImageSource;
            await enablePlayerTurnAsync();
            CountScore();
            (string identity, int score) = Players[playerturn];
            if (identity == "AI")
            {
                string AIColor = colors[playerturn - 1];
                Rectangle pawnAI = AI_Handler.pawnToMove(AIColor);
                if (pawnAI != null)
                {
                    await pawn_Event(pawnAI);
                }
            }
        }

        /// <summary>
        /// enables player pawns during players turn
        /// </summary>
        /// <returns></returns>
        private async Task enablePlayerTurnAsync()
        {
            if ((currentDiceResult == 1 | currentDiceResult == 6) && PawnHandler.hasPawnOnSpawn(colors[playerturn - 1]) == true)
            {
                imageSource.IsHitTestVisible = false;
                MarkCurrentPlayerTurnChoice(colors[playerturn - 1]);
                PawnHandler.enablePlayerSpawnPawns(colors[playerturn - 1]);
                PawnHandler.enablePlayerBoardPawns(colors[playerturn - 1]);
                MarkPlayerSpawns(playerturn);
            }
            else if (PawnHandler.hasPawnOnBoard(colors[playerturn - 1]) == true | PawnHandler.hasMovablePawnOnGoalTiles(colors[playerturn - 1]) == true)
            {
                imageSource.IsHitTestVisible = false;
                PawnHandler.enablePlayerBoardPawns(colors[playerturn - 1]);
                MarkPlayerSpawns(playerturn);
            }
            else
            {
                (string identity, int score) = Players[CheckNextplayerturn(playerturn)];
                if (identity == "AI")
                {
                    AITurn = true;
                    MarkPlayerSpawns(playerturn);
                }
                else
                {
                    imageSource.IsHitTestVisible = true;
                }
            }

            if (!PawnHandler.hasPawnOnBoard(colors[playerturn - 1]) && (stepCount != 1 && stepCount != 6))
            {
                if (playerturn == Players.Count)
                {
                    MarkPlayerSpawns(1);
                }
                else
                {
                    MarkPlayerSpawns(playerturn + 1);
                }

            }
        }

        /// <summary>
        /// turns of the dice click function if the current turn is Ai
        /// </summary>
        private void disableDiceClick()
        {
            (string identity, int score) = Players[CheckNextplayerturn(playerturn)];
            if (identity == "AI")
            {
                imageSource.IsHitTestVisible = false;
            }
        }

        /// <summary>
        /// handles player turns depending on current roll
        /// </summary>
        /// <returns></returns>
        public async Task turnHandler()
        {
            if (currentDiceResult == 6)
            {
                if (isAiTurn(playerturn) && stepCount == 0)
                {
                    AITurn = true;
                }
                //player goes again
                numberOfSixInARow++;
                BombHandler.ChangeBombImage(numberOfSixInARow, bombImage);
            }
            else if (currentDiceResult < 6 && currentDiceResult > 0)
            {
                playerturn = nextplayerturn(playerturn);
                if (isAiTurn(playerturn) && stepCount == 0)
                {
                    AITurn = true;
                }
            }
            while (playerHasWon(colors[playerturn - 1]) && Winners.Count != (Players.Count - 1))
            {
                playerturn = nextplayerturn(playerturn);
                if (isAiTurn(playerturn) && stepCount == 0)
                {
                    AITurn = true;
                }
            }
        }

        /// <summary>
        /// takes turn as arugment and returns true if the current turn is AIs turn
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public bool isAiTurn(int turn)
        {
            (string identity, int score) = Players[turn];
            if (identity == "AI")
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Returns next playerturn number additionaly resets bomb counter
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        private int nextplayerturn(int playerID)
        {
            int newid = playerID + 1;
            if (newid > Players.Count)
            {
                newid = 1;
            }
            numberOfSixInARow = 0;
            BombHandler.ChangeBombImage(numberOfSixInARow, bombImage);
            return newid;
        }

        /// <summary>
        /// returns next playerturn number
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        private int CheckNextplayerturn(int playerID)
        {
            int newid = playerID + 1;
            if (newid > Players.Count)
            {
                newid = 1;
            }
            return newid;
        }

        /// <summary>
        /// adds +1 depending on players turn
        /// </summary>
        private void CountScore()
        {
            switch (playerturn)
            {
                case 1:
                    (string identity, int score) = Players[1];
                    int newScore = score + 1;
                    Players[1] = (identity, newScore);
                    OneScore.Text = newScore.ToString();
                    break;
                case 2:
                    (string identity2, int score2) = Players[2];
                    int newScore2 = score2 + 1;
                    Players[2] = (identity2, newScore2);
                    TwoScore.Text = newScore2.ToString();
                    break;
                case 3:
                    (string identity3, int score3) = Players[3];
                    int newScore3 = score3 + 1;
                    Players[3] = (identity3, newScore3);
                    ThreeScore.Text = newScore3.ToString();
                    break;
                case 4:
                    (string identity4, int score4) = Players[4];
                    int newScore4 = score4 + 1;
                    Players[4] = (identity4, newScore4);
                    FourScore.Text = newScore4.ToString();
                    break;
            }
            if (Winners.Count == 3)
            {
                //Player has won
            }
        }

        /// <summary>
        /// checks if the player has all of its pawns on goal tiles if it does returns true else false
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool playerHasWon(string color)
        {
            int pawnsOnGoalTile = 0;
            foreach (string key in pawnsOnGoalTiles.Keys)
            {
                if (key.Contains(color))
                {
                    pawnsOnGoalTile += 1;
                }
            }
            if (pawnsOnGoalTile == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// handles animation for pawn movment
        /// </summary>
        /// <param name="pawn"></param>
        public void AnimatePawnLift(Rectangle pawn)
        {
            var storyboard = new Storyboard();

            var scaleXAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 1.3, // 130%
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = true // Automatically reverse the animation (shrink back)
            };
            Storyboard.SetTarget(scaleXAnimation, pawn);
            Storyboard.SetTargetProperty(scaleXAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");

            var scaleYAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 1.3,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                AutoReverse = true
            };
            Storyboard.SetTarget(scaleYAnimation, pawn);
            Storyboard.SetTargetProperty(scaleYAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");

            storyboard.Children.Add(scaleXAnimation);
            storyboard.Children.Add(scaleYAnimation);

            storyboard.Begin();
        }

        /// <summary>
        /// marks player spawn points
        /// </summary>
        /// <param name="colorIndex"></param>
        public void MarkPlayerSpawns(int colorIndex)
        {
            foreach (Ellipse ellipse in listOfAllGoalTileEllipses.Keys)
            {
                ellipse.StrokeThickness = 1;
                ellipse.Opacity = 0.5;
            }

            foreach (Ellipse ellipse in listOfAllGoalTileEllipses.Keys)
            {
                if (listOfAllGoalTileEllipses[ellipse] == colors[colorIndex - 1])
                {
                    ellipse.StrokeThickness = 4;
                    ellipse.Stroke = new SolidColorBrush(Colors.Black);
                    StartPulsingAnimation(ellipse);
                    ellipse.Opacity = 1;
                    if ((stepCount == 1 || stepCount == 6) && PawnHandler.hasPawnOnSpawn(colors[colorIndex - 1]) && PawnHandler.hasPawnOnBoard(colors[colorIndex - 1]))
                    {
                        MarkCurrentPlayerTurnChoice(colors[colorIndex - 1]);
                    }
                }
            }

        }

        /// <summary>
        /// marks player pawns
        /// </summary>
        /// <param name="currentPlayer"></param>
        private void MarkCurrentPlayerTurnChoice(string currentPlayer)
        {
            // Ta bort tidigare markeringar
            ClearPreviousPlayerChoiceIndications();

            // Loopa genom alla barn till spelbrädet
            foreach (var child in Board.Children)
            {
                if (child is Rectangle pawn && pawn.Name.Contains(currentPlayer))
                {
                    // FIX: Only target the pawns that has not reached the goalTiles
                    pawn.Stroke = new SolidColorBrush(Colors.Gold);
                    pawn.StrokeThickness = 2;
                    AnimatePawnLift(pawn);
                }
            }

        }


        /// <summary>
        /// clears all strokes from the pawns
        /// </summary>
        private void ClearPreviousPlayerChoiceIndications()
        {
            foreach (var child in Board.Children)
            {
                if (child is Rectangle pawn)
                {
                    // Återställ visuella effekter för pionen
                    pawn.Stroke = new SolidColorBrush(Colors.Transparent);
                    pawn.StrokeThickness = 0;
                }
            }
        }

        /// <summary>
        /// takes string as argument and plays the sound from the assets
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task PlaySound(string sound)
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
                turnOnMusic();
            }
            else
            {
                TurnOffMusic();
            }
        }
        /// <summary>
        /// Starts playing music and updates the music icon accordingly
        /// </summary>
        private void TurnOffMusic()
        {
            musicImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/music-off-icon.png"));
            isMusicOn = false;
            musicPlayer.Pause();
        }

        /// <summary>
        /// stops playing music and updates the music icon accordingly
        /// </summary>
        private void turnOnMusic()
        {
            musicImageSource.Source = new BitmapImage(new Uri("ms-appx:///Assets/music-icon.png"));
            isMusicOn = true;
            if (isSoundOn) musicPlayer.Play();
        }

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
            //BUG: The Dice shows on about when game is on. when hide aboutview, the dice is not visible
            if (isAboutVisible)
            {
                // Start aboutOut animation
                aboutOut.Begin();
                aboutOut.Completed += (s, args) =>
                {
                    // Hide aboutView when animation is complete
                    aboutView.Visibility = Visibility.Collapsed;
                    BlurdGridFadeOut.Begin();
                    blurGrid.Visibility = Visibility.Collapsed;
                    StartHighScoreAnimation();
                    imageSource.Visibility = (MainMenu.Instance.MainMenuContent.Visibility == Visibility.Visible || MainMenu.Instance.HighScoreMenu.Visibility == Visibility.Visible || MainMenu.Instance.SelectPlayerMenu.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                };
                isAboutVisible = false;
            }
            else
            {
                // Show aboutView and start aboutIn animation
                aboutView.Visibility = Visibility.Visible;
                aboutIn.Begin();
                isAboutVisible = true;
                blurGrid.Visibility = Visibility.Visible;
                BlurdGridFadeIn.Begin();
                imageSource.Visibility = Visibility.Collapsed;
            }

            // Update visability for mainMenu and imageSource
            mainMenu.Visibility = (mainMenu.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            FadeinMainMenu.Begin();
            //imageSource.Visibility = (imageSource.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// hides backButton and shows main menu view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            backButton.Visibility = Visibility.Collapsed;
            MainMenu.Instance.CloseButton.Visibility = Visibility.Visible;
            MainMenu.Instance.ShowMainMenu();
        }

        /// <summary>
        /// Plays highscore animation
        /// </summary>
        public void StartHighScoreAnimation()
        {
            highScoreIn.Begin();
        }

        /// <summary>
        /// Method to create and start the pulsing animation for an ellipse's stroke thickness
        /// </summary>
        /// <param name="targetElement"></param>
        private void StartPulsingAnimation(UIElement targetElement)
        {

            // Create the storyboard
            var storyboard = new Storyboard();

            // Create scale transform and apply to the target element if not already applied
            if (targetElement.RenderTransform as ScaleTransform == null)
            {
                targetElement.RenderTransform = new ScaleTransform();
                targetElement.RenderTransformOrigin = new Point(0.5, 0.5); // Center the scaling
            }

            var pulsingScaleXAnimation = new DoubleAnimation
            {
                From = 1.0, // Start scale
                To = 1.2, // End scale (20% larger)
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = true
            };

            var pulsingScaleYAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = true

            };

            Storyboard.SetTarget(pulsingScaleXAnimation, targetElement);
            Storyboard.SetTargetProperty(pulsingScaleXAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");

            Storyboard.SetTarget(pulsingScaleYAnimation, targetElement);
            Storyboard.SetTargetProperty(pulsingScaleYAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");

            storyboard.Children.Add(pulsingScaleXAnimation);
            storyboard.Children.Add(pulsingScaleYAnimation);

            storyboard.Begin();
        }

        /// <summary>
        /// ***REPLACE THIS METHOD FOR RELEASE***
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DEBUG_Win_Button_Click(object sender, RoutedEventArgs e)
        {
            showVictoryView(colors[random.Next(4)], random.Next(75));
        }

        /// <summary>
        /// initializes the victoryr page and makes it visible.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="moves"></param>
        public void showVictoryView(string color, int moves)
        {
            VictoryPage.instance.loadPage(color, moves);
            victoryView.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Resets the whole game state to the initial values.
        /// Rebuilds the entire board structure.
        /// Needs to be called before starting a new game.
        /// </summary>
        public void ResetGame()
        {
            Players.Clear();
            pawnsOnGoalTiles.Clear();
            Winners.Clear();
            stepCount= 0;
            playerturn = 1;
            numberOfSixInARow= 0;
            OneScore.Text = "0";
            TwoScore.Text = "0";
            ThreeScore.Text = "0";
            FourScore.Text = "0";
            imageSource.IsHitTestVisible = true;

            BoardInstance.Children.Clear();
            boardPath.Clear();
            goalTiles.Clear();
            spawnTiles.Clear();
            createBoard.generateAllPaths();
            createBoard.generateBoard();
        }

        /// <summary>
        /// Handles the SizeChanged event of the window and enforces the minimum window size.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {

            // Ensure the window doesn't go below the minimum size
            if (Window.Current.Bounds.Width < minWinSizeWidth)
            {
                ApplicationView.GetForCurrentView().TryResizeView(new Windows.Foundation.Size(minWinSizeWidth, Window.Current.Bounds.Height));
            }
            if (Window.Current.Bounds.Height < minWinSizeHeight)
            {
                ApplicationView.GetForCurrentView().TryResizeView(new Windows.Foundation.Size(Window.Current.Bounds.Width, minWinSizeHeight));
            }
        }

        /// <summary>
        /// Handles the Activated event of the window and restores the window size to minimum if it becomes active and its size is less than the minimum.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            // Restore the window size to minimum if it becomes active and its size is less than minimum
            if (Window.Current.Bounds.Width < minWinSizeWidth || Window.Current.Bounds.Height < minWinSizeHeight)
            {
                ApplicationView.GetForCurrentView().TryResizeView(new Windows.Foundation.Size(minWinSizeWidth, minWinSizeHeight));
            }
        }
    }
}
