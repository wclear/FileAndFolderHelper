using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace FileAndFolderHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void setSearchDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            using (WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += worker_DoWork;
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted; 
                    worker.RunWorkerAsync(folderBrowser.SelectedPath);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<FileModel> files = e.Result as List<FileModel>;
            List<FileModel> filesSource = files.OrderBy(f => f.FileName).ToList();
            if (filesSource.Count > 1)
            {
                for (int i = 1; i < filesSource.Count; i++)
                {
                    if (filesSource[i].FileName.Equals(filesSource[i - 1].FileName)
                        && filesSource[i].ByteSize == filesSource[i - 1].ByteSize)
                    {
                        filesSource[i].IsDuplicated = true;
                        filesSource[i - 1].IsDuplicated = true;
                    }
                }
            }
            
            filesDataGrid.ItemsSource = filesSource;
            ICollectionView dataView = CollectionViewSource.GetDefaultView(filesDataGrid.ItemsSource);
            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription("IsDuplicated", ListSortDirection.Descending));
            dataView.Refresh();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<FileModel> files = new List<FileModel>();
            searchDirectory(e.Argument.ToString(), files);
            e.Result = files;
        }

        private void searchDirectory(string directory, List<FileModel> files)
        {
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly))
            {
                FileInfo info = new FileInfo(file);
                FileModel model = new FileModel()
                {
                    ByteSize = info.Length,
                    FileName = info.Name,
                    FullFileName = file
                };

                files.Add(model);
            }

            foreach (string innerDirectory in Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                searchDirectory(innerDirectory, files);
            }
        }

        private void filesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private List<FileModel> GetDuplicates(FileModel selected)
        {
            List<FileModel> datasource = filesDataGrid.ItemsSource as List<FileModel>;
            return datasource == null
                ? null
                : (from f in datasource
                    where f.FileName.Equals(selected.FileName)
                    && f.ByteSize == selected.ByteSize
                    select f).ToList();
        }

        private void filesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileModel selected = (e.Source as DataGrid).SelectedItem as FileModel;
            if (selected == null)
            {
                return;
            }

            List<FileModel> results = GetDuplicates(selected);
            DuplicatesWindow dupWindow = new DuplicatesWindow(results);
            dupWindow.Show();
            dupWindow.Focus();
        }
    }
}
