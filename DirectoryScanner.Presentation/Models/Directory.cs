using DirectoryScanner.Presentation.Interfaces;
using DirectoryScanner.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner.Presentation.Models
{
    public class Directory : IFileSystemObject
    {
        public Directory(string name, long size, float sizeInPercents)
        {
            Directories = new ObservableCollection<IFileSystemObject>();
            Name = name;
            Size = size;
            SizeInPercents = sizeInPercents;
        }

        public string Name { get; }
        public long Size { get; }
        public float SizeInPercents { get; }
        public ObservableCollection<IFileSystemObject> Directories { get; }
    }
}
