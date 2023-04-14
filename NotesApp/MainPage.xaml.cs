using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotesApp
{
    public partial class MainPage : Page
    {
        List<Note> notesList = new();
        FileIOServices _file;
        private string path;
        string query;
        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                query = "SELECT * FROM Notes";
                notesList = SQLHelper.executeReadingQuery(query);
                if (notesList.Count > 0)
                {
                    foreach (Note note in notesList)
                    {
                        path = $"{Environment.CurrentDirectory}\\Dictionaries\\{note.Id}.json";
                        _file = new FileIOServices(path);
                        Dictionary<char, string> d = _file.ReadingData();
                        note.DecodedText = CryptoServices.Decrypt(d, note.EncodedText);
                    }
                    notesList = notesList.OrderByDescending(a => a.CreationDateTime).ToList();
                }
            }
            catch
            {
                MessageBox.Show("Somethig went wrong while loading data from db");
            }
            NotesListView.ItemsSource = notesList;
        }

        private void AddNewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditingDetailsPage(new Note()));
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var no = NotesListView.SelectedItem as Note;
            if (no != null)
            {
                NavigationService.Navigate(new EditingDetailsPage(no));
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                NotesListView.ItemsSource = notesList;
            else
            {
                var notesFound = KMP.Search(SearchTextBox.Text, notesList);
                NotesListView.ItemsSource = notesFound;
            }
        }
    }
}
