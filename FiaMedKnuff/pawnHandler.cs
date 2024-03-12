using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FiaMedKnuff
{
    public class pawnHandler
    {
        public int checkNextGoalTileIndex(string color, int currentrow, int currentcolumn)
        {
            for (int i = 4; i > 0; i--)
            {
                (int row, int column) = MainPage.Instance.goalTiles[color + "-" + i];
                if (tileIsEmpty(row, column))
                {
                    return i;
                }
                if ((row, column) == (currentrow, currentcolumn))
                {
                    return i;
                }
            }
            return 0;
        }

        public int checkNextAvailablePosition(string color, int targetTile, int currentposition)
        {
            if (MainPage.Instance.stepCount >= targetTile - currentposition)
            {
                return targetTile;
            }
            else
            {
                for (int i = currentposition + MainPage.Instance.stepCount; i > currentposition; i--)
                {
                    (int row, int column) = MainPage.Instance.goalTiles[color + "-" + i];
                    if (tileIsEmpty(row, column))
                    {
                        return i;
                    }
                }
                return currentposition;
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
            foreach (object child in MainPage.Instance.BoardInstance.Children)
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
        /// Move a pawn from its current position to a corresponding color spawn position
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public void resetPawn(Rectangle rect)
        {
            int index = 1;
            while (index < 5)
            {
                (int row, int column) = MainPage.Instance.spawnTiles[rect.Name + $"-{index}"];
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
                    MainPage.Instance.PlaySound("eat");
                    break;
                }
                index++;
            }
        }

        /// <summary>
        /// Checks if there is an enemy pawns on the tile your pawn stands
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public async Task checkForEnemyPawns(int row, int column, string color)
        {
            foreach (object child in MainPage.Instance.BoardInstance.Children)
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
        /// sets rectangle position to first tile of the path dictionary
        /// </summary>
        /// <param name="rectangle"></param>
        public async Task linkEndToStartPath(Rectangle rectangle)
        {
            (int row, int column) = MainPage.Instance.boardPath[0];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            MainPage.Instance.stepCount -= 1;
            if (MainPage.Instance.stepCount == 0)
            {
                if (MainPage.Instance.colors[MainPage.Instance.playerturn - 1] != rectangle.Name && MainPage.Instance.isAiTurn(MainPage.Instance.playerturn))
                {
                    MainPage.Instance.AITurn = true;
                }
                await checkForEnemyPawns(row, column, rectangle.Name);
                MainPage.Instance.ImageSource.IsHitTestVisible = true;
                MainPage.Instance.AITurn = true;
            }
        }

        /// <summary>
        /// Places a pawn from the spawn point to the starting position depending on its color
        /// </summary>
        /// <param name="rectangle"></param>
        public async Task placepawnOnTheBoard(Rectangle rectangle)
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
            (int row, int column) = MainPage.Instance.boardPath[startingposition];
            Grid.SetRow(rectangle, row);
            Grid.SetColumn(rectangle, column);
            rectangle.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle.VerticalAlignment = VerticalAlignment.Center;
            MainPage.Instance.stepCount = 0;
            await checkForEnemyPawns(row, column, rectangle.Name);
        }

        public void disableAllPawns()
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn)
                {
                    pawn.IsHitTestVisible = false;
                }
            }
        }

        public void enablePlayerBoardPawns(string color)
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color))
                {
                    //bool result = pawnHasReachedGoal(pawn);
                    if (!pawnHasReachedGoal(pawn) && !isAtSpawn(pawn))
                    {
                        pawn.IsHitTestVisible = true;
                    }
                }
            }
        }

        public bool isAtSpawn(Rectangle pawn)
        {
            if (MainPage.Instance.spawnTiles.ContainsValue((Grid.GetRow(pawn), Grid.GetColumn(pawn))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void enablePlayerSpawnPawns(string color)
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color))
                {
                    //bool result = pawnHasReachedGoal(pawn);
                    if (isAtSpawn(pawn))
                    {
                        pawn.IsHitTestVisible = true;
                    }
                }
            }
        }

        public bool pawnHasReachedGoal(Rectangle pawn)
        {
            if (MainPage.Instance.pawnsOnGoalTiles.ContainsValue((Grid.GetRow(pawn), Grid.GetColumn(pawn))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool hasPawnOnBoard(string color)
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color) && MainPage.Instance.boardPath.Values.Contains((Grid.GetRow(pawn), Grid.GetColumn(pawn))))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasPawnOnSpawn(string color)
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color) && MainPage.Instance.spawnTiles.Values.Contains((Grid.GetRow(pawn), Grid.GetColumn(pawn))))
                {
                    return true;
                }
            }
            return false;
        }             

        public bool hasMovablePawnOnGoalTiles(string color) 
        {
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color) && MainPage.Instance.goalTiles.Values.Contains((Grid.GetRow(pawn), Grid.GetColumn(pawn))) && !pawnHasReachedGoal(pawn))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
