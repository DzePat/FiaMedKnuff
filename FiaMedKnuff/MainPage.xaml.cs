using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private DispatcherTimer _animationTimer;

        public MainPage()
        {
            this.InitializeComponent();
            populateBoard();
            generatePath();
            InitializeAnimationTimer();
        }
        private void InitializeAnimationTimer()
        {
            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromSeconds(1);
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        //add tiles and colorings to the board
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

        //Create a path on the board
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

        //add all player pawns
        private void addPlayerPawns(int row, int column, int playerID, string nameID)
        {
            string[] pawnPaths = new string[] {
                "/Assets/Gul.png",
                "/Assets/Blå.png",
                "/Assets/Röd.png",
                "/Assets/Grön.png",
            };

            string workingdirectory = Directory.GetCurrentDirectory();
            addPawn(row, column, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Right, VerticalAlignment.Bottom, nameID + 1);
            addPawn(row, column + 1, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Left, VerticalAlignment.Bottom, nameID + 2);
            addPawn(row + 1, column, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Right, VerticalAlignment.Top, nameID + 3);
            addPawn(row + 1, column + 1, workingdirectory + pawnPaths[playerID - 1], HorizontalAlignment.Left, VerticalAlignment.Top, nameID + 4);
        }

        //add Pawn to the Board
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

        private void Pawn_Clicked(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                int currentRow = Grid.GetRow(rectangle);
                int currentColumn = Grid.GetColumn(rectangle);
                int foundKey;

                if (boardPath.ContainsValue((currentRow, currentColumn)))
                {
                    foundKey = boardPath.FirstOrDefault(x => x.Value == (currentRow, currentColumn)).Key;
                    if (boardPath.ContainsKey((int)foundKey + 1))
                    {
                        (int row, int column) = boardPath[foundKey + 1];
                        Grid.SetRow(rectangle, row);
                        Grid.SetColumn(rectangle, column);
                    }
                }
                else
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
                }

            }
        }


        //add ellipse to the board
        private void addellipse(int row, int column, Color color)
        {
            Ellipse ellipse = createElipse(color, 40);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            Board.Children.Add(ellipse);
        }

        //add player pawn Spawns
        private void addspawntile(int row, int column, Color color)
        {
            Ellipse ellipse = createElipse(color, 100);
            Grid.SetRowSpan(ellipse, 2);
            Grid.SetColumnSpan(ellipse, 2);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            Board.Children.Add(ellipse);

        }

        //create an ellipse of specific color
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
        private void gifDice(object sender, RoutedEventArgs e)
        {

            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Media.Imaging.BitmapImage", "AutoPlay"))
            {
                var bitmapImage = imageSource.Source as BitmapImage;
                if (bitmapImage != null) bitmapImage.AutoPlay = false;
            }
        }
        private void AnimationTimer_Tick(object sender, object e)
        {
            //TODO: När randomfunktionen har genererat en siffra så ska den siffran visas över denna gif animationen. 
            _animationTimer.Stop();


            var newImageSource = new BitmapImage(new Uri("ms-appx:///Assets/dice-fast.gif")) { AutoPlay = false };
            imageSource.Source = newImageSource;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/dice-fast.gif")) { AutoPlay = true };
            imageSource.Source = bitmapImage;

            _animationTimer.Start();
        }

    }
}
