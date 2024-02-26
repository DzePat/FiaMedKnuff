using System;
using System.Collections.Generic;
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
            Grid grid = (Grid)sender;
            grid.Background = CreateSolidColorBrushFromHex("#4C1A35");
            TextBlock textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            if (textBlock != null)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.White);
            }
        }

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

            return solidColorBrush;
        }
    }
}
