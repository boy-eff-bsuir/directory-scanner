using DirectoryScanner.Application;
using DirectoryScanner.Application.Services;
using DirectoryScanner.Presentation.Commands;
using DirectoryScanner.Presentation.Extensions;
using DirectoryScanner.Presentation.Interfaces;
using DirectoryScanner.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirectoryScanner.Presentation.ViewModels
{
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        private ICommand _changeDirectoryCommand;
        private ICommand _cancelDirectorySearchCommand;
        private ObservableCollection<IFileSystemObject> _rootDirectory;
        private Scanner _scanner;

        public ApplicationViewModel()
        {
            var treeTraverseService = new TreeTraverseService();
            _scanner = new Scanner(treeTraverseService);
        }

        public ObservableCollection<IFileSystemObject> RootDirectory 
        {
            get 
            {
                return _rootDirectory;
            }
            set
            {
                _rootDirectory = value;
                Notify(nameof(RootDirectory));
            }
        }

        public ICommand ChangeDirectoryCommand 
        {
            get 
            {
                return _changeDirectoryCommand ?? (_changeDirectoryCommand = new ChangeDirectoryCommand( (obj) => { return true; }, async obj => {
                    var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                    if (dialog.ShowDialog().GetValueOrDefault())
                    {
                        _rootDirectory = new ObservableCollection<IFileSystemObject>();
                        await Task.Run(() =>
                        {
                            var result = _scanner.StartScanning(dialog.SelectedPath, dialog.SelectedPath, 150);
                            RootDirectory = new ObservableCollection<IFileSystemObject>
                            {
                                result.ToDto()
                            };
                        });
                    }
                }));
            }
        }

        public ICommand CancelDirectorySearchCommand
        {
            get
            {
                return _cancelDirectorySearchCommand ?? (_cancelDirectorySearchCommand = new ChangeDirectoryCommand((obj) => { return true; }, obj => {
                    _scanner.CancelScanning();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
