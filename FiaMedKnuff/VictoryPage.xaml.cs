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
        
        private void uploadToHighscore()
        {
            string name = winnerNameTextBox.Text;
            if (name != string.Empty)
            {
                HighscorePage.instance.tryAddRecord(name, winnerMoves);
            }
        }

        public void loadPage(string winnerColor, int moves)
        {
            victoryText.Text = winnerColor + " har vunnit med "+moves+" drag!";
            winnerNameTextBox.Text = "";
            winnerMoves = moves;
        }

        private void ChangeColorOnHover(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorOnHover(sender);
        }

        private void ChangeBackColorToDefault(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorBackToDefault(sender);
        }
        private void BackToMenu(object sender, PointerRoutedEventArgs e)
        {
            MainMenu.Instance.ShowMainMenu();
            MainPage.Instance.VictoryScreen.Visibility = Visibility.Collapsed;
            uploadToHighscore();
        }

        private void continuePlaying(object sender, PointerRoutedEventArgs e)
        {
            MainPage.Instance.VictoryScreen.Visibility = Visibility.Collapsed;
            uploadToHighscore();
        }
    }
}
