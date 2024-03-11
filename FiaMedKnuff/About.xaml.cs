using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedKnuff
{
    public sealed partial class About : UserControl
    {
        public static About Instance { get; private set; }

        public About()
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

        private void backButton_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            MainMenu.Instance.AboutMenu.Visibility = Visibility.Collapsed;
            MainPage.Instance.BlurGrid.Visibility = Visibility.Collapsed;
            //TODO: Add a method that displays blurgridanimation
            MainMenu.Instance.MainMenuContent.Visibility = Visibility.Visible;
        }
    }
}
