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
using System.Windows.Shapes;

namespace FileAndFolderHelper
{
    /// <summary>
    /// Interaction logic for DuplicatesWindow.xaml
    /// </summary>
    public partial class DuplicatesWindow : Window
    {
        public DuplicatesWindow(List<FileModel> duplicateList)
        {
            InitializeComponent();
            duplicatesDataGrid.ItemsSource = duplicateList;
        }
    }
}
