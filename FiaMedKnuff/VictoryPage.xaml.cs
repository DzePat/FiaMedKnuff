using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace FiaMedKnuff
{

    public sealed partial class VictoryPage : Page
    {
        private int winnerMoves;
        public static VictoryPage instance { get; private set; }
        public VictoryPage()
        {
            this.InitializeComponent();
            instance = this;

        }
        /// <summary>
        /// tries to add the latest winner to the highscore.
        /// </summary>
        private void uploadToHighscore()
        {
            string name = winnerNameTextBox.Text;
            if (name != string.Empty)
            {
                HighscorePage.instance.tryAddRecord(name, winnerMoves);
            }
        }

        /// <summary>
        /// Initializes the page and must be called before showing every time
        /// </summary>
        /// <param name="winnerColor"></param>
        /// <param name="moves"></param>
        public void loadPage(string winnerColor, int moves)
        {
            victoryText.Text = winnerColor + " har vunnit med "+moves+" drag!";
            winnerNameTextBox.Text = "";
            winnerMoves = moves;
            MainPage.Instance.PlaySound("win");
        }

        private void ChangeColorOnHover(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorOnHover(sender);
        }

        private void ChangeBackColorToDefault(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorBackToDefault(sender);
        }

        /// <summary>
        /// sets up the viws to show the main menu
        /// and uploads the result to the highscore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMenu(object sender, PointerRoutedEventArgs e)
        {
            MainMenu.Instance.ShowMainMenu();
            MainPage.Instance.VictoryScreen.Visibility = Visibility.Collapsed;
            uploadToHighscore();
        }


        /// <summary>
        /// sets up the viws to continue playing the current game.
        /// and uploads the result to the highscore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continuePlaying(object sender, PointerRoutedEventArgs e)
        {
            MainPage.Instance.VictoryScreen.Visibility = Visibility.Collapsed;
            uploadToHighscore();
        }
    }
}
