using System;
using System.Collections.Concurrent;
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

        public Node(string name, string path, Node parent) 
        {
            this.Directory = new DirectoryModel(name, path);
            this.Parent = parent;
        }

        public DirectoryModel Directory { get; }
        public List<Node> Children { get; set; } = new List<Node>();
        public Node Parent { get; }
    }
}