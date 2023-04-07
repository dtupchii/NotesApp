using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotesApp
{
    public partial class EditingDetailsPage : Page
    {
        Note editingNote = new Note();
        List<Note> notesList;
        private string path;
        private FileIOServices _file;
        string query;

        public EditingDetailsPage(Note note)
        {
            InitializeComponent();

            editingNote = note;
            DataContext = editingNote;

            textEditTextBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            editingNote.EncodedText = "";

            Dictionary<char, string> csp = CryptoServices.Encrypt(editingNote.DecodedText);

            foreach (char c in editingNote.DecodedText)
                editingNote.EncodedText += csp.Where(k => k.Key == c).FirstOrDefault().Value + " ";

            query = $"SELECT * FROM [NotesAppDB].[dbo].[Notes] WHERE CreationDateTime = '{editingNote.CreationDateTime}'";
            notesList = SQLHelper.executeReadingQuery(query);

            if (notesList != null && notesList.Count > 0)
            {
                Note nn = notesList[0];
                query = $"UPDATE [NotesAppDB].[dbo].[Notes] SET EncodedText = '{editingNote.EncodedText}' WHERE CreationDateTime = '{editingNote.CreationDateTime}'";
                SQLHelper.executeSavingQuery(query);

                editingNote.Id = nn.Id;
            }
            else
            {
                query = $"INSERT [NotesAppDB].[dbo].[Notes] VALUES ('{editingNote.CreationDateTime}', '{editingNote.EncodedText}')";
                SQLHelper.executeSavingQuery(query);
            }

            query = $"SELECT * FROM [NotesAppDB].[dbo].[Notes] WHERE CreationDateTime = '{editingNote.CreationDateTime}'";
            notesList = SQLHelper.executeReadingQuery(query);
            Note n = notesList[0];

            editingNote.Id = n.Id;

            try
            {
                path = $"{Environment.CurrentDirectory}\\Dictionaries\\{editingNote.Id}.json";
                _file = new FileIOServices(path);
                _file.WritingData(csp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't save dictionary to file");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {


            query = $"SELECT * FROM [Notes] WHERE Id = {editingNote.Id}";
            notesList = SQLHelper.executeReadingQuery(query);
            Note n = notesList[0];

            if (n != null)
            {
                query = $"DELETE [Notes] WHERE Id = {editingNote.Id}";
                SQLHelper.executeSavingQuery(query);

                path = $"{Environment.CurrentDirectory}\\Dictionaries\\{editingNote.Id}.json";
                File.Delete(path);
            }
            else
                MessageBox.Show("Something went wrong");

            NavigationService.GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
