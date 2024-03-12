using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace FiaMedKnuff
{
    internal class AIHandler
    {
        private pawnHandler  PawnHandler = new pawnHandler();
        private void isEnemyPawnAvailable(int AI_ID) 
        {
            int steps = MainPage.Instance.currentDiceResult;
            if (steps == 1 | steps == 6) 
            { 
                
            }
        }

        public Rectangle pawnToMove(string AIColor) 
        {
            if (PawnHandler.hasPawnOnBoard(AIColor) == true) 
            {
                foreach (object obj in MainPage.Instance.BoardInstance.Children)
                {
                    if (obj is Rectangle pawn && pawn.Name == AIColor && MainPage.Instance.boardPath.ContainsValue((Grid.GetRow(pawn),Grid.GetColumn(pawn))))
                    {
                        return pawn;
                    }
                }
            }
            else if (PawnHandler.hasMovablePawnOnGoalTiles(AIColor)) 
            {
                foreach (object obj in MainPage.Instance.BoardInstance.Children)
                {
                    if (obj is Rectangle pawn && pawn.Name == AIColor && MainPage.Instance.goalTiles.ContainsValue((Grid.GetRow(pawn), Grid.GetColumn(pawn))) && !PawnHandler.pawnHasReachedGoal(pawn))
                    {
                        return pawn;
                    }
                }
            }
            else if((MainPage.Instance.currentDiceResult == 1 | MainPage.Instance.currentDiceResult == 6) && PawnHandler.hasPawnOnSpawn(AIColor)) 
            {
                foreach (object obj in MainPage.Instance.BoardInstance.Children)
                {
                    if (obj is Rectangle pawn && pawn.Name == AIColor && MainPage.Instance.spawnTiles.ContainsValue((Grid.GetRow(pawn), Grid.GetColumn(pawn))))
                    {
                        return pawn;
                    }
                }
            }
            return null; 
        }
    }
}
