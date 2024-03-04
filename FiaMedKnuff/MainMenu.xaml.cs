using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedKnuff
{
    public sealed partial class MainMenu : UserControl
    {

        public static MainMenu Instance { get; private set; }
        public StackPanel MainMenuContent { get { return mainMenuContent; } }
        public Grid highScoreMenu { get {  return highscoreMenu; } }

        public MainMenu()
        {
            this.InitializeComponent();
            FadeinMainMenu.Begin();
            Instance = this;
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

        /// <summary>
        /// shows the highscore session when the user clicks the button. The main menu is hidden and the <see cref="HighscorePage"/> is shown.
        /// </summary>
        private void ShowHighscore(object sender, PointerRoutedEventArgs e)
        {
            mainMenuContent.Visibility = Visibility.Collapsed;
            highscoreMenu.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// shows the main menu view and hides all the others.
        /// </summary>
        public void ShowMainMenu()
        {
            mainMenuContent.Visibility = Visibility.Visible;
            highscoreMenu.Visibility = Visibility.Collapsed;
            selectPlayerMenu.Visibility = Visibility.Collapsed;
        }
    }
}
