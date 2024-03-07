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
        public VictoryPage()
        {
            this.InitializeComponent();


        }
        
        private void uploadToHighscore()
        {
            string name = winnerNameTextBox.Text;

            HighscorePage.instance.tryAddRecord(name, winnerMoves);
        }

        public void loadPage(string winnerColor, int moves)
        {
            victoryText.Text = winnerColor + " har vunnit med "+moves+" drag!";
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

    }
}
