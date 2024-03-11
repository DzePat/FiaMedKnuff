using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace FiaMedKnuff
{
    public class BombHandler
    {
        public static void ChangeBombImage(int numberOfSix, Image bombImage)
        {
            var bombImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/Bombs/bombState-{numberOfSix}.png"));
            bombImage.Source = bombImageSource;
            if (numberOfSix == 3)
            {
                BombExplotion();
                MainPage.Instance.numberOfSixInARow = 0;
            }
        }

        public static async void BombExplotion()
        {
            MainPage.Instance.ExplotionImage.Visibility = Visibility.Visible;
            await Task.Delay(1000);
            foreach (object Object in MainPage.Instance.BoardInstance.Children)
            {
                // reset pawn if the object is a pawn and not at spawn or goal and not the same color as the current player
                if (Object is Rectangle pawn && !MainPage.Instance.PawnHandler.isAtSpawn(pawn) && !MainPage.Instance.PawnHandler.pawnHasReachedGoal(pawn) && !pawn.Name.Contains(MainPage.Instance.colors[MainPage.Instance.playerturn - 1]))
                {
                    MainPage.Instance.PawnHandler.resetPawn(pawn);
                }
            }
            MainPage.Instance.ExplotionImage.Visibility = Visibility.Collapsed;
        }
    }
}
