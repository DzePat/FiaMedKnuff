using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;

namespace FiaMedKnuff
{
    internal class createPlayers
    {
        /// <summary>
        /// Adds player pawns depending on the player amount on selectplayerPage.
        /// </summary>

        public void initializePlayers()
        {
            for (int a = 1; a < SelectPlayersPage.Instance.Players.Count + 1; a++)
            {
                string identity = SelectPlayersPage.Instance.Players[a];
                MainPage.Instance.Players.Add(a, (identity, 0));
            }
            switch (MainPage.Instance.Players.Count)
            {
                case 2:
                    //player 1
                    addPlayerPawns(11, 0, 1, "Gul");
                    //player 2
                    addPlayerPawns(0, 0, 2, "Blå");
                    MainPage.Instance.yellowScore.Visibility = Visibility.Visible;
                    MainPage.Instance.blueScore.Visibility = Visibility.Visible;
                    break;
                case 3:
                    //player 1
                    addPlayerPawns(11, 0, 1, "Gul");
                    //player 2
                    addPlayerPawns(0, 0, 2, "Blå");
                    //player 3
                    addPlayerPawns(0, 11, 3, "Röd");
                    MainPage.Instance.yellowScore.Visibility = Visibility.Visible;
                    MainPage.Instance.blueScore.Visibility = Visibility.Visible;
                    MainPage.Instance.redScore.Visibility = Visibility.Visible;
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
                    MainPage.Instance.yellowScore.Visibility = Visibility.Visible;
                    MainPage.Instance.blueScore.Visibility = Visibility.Visible;
                    MainPage.Instance.redScore.Visibility = Visibility.Visible;
                    MainPage.Instance.greenScore.Visibility = Visibility.Visible;
                    break;
                default:
                    var dialog = new MessageDialog($"player Amount {MainPage.Instance.Players.Count}");
                    dialog.ShowAsync();
                    break;
            }
            MainPage.Instance.MarkPlayerSpawns(1);
        }

        /// <summary>
        /// Adds 4 pawns of the specified color to the spawn points
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
        public void addPawn(int row, int column, string imagePath, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, string NameID)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = 30,
                Height = 40
            };

            rectangle.RenderTransform = new ScaleTransform();
            rectangle.RenderTransformOrigin = new Point(0.5, 0.5);
            rectangle.Margin = new Thickness(0, 8, 0, 0);
            rectangle.PointerPressed += MainPage.Instance.Pawn_Clicked;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath));

            rectangle.Fill = imageBrush;

            rectangle.HorizontalAlignment = horizontalAlignment;
            rectangle.VerticalAlignment = verticalAlignment;
            rectangle.Name = NameID;
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            MainPage.Instance.BoardInstance.Children.Add(rectangle);
        }

    }
}

