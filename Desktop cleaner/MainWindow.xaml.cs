using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

namespace Desktop_cleaner
{
    public partial class MainWindow : Window
    {
        public List<string> FileTypes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            FileTypes = new List<string>
                {
                    ".docx",
                    ".pdf",
                    ".jpg",
                    ".png",
                    ".xls",
                    ".ppt",
                };

            cbFileTypes.ItemsSource = FileTypes;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int movedFiles = 0; 

            foreach (ListViewItem item in lvRules.Items)
            {
                dynamic rules = item.Content;
                string fileType = rules.FileType;
                string destinationPath = rules.FolderPath;
                string sourcePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var filesToMove = Directory.GetFiles(sourcePath, $"*{fileType}");


                foreach (string file in filesToMove)
                {
                    string destinationFile = System.IO.Path.Combine(destinationPath, System.IO.Path.GetFileName(file));
                    if (!File.Exists(destinationFile)) 
                    {
                        File.Move(file, destinationFile);
                        movedFiles++;
                    }
                }
            }
            MessageBox.Show($"{movedFiles} files has been moved", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                CheckFileExists = false,
                FileName = "Select Folder",
                Title = "Select a Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                string folderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                txtFolderPath.Text = folderPath;
            }
        }

        private void BtnAddRule_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbFileTypes.Text) && !string.IsNullOrEmpty(txtFolderPath.Text))
            {
                ListViewItem item = new ListViewItem
                {
                    Content = new { FileType = cbFileTypes.Text, FolderPath = txtFolderPath.Text }
                };
                lvRules.Items.Add(item);
            }
            else
            {
                MessageBox.Show("Please select a file type and a destination folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
