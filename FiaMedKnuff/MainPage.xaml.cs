using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Windows;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FiaMedKnuff
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            populateBoard();
        }

        private void populateBoard() 
        {
            Board.RowDefinitions.Clear();
            Board.ColumnDefinitions.Clear();

            //add rows and columns to the Board grid
            for (int i = 0; i < 13; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                Board.RowDefinitions.Add(rowDefinition);
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(50);
                Board.ColumnDefinitions.Add(columnDefinition);
            }

            //add top tiles
            for (int i = 0; i < 5; i++)
            {
                addellipse(i, 5, Colors.White);
                addellipse(i, 6, Colors.Red);
                addellipse(i, 7, Colors.White);
            }

            //add bottom tiles
            for (int i = 8;  i < 13; i++)
            {
                addellipse(i, 5, Colors.White);
                addellipse(i, 6, Colors.Yellow);
                addellipse(i, 7, Colors.White);
            }
            //add left tiles
            for (int i = 0; i < 5; i++)
            {
                addellipse(5, i, Colors.White);
                addellipse(6, i, Colors.Blue);
                addellipse(7, i, Colors.White);
            }
            //add right tiles
            for(int i = 8;i < 13; i++) 
            {
                addellipse(5, i, Colors.White);
                addellipse(6, i, Colors.Green);
                addellipse(7, i, Colors.White);
            }
            //add middle tiles
            {
                addellipse(5, 5, Colors.White);
                addellipse(5, 7, Colors.White);
                addellipse(7, 5, Colors.White);
                addellipse(7, 7, Colors.White);
                addellipse(6, 6, Colors.Black);
                addellipse(5, 6, Colors.Red);
                addellipse(6, 5, Colors.Blue);
                addellipse(6, 7, Colors.Green);
                addellipse(7, 6, Colors.Yellow);
            }

        }

        private void addellipse(int row , int column, Color color)
        {
            Ellipse ellipse = createElipse(color);
            Grid.SetRow(ellipse, row);
            Grid.SetColumn(ellipse, column);
            Board.Children.Add(ellipse);
        }

        private Ellipse createElipse(Color color)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(color),
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Black),
            };
            return ellipse;
        }
    }
}
