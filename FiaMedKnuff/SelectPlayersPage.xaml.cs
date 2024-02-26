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

namespace FiaMedKnuff
{

    public sealed partial class SelectPlayersPage : Page
    {
        private int selectedNumber = 2;
        private readonly int buttonSize = 64;
        private List<Button> Playerbuttons = new List<Button>();
        public SelectPlayersPage()
        {
            this.InitializeComponent();

            Color[] colors = {Colors.Yellow, Colors.Blue, Colors.Red, Colors.Green};

            for (int i = 0; i < 4; i++)
            {
                Button newButton = CreatePlayerButton(i + 1, colors[i]);
                Playerbuttons.Add(newButton);
                playerButtonStackPanel.Children.Add(newButton);
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
        ///Event function when butotns are clicked
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
    }
}
