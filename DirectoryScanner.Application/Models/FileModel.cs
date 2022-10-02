using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Models
{
    public class FileModel
    {
        public FileModel(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public string Name { get; }
        public long Size { get; }
    }
}