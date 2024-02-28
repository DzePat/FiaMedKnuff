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

    public sealed partial class HighscorePage : Page
    {
        private List<Record> recordList = new List<Record>();
        private class Record
        {
            public string name;
            public int moves;
        }

            
        public HighscorePage()
        {
            this.InitializeComponent();

            addEntryToGUI("test2", 12);
        }

        private void addEntryToGUI(string name, int moves)
        {
            Grid entry = new Grid();
            entry.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            entry.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameTextBlock = new TextBlock
            {
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock movesTextBlock = new TextBlock
            {
                Text = ""+ moves,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Grid.SetColumn(nameTextBlock, 0);
            Grid.SetColumn(movesTextBlock, 1);

            entry.Children.Add(nameTextBlock);
            entry.Children.Add(movesTextBlock);

            entriesPanel.Children.Add(entry);
        }
    }
}
