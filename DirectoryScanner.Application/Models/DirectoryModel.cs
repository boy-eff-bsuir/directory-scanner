using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Models
{
    public class DirectoryModel
    {
        public DirectoryModel(string fullPath)
        {
            FullPath = fullPath;
        }
        public string FullPath { get; }
        public long Size { get; internal set; }
        public List<FileModel> Files { get; } = new List<FileModel>();
    }
}