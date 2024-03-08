using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FiaMedKnuff
{
    public sealed partial class MainMenu : UserControl
    {

        public static MainMenu Instance { get; private set; }
        public StackPanel MainMenuContent { get { return mainMenuContent; } }
        public Grid SelectPlayerMenu { get { return selectPlayerMenu; } }
        public Grid HighScoreMenu { get { return highscoreMenu; } }

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
            Instance.MainMenuContent.Visibility = Visibility.Collapsed;
            Instance.SelectPlayerMenu.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// shows the highscore session when the user clicks the button. The main menu is hidden and the <see cref="HighscorePage"/> is shown.
        /// </summary>
        private void ShowHighscore(object sender, PointerRoutedEventArgs e)
        {
            HighscorePage.instance.loadPage();
            Instance.MainMenuContent.Visibility = Visibility.Collapsed;
            Instance.HighScoreMenu.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// shows the main menu view and hides all the others.
        /// </summary>
        public void ShowMainMenu()
        {

            Instance.Visibility= Visibility.Visible;
            Instance.MainMenuContent.Visibility = Visibility.Visible;
            Instance.HighScoreMenu.Visibility = Visibility.Collapsed;
            Instance.SelectPlayerMenu.Visibility = Visibility.Collapsed;
        }
    }
}
