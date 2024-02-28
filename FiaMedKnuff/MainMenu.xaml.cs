using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedKnuff
{
    public sealed partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            this.InitializeComponent();
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
        /// Starts a new game session when the user clicks the button. The main menu is hidden and the <see cref="SelectPlayersPage"/> is shown.
        /// </summary>
        private void StartNewGameSession(object sender, PointerRoutedEventArgs e)
        {
            mainMenuContent.Visibility = Visibility.Collapsed;
            selectPlayerMenu.Visibility = Visibility.Visible;
        }
    }
}
