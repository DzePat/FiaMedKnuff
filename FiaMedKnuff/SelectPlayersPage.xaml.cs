using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Runtime.ConstrainedExecution;
using Windows.Perception.Spatial.Preview;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Windows.UI.Xaml.Input;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace FiaMedKnuff
{

    public sealed partial class SelectPlayersPage : UserControl
    {
        private int selectedNumber = 2;
        private bool[] AI = new bool[4];
        private readonly int buttonSize = 64;
        private List<Button> Playerbuttons = new List<Button>();
        private List<Button> aiButtons = new List<Button>();
        public Dictionary<int,string> Players = new Dictionary<int,string>();

        public static SelectPlayersPage Instance { get; private set; }
        public StackPanel selectPlayersPage { get { return pageStackPanel;} }

        public SelectPlayersPage()
        {
            this.InitializeComponent();
            Instance = this;
            Color[] colors = {Colors.Yellow, Colors.Blue, Colors.Red, Colors.Green};

            for (int i = 0; i < 4; i++)
            {
                Button playerButton = CreatePlayerButton(i + 1, colors[i]);
                Playerbuttons.Add(playerButton);
                Button aiButton = createAIButton(i+1);
                aiButtons.Add(aiButton);
                StackPanel panel = new StackPanel();
                panel.HorizontalAlignment = HorizontalAlignment.Stretch;
                panel.VerticalAlignment = VerticalAlignment.Stretch;
                aiButton.HorizontalAlignment= HorizontalAlignment.Center;
                panel.Children.Add(playerButton);
                panel.Children.Add(aiButton);
                playerButtonStackPanel.Children.Add(panel);
            }

            selectPlayer(2);
        }

        ///<summary>
        ///Creates a new button displaying a number to be used for selecting the number of players.
        ///</summary>
        ///
        /// <return>
        /// the new button
        /// </return>
        private Button CreatePlayerButton(int playerNum, Color color)
        {
            Button button = new Button();
            button.Name = "Player"+playerNum+"Button";
            button.Content = ""+playerNum;
            button.Width = buttonSize;
            button.Height = buttonSize;
            button.BorderThickness = new Thickness(5);
            button.Background = new SolidColorBrush(color);
            button.Foreground = new SolidColorBrush(Colors.Black);
            button.CornerRadius = new CornerRadius(buttonSize);
            button.BorderBrush = new SolidColorBrush(Colors.Black);
           
            button.FontFamily = new FontFamily("Arial");
            button.FontSize = 35;
            button.Click += SelectPlayerClick;
            return button;
        }

        ///<summary>
        ///Creates a new button for toggling AI on and off.
        ///</summary>
        ///
        /// <return>
        /// the new button
        /// </return>
        private Button createAIButton(int AInum)
        {
            Button button = new Button();
            button.Name = "AI" + AInum + "Button";
            button.Content = "AI";
            button.Width = buttonSize*0.7;
            button.Height = buttonSize*0.7;
            button.BorderThickness = new Thickness(5);
            button.Background = new SolidColorBrush(Colors.DarkGray);
            button.Foreground = new SolidColorBrush(Colors.Black);
            button.CornerRadius = new CornerRadius(buttonSize);
            button.BorderBrush = new SolidColorBrush(Colors.Black);

            button.FontFamily = new FontFamily("Arial");
            button.FontSize = 14;
            button.Click += AIButton_Click; ;
            return button;
        }
        ///<summary>
        ///Event function when AI butotns are clicked
        ///toggles the button on and off when clicking
        ///</summary>
        private void AIButton_Click(object sender, RoutedEventArgs e)
        {
            int num = aiButtons.IndexOf((Button)sender);
            if (AI[num])
            {
                AI[num] = false;
                aiButtons[num].Background = new SolidColorBrush(Colors.DarkGray);
            } else
            {
                AI[num] = true;
                aiButtons[num].Background= new SolidColorBrush(Colors.White);
            }
            addPlayer(num + 1, "AI");
        }

        ///<summary>
        ///Event function when player butotns are clicked
        ///</summary>
        private void SelectPlayerClick(object sender, RoutedEventArgs e)
        {
            selectPlayer(Playerbuttons.IndexOf((Button)sender) + 1);
        }


        ///<summary>
        ///sets the selected number of players and highlights the selected buttons border.
        ///</summary>
        private void selectPlayer(int number)
        {
            selectedNumber = number;

            foreach (Button button in Playerbuttons)
            {
                button.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            Playerbuttons[selectedNumber-1].BorderBrush= new SolidColorBrush(Colors.White);
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
        /// add a player or Ai to the dictionary
        /// </summary>
        /// <param name="number"></param>
        /// <param name="identity"></param>
        private void addPlayer(int number , string identity) 
        { 
            if(Players.ContainsKey(number))
            {
                Players[number] = identity;
            }
            else 
            { 
                Players.Add(number, identity);
            }
        }

        /// <summary>
        /// Starts the game after selecting the players
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// TBD add actual selection as of right now it just launches the game with all 4 players selected
        private void startButtonSelect(object sender, PointerRoutedEventArgs e)
        {
            MainMenu.Instance.Visibility = Visibility.Collapsed;
            MainPage.Instance.ImageSource.Visibility = Visibility.Visible; 
        }

        private void backButtonSelect(object sender, PointerRoutedEventArgs e)
        {
            MainMenu.Instance.MainMenuContent.Visibility = Visibility.Visible;
            MainMenu.Instance.SelectPlayerMenu.Visibility = Visibility.Collapsed;
        }
    }
}
