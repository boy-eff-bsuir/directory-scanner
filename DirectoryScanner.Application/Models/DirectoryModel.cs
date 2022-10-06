using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Models
{
    public class DirectoryModel
    {
        public DirectoryModel(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
        public string Name { get; }
        public string FullPath { get; }
        public long Size { get; internal set; }
        public List<FileModel> Files { get; } = new List<FileModel>();
    }
}