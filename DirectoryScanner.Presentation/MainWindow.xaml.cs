using DirectoryScanner.Application;
using DirectoryScanner.Application.Services;
using DirectoryScanner.Presentation.Extensions;
using DirectoryScanner.Presentation.Interfaces;
using DirectoryScanner.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace DirectoryScanner.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Scanner _scanner;
        private TreeTraverseService _treeTraverseService;
        ObservableCollection<IFileSystemObject> directories;

        public MainWindow()
        {
            InitializeComponent();
            _treeTraverseService = new TreeTraverseService();
            _scanner = new Scanner(_treeTraverseService);
            DataContext = new ApplicationViewModel();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                directories = new ObservableCollection<IFileSystemObject>();
                tvDirectories.Items.Clear();
                await Task.Run(() =>
                {
                    var stopwatch = new Stopwatch();
                    var result = _scanner.StartScanning(dialog.SelectedPath, dialog.SelectedPath, 150);
                    directories.Add(result.ToDto());
                });
                tvDirectories.ItemsSource = directories;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _scanner.CancelScanning();
        }
    }
}
