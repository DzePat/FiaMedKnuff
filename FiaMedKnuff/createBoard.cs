using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace FiaMedKnuff
{
    internal class createBoard
    {
        /// <summary>
        /// Generate Path on the board for pawns to move
        /// </summary>
        /// <returns></returns>
        private void generatePath()
        {
            //Yellow Start to left
            MainPage.Instance.boardPath.Add(0, (11, 5));
            MainPage.Instance.boardPath.Add(1, (10, 5));
            MainPage.Instance.boardPath.Add(2, (9, 5));
            MainPage.Instance.boardPath.Add(3, (8, 5));
            MainPage.Instance.boardPath.Add(4, (7, 5));
            // Left to top
            MainPage.Instance.boardPath.Add(5, (7, 4));
            MainPage.Instance.boardPath.Add(6, (7, 3));
            MainPage.Instance.boardPath.Add(7, (7, 2));
            MainPage.Instance.boardPath.Add(8, (7, 1));
            MainPage.Instance.boardPath.Add(9, (6, 1));
            MainPage.Instance.boardPath.Add(10, (5, 1));
            MainPage.Instance.boardPath.Add(11, (5, 2));
            MainPage.Instance.boardPath.Add(12, (5, 3));
            MainPage.Instance.boardPath.Add(13, (5, 4));
            MainPage.Instance.boardPath.Add(14, (5, 5));
            //Top to right
            MainPage.Instance.boardPath.Add(15, (4, 5));
            MainPage.Instance.boardPath.Add(16, (3, 5));
            MainPage.Instance.boardPath.Add(17, (2, 5));
            MainPage.Instance.boardPath.Add(18, (1, 5));
            MainPage.Instance.boardPath.Add(19, (1, 6));
            MainPage.Instance.boardPath.Add(20, (1, 7));
            MainPage.Instance.boardPath.Add(21, (2, 7));
            MainPage.Instance.boardPath.Add(22, (3, 7));
            MainPage.Instance.boardPath.Add(23, (4, 7));
            MainPage.Instance.boardPath.Add(24, (5, 7));
            //Right to bottom
            MainPage.Instance.boardPath.Add(25, (5, 8));
            MainPage.Instance.boardPath.Add(26, (5, 9));
            MainPage.Instance.boardPath.Add(27, (5, 10));
            MainPage.Instance.boardPath.Add(28, (5, 11));
            MainPage.Instance.boardPath.Add(29, (6, 11));
            MainPage.Instance.boardPath.Add(30, (7, 11));
            MainPage.Instance.boardPath.Add(31, (7, 10));
            MainPage.Instance.boardPath.Add(32, (7, 9));
            MainPage.Instance.boardPath.Add(33, (7, 8));
            MainPage.Instance.boardPath.Add(34, (7, 7));
            //Bottom to yellow start
            MainPage.Instance.boardPath.Add(35, (8, 7));
            MainPage.Instance.boardPath.Add(36, (9, 7));
            MainPage.Instance.boardPath.Add(37, (10, 7));
            MainPage.Instance.boardPath.Add(38, (11, 7));
            MainPage.Instance.boardPath.Add(39, (11, 6));
        }

        /// <summary>
        /// generate a placeholder tiles for spawn tiles
        /// </summary>
        private void generateSpawnTiles()
        {
            //yellow spawn tiles
            MainPage.Instance.spawnTiles.Add("Gul-1", (11, 0));
            MainPage.Instance.spawnTiles.Add("Gul-2", (11, 1));
            MainPage.Instance.spawnTiles.Add("Gul-3", (12, 0));
            MainPage.Instance.spawnTiles.Add("Gul-4", (12, 1));
            //Blue spawn tiles
            MainPage.Instance.spawnTiles.Add("Blå-1", (0, 0));
            MainPage.Instance.spawnTiles.Add("Blå-2", (0, 1));
            MainPage.Instance.spawnTiles.Add("Blå-3", (1, 0));
            MainPage.Instance.spawnTiles.Add("Blå-4", (1, 1));
            //Red spawn tiles 
            MainPage.Instance.spawnTiles.Add("Röd-1", (0, 11));
            MainPage.Instance.spawnTiles.Add("Röd-2", (0, 12));
            MainPage.Instance.spawnTiles.Add("Röd-3", (1, 11));
            MainPage.Instance.spawnTiles.Add("Röd-4", (1, 12));
            //Green spawn tiles
            MainPage.Instance.spawnTiles.Add("Grön-1", (11, 11));
            MainPage.Instance.spawnTiles.Add("Grön-2", (11, 12));
            MainPage.Instance.spawnTiles.Add("Grön-3", (12, 11));
            MainPage.Instance.spawnTiles.Add("Grön-4", (12, 12));
        }

        /// <summary>
        /// Generate path for the Goal tiles of corresponding colors
        /// </summary>
        private void generateGoalPath()
        {
            //yellow goal path
            MainPage.Instance.goalTiles.Add("Gul-0", (11, 6));
            MainPage.Instance.goalTiles.Add("Gul-1", (10, 6));
            MainPage.Instance.goalTiles.Add("Gul-2", (9, 6));
            MainPage.Instance.goalTiles.Add("Gul-3", (8, 6));
            MainPage.Instance.goalTiles.Add("Gul-4", (7, 6));
            //blue goal path 
            MainPage.Instance.goalTiles.Add("Blå-0", (6, 1));
            MainPage.Instance.goalTiles.Add("Blå-1", (6, 2));
            MainPage.Instance.goalTiles.Add("Blå-2", (6, 3));
            MainPage.Instance.goalTiles.Add("Blå-3", (6, 4));
            MainPage.Instance.goalTiles.Add("Blå-4", (6, 5));
            //red goal path
            MainPage.Instance.goalTiles.Add("Röd-0", (1, 6));
            MainPage.Instance.goalTiles.Add("Röd-1", (2, 6));
            MainPage.Instance.goalTiles.Add("Röd-2", (3, 6));
            MainPage.Instance.goalTiles.Add("Röd-3", (4, 6));
            MainPage.Instance.goalTiles.Add("Röd-4", (5, 6));
            //green goal path
            MainPage.Instance.goalTiles.Add("Grön-0", (6, 11));
            MainPage.Instance.goalTiles.Add("Grön-1", (6, 10));
            MainPage.Instance.goalTiles.Add("Grön-2", (6, 9));
            MainPage.Instance.goalTiles.Add("Grön-3", (6, 8));
            MainPage.Instance.goalTiles.Add("Grön-4", (6, 7));
        }

        /// <summary>
        /// Sum function of all the generate paths above to call on them with one function
        /// </summary>
        public void generateAllPaths() 
        {
            generatePath();
            generateGoalPath();
            generateSpawnTiles();
        }

        /// <summary>
        /// Creates a board grid
        /// </summary>
        public void generateBoard() 
        {
            MainPage.Instance.BoardInstance.RowDefinitions.Clear();
            MainPage.Instance.BoardInstance.ColumnDefinitions.Clear();

            //add rows and columns to the Board grid
            for (int i = 0; i < 13; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                MainPage.Instance.BoardInstance.RowDefinitions.Add(rowDefinition);
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(50);
                MainPage.Instance.BoardInstance.ColumnDefinitions.Add(columnDefinition);
            }

            //add top tiles
            for (int i = 2; i < 5; i++)
            {
                addellipse(i, 5, Colors.White, "Vit", false);
                addellipse(i, 6, Colors.Red, "Röd", true);
                addellipse(i, 7, Colors.White, "Vit", false);
            }

            //add bottom tiles
            for (int i = 8; i < 11; i++)
            {
                addellipse(i, 5, Colors.White, "Vit", false);
                addellipse(i, 6, Colors.Yellow, "Gul", true);
                addellipse(i, 7, Colors.White, "Vit", false);
            }
            //add left tiles
            for (int i = 2; i < 5; i++)
            {
                addellipse(5, i, Colors.White, "Vit", false);
                addellipse(6, i, Colors.Blue, "Blå", true);
                addellipse(7, i, Colors.White, "Vit", false);
            }
            //add right tiles
            for (int i = 8; i < 11; i++)
            {
                addellipse(5, i, Colors.White, "Vit", false);
                addellipse(6, i, Colors.Green, "Grön", true);
                addellipse(7, i, Colors.White, "Vit", false);
            }
            //add end tiles and start positions
            for (int i = 5; i < 8; i++)
            {
                if (i == 5)
                {
                    addellipse(1, i, Colors.White, "Vit", false);
                    addellipse(11, i, Windows.UI.Color.FromArgb(100, 255, 255, 0), "", false);
                    addellipse(i, 1, Windows.UI.Color.FromArgb(100, 0, 0, 255), "", false);
                    addellipse(i, 11, Colors.White, "Vit", false);
                }
                else if (i == 7)
                {
                    addellipse(1, i, Windows.UI.Color.FromArgb(100, 255, 0, 0), "", false);
                    addellipse(11, i, Colors.White, "Vit", false);
                    addellipse(i, 1, Colors.White, "Vit", false);
                    addellipse(i, 11, Windows.UI.Color.FromArgb(100, 0, 255, 0), "", false);
                }
                else
                {
                    addellipse(1, i, Colors.White, "Vit", false);
                    addellipse(11, i, Colors.White, "Vit", false);
                    addellipse(i, 1, Colors.White, "Vit", false);
                    addellipse(i, 11, Colors.White, "Vit", false);
                }
            }
            //add middle tiles
            addellipse(5, 5, Colors.White, "Vit", false);
            addellipse(5, 7, Colors.White, "Vit", false);
            addellipse(7, 5, Colors.White, "Vit", false);
            addellipse(7, 7, Colors.White, "Vit", false);
            addellipse(6, 6, Colors.Black, "Svart", false);
            addellipse(5, 6, Colors.Red, "Röd", true);
            addellipse(6, 5, Colors.Blue, "Blå", true);
            addellipse(6, 7, Colors.Green, "Grön", true);
            addellipse(7, 6, Colors.Yellow, "Gul", true);
            //add Player Pawn Spawn tiles
            addspawntile(0, 0, Colors.Blue, "Blå", true);
            addspawntile(0, 11, Colors.Red, "Röd", true);
            addspawntile(11, 0, Colors.Yellow, "Gul", true);
            addspawntile(11, 11, Colors.Green, "Grön", true);
        }

        /// <summary>
        /// add ellipse to the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        private void addellipse(int row, int column, Color color, string colorString, bool isGoalTile)
        {
            Ellipse ellipse = createElipse(color, 40);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            if (isGoalTile) MainPage.Instance.listOfAllGoalTileEllipses.Add(ellipse, colorString);
            MainPage.Instance.BoardInstance.Children.Add(ellipse);
        }

        /// <summary>
        /// add player pawn Spawn tiles on the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        private void addspawntile(int row, int column, Color color, string colorString, bool isGoalTile)
        {
            Ellipse ellipse = createElipse(color, 100);
            Grid.SetRowSpan(ellipse, 2);
            Grid.SetColumnSpan(ellipse, 2);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            if (isGoalTile) MainPage.Instance.listOfAllGoalTileEllipses.Add(ellipse, colorString);
            MainPage.Instance.BoardInstance.Children.Add(ellipse);

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
    }
}
