﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace FiaMedKnuff
{
    public class pawnHandler
    {
        /// <summary>
        /// returns last tile from the goaltiles that is not occupied
        /// </summary>
        /// <param name="color"></param>
        /// <param name="currentrow"></param>
        /// <param name="currentcolumn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns index of next available goal position for the pawn
        /// </summary>
        /// <param name="color"></param>
        /// <param name="targetTile"></param>
        /// <param name="currentposition"></param>
        /// <returns></returns>
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

        /// <summary>
        /// disables click events for all the pawns
        /// </summary>
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

        /// <summary>
        /// Enables player pawns on the boartpath
        /// </summary>
        /// <param name="color"></param>
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

        /// <summary>
        /// returns true if given pawn is on spawn tile
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Enables click event for player pawns on spawn tiles
        /// </summary>
        /// <param name="color"></param>
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

        /// <summary>
        /// returns true if given pawn has reached last available goal position
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns true if player has pawn on the boardpath
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns true if player has a pawn on the spawn position
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Returns true if the player of given color has a pawn at goal tiles but its not on the last position
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// returns the number of pawns on the tile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public int CountOwnPawnsOnTile(int row, int column, string color)
        {
            int count = 0;
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color) && Grid.GetRow(pawn) == row && Grid.GetColumn(pawn) == column)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// adjusts pawn positions if there is multiple on single tile
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        public void AdjustPawnPositions(int row, int column, string color)
        {
            var pawnsOnTile = new List<Rectangle>();
            foreach (object obj in MainPage.Instance.BoardInstance.Children)
            {
                if (obj is Rectangle pawn && pawn.Name.Contains(color) && Grid.GetRow(pawn) == row && Grid.GetColumn(pawn) == column)
                {
                    pawnsOnTile.Add(pawn);
                }
            }

            int count = pawnsOnTile.Count;
            int offset = 10;
            for (int i = 0; i < count; i++)
            {
                int startOffset = i * offset;
                pawnsOnTile[i].Margin = new Thickness(startOffset, 0, 0, 0);
            }
        }

        /// <summary>
        /// Resets margin for pawn
        /// </summary>
        /// <param name="pawn"></param>
        public void ResetPawnMargin(Rectangle pawn)
        {
            pawn.Margin = new Thickness(0, 0, 0, 0);
        }

    }
}
