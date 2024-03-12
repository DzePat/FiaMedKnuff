using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace FiaMedKnuff
{
    public class BombHandler
    {
        /// <summary>
        /// Change Bomb image depending on how many times in a row player has rolled a six
        /// </summary>
        /// <param name="numberOfSix"></param>
        /// <param name="bombImage"></param>
        public static async void ChangeBombImage(int numberOfSix, Image bombImage)
        {
            var element = new MediaElement();
            var bombImageSource = new BitmapImage(new Uri($"ms-appx:///Assets/Bombs/bombState-{numberOfSix}.png"));
            bombImage.Source = bombImageSource;
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = null;

            switch (numberOfSix)
            {
                case 0:
                    file = null;
                    break;
                case 1:
                case 2:
                    file = await folder.GetFileAsync("fuse.mp3");
                    break;
                case 3:
                    file = await folder.GetFileAsync("explosion.mp3");
                    break;
                default:
                    break;
            }
            if (numberOfSix == 3)
            {
                BombExplosion();
                MainPage.Instance.bigboom();
                MainPage.Instance.numberOfSixInARow = 0;
            }
            if (MainPage.Instance.isSoundOn == true && file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                element.SetSource(stream, file.ContentType);
                element.Play();

                await Task.Delay(2000);

                element.Stop();
            }
        }

        /// <summary>
        /// Resets the pawns for all players but the player that managed to roll 6 for three times in a row
        /// </summary>
        public static async void BombExplosion()
        {
            MainPage.Instance.ExplosionImage.Visibility = Visibility.Visible;
            await Task.Delay(1000);
            foreach (object Object in MainPage.Instance.BoardInstance.Children)
            {
                // reset pawn if the object is a pawn and not at spawn or goal and not the same color as the current player
                if (Object is Rectangle pawn && !MainPage.Instance.PawnHandler.isAtSpawn(pawn) && !MainPage.Instance.PawnHandler.pawnHasReachedGoal(pawn) && !pawn.Name.Contains(MainPage.Instance.colors[MainPage.Instance.playerturn - 1]))
                {
                    MainPage.Instance.PawnHandler.resetPawn(pawn);
                }
            }
            ChangeBombImage(0, MainPage.Instance.BombImage);
            MainPage.Instance.ExplosionImage.Visibility = Visibility.Collapsed;
        }
    }
}
