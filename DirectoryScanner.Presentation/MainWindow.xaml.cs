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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}
