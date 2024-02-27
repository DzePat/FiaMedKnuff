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

        /// <summary>
        /// Changes the background color of the button to a darker color when the user hovers over it.
        /// </summary>
        private void ChangeColorOnHover(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Background = CreateSolidColorBrushFromHex("#4C1A35");
            TextBlock textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// Changes the background color of the button back to the default color when the user stops hovering over it.
        /// </summary>
        private void ChangeBackColorToDefault(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Background = CreateSolidColorBrushFromHex("#CC6A9F");
            TextBlock textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Creates a SolidColorBrush from a hex code. The hex code can be either 6 or 8 characters long. If it's 6 characters long, the alpha value is set to 255.
        /// </summary>
        /// <param name="hexCode">A hexcode with or without alpha value</param>
        /// <returns><see cref="SolidColorBrush"/> that matches the given hexcode</returns>
        public SolidColorBrush CreateSolidColorBrushFromHex(string hexCode)
        {
            if (hexCode.Length == 7)
                hexCode = "#FF" + hexCode.Substring(1);

            Color color = Color.FromArgb(
                Convert.ToByte(hexCode.Substring(1, 2), 16),
                Convert.ToByte(hexCode.Substring(3, 2), 16),
                Convert.ToByte(hexCode.Substring(5, 2), 16),
                Convert.ToByte(hexCode.Substring(7, 2), 16)
            );

            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            Debug.WriteLine(hexCode);
            Debug.WriteLine(solidColorBrush.Color);
            return solidColorBrush;
        }

        /// <summary>
        /// Starts a new game session when the user clicks the button. The main menu is hidden and the <see cref="SelectPlayersPage"/> is shown.
        /// </summary>
        private void StartNewGameSession(object sender, PointerRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
