using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Models
{
    public class Node
    {
        public Node(DirectoryModel directory) 
        {
            this.Directory = directory;
        }

        public Node(string directoryFullPath) 
        {
            this.Directory = new DirectoryModel(directoryFullPath);
        }

        public DirectoryModel Directory { get; }
        public List<Node> Children { get; set; } = new List<Node>();
    }
}