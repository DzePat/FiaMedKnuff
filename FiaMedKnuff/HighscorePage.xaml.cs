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

using System.IO;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage;
using System.Text;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;

namespace FiaMedKnuff
{

    public sealed partial class HighscorePage : Page
    {
        private List<Record> recordList = new List<Record>();
        private int maxRecords = 5;
        private string fileName = "highscore.txt";
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Interal class to keep track of records
        /// </summary>
        private class Record
        {
            public string name;
            public int moves;

            public Record(string namne, int moves)
            {
                this.name = namne;
                this.moves = moves;
            }
        }

            
        public HighscorePage()
        {
            this.InitializeComponent();


            //executableDirectory=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //tryAddRecord("hej1", 2);
            loadHighscoreFromFile();
            loadPage();

        }
        /// <summary>
        /// Needs to be called every time the highscore pages is shown.
        /// To guarantee the list is updated
        /// </summary>
        public void loadPage()
        {
            //clear all previous listings
            entriesPanel.Children.Clear();

           foreach (Record record in recordList)
            {
                addEntryToGUI(record);
            }
        }

        /// <summary>
        /// Tries to add a new record to the highscore
        /// Will only add if there is room and/or if the
        /// new record is strong enough to fit in the highscore.
        /// </summary>
        /// <returns>
        /// true if the new record could be added. otherwise false.
        /// </returns>
        /// <param name="name">name of the record holder</param>
        /// <param name="moves">the number of moves in the new record</param>
        public bool tryAddRecord(string name, int moves)
        {
            Boolean added = false;
            name.Replace("|", ""); //remove all separator characters not to corrupt the savefile later
            //check if we need to replace a record
            if (recordList.Count >= maxRecords)
            {
                //check if new record is better than current worst record
                if (moves < recordList[recordList.Count - 1].moves)
                {
                    recordList.RemoveAt(recordList.Count - 1); //remove last record
                    recordList.Add(new Record(name, moves));
                    added = true;
                }
            } else //else we just add it
            {
                recordList.Add(new Record(name, moves));
                added = true;
            }

            if (added)
            {
                //sort the whole list
                recordList.Sort((record1, record2) => record1.moves.CompareTo(record2.moves));
                return true;
            } else //did not add any new record
            {
                return false;
            }

        }


        /// <summary>
        /// Saves the current highscore to a file on the machine.
        /// </summary>
        public async Task SaveHighscoreToFile()
        {
            try
            {
                StringBuilder content = new StringBuilder();
                foreach (Record record in recordList)
                {
                    content.AppendLine(record.name + "|" + record.moves);
                }

                // Create (or replace existing) file in the local folder
                StorageFile saveFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                // Write the content to the file
                using (Stream stream = await saveFile.OpenStreamForWriteAsync())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        await writer.WriteAsync(content.ToString());
                    }
                }

                Debug.WriteLine("File write successful!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
        /// <summary>
        /// loads the highscore records from a saved file
        /// </summary>
        public async void loadHighscoreFromFile()
        {
            StorageFile saveFile = await localFolder.GetFileAsync(fileName);

            // Open a stream for the file
            using (IRandomAccessStream stream = await saveFile.OpenAsync(FileAccessMode.Read))
            {
                using (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] words = line.Split('|');
                        recordList.Add(new Record(words[0], Int32.Parse(words[1])));
                    }
                }
            }
        }


        /// <summary>
        /// adds a record to the list of records in the GUI.
        /// </summary>
        /// <param name="record">record to be added</param>
        private void addEntryToGUI(Record record)
        {
            Grid entry = new Grid();
            entry.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            entry.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock nameTextBlock = new TextBlock
            {
                Text = record.name,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 24
            };

            TextBlock movesTextBlock = new TextBlock
            {
                Text = ""+ record.moves,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontSize = 24
            };

            Grid.SetColumn(nameTextBlock, 0);
            Grid.SetColumn(movesTextBlock, 1);

            entry.Children.Add(nameTextBlock);
            entry.Children.Add(movesTextBlock);

            entriesPanel.Children.Add(entry);
        }

        private void ChangeColorOnHover(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorOnHover(sender);
        }

        private void ChangeBackColorToDefault(object sender, PointerRoutedEventArgs e)
        {
            Design.ChangeButtonColorBackToDefault(sender);
        }
        private void BackToMenu(object sender, PointerRoutedEventArgs e)
        {
            MainMenu.Instance.MainMenuContent.Visibility = Visibility.Visible;
            MainMenu.Instance.highScoreMenu.Visibility = Visibility.Collapsed;
        }

    }
}
