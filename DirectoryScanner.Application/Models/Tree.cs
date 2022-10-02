using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Models
{
    public class Tree
    {
        public Tree(Node root) 
        {
            this.Root = root;
        }

        public Tree(string directoryPath) 
        {
            this.Root = new Node(directoryPath);
        }

        public Node Root { get; }
    }
}