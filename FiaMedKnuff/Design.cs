using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FiaMedKnuff
{
    /// <summary>
    /// Contains methods for changing the design of the application. 
    /// </summary>
    public class Design
    {
        /// <summary>
        /// Creates a SolidColorBrush from a hex code. The hex code can be either 6 or 8 characters long. If it's 6 characters long, the alpha value is set to 255.
        /// </summary>
        /// <param name="hexCode">A hexcode with or without alpha value</param>
        /// <returns><see cref="SolidColorBrush"/> that matches the given hexcode</returns>
        public static SolidColorBrush CreateSolidColorBrushFromHex(string hexCode)
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
            return solidColorBrush;
        }

        /// <summary>
        /// Changes the background color of the button to a darker color and lighter foreground when the user hovers over it.
        /// </summary>        
        /// <param name="sender">The button that the user is hovering over</param>
        public static void ChangeButtonColorOnHover(object sender)
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
        /// Changes the foreground and background color of the button back to the default colors when the user stops hovering over it.
        /// </summary>
        /// <param name="sender">The button that the user is no longer hovering over</param>
        public static void ChangeButtonColorBackToDefault(object sender)
        {
            Grid grid = (Grid)sender;
            grid.Background = CreateSolidColorBrushFromHex("#CC6A9F");
            TextBlock textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public static void ChangeBombImage(int numberOfSix, Image bombImage)
        {
            var bombImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri($"ms-appx:///Assets/Bombs/bombState-{numberOfSix}.png"));
            bombImage.Source = bombImageSource;
        }
    }
}
