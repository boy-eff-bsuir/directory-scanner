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

        public Tree(string name, string path, Node parent) 
        {
            this.Root = new Node(name, path, parent);
        }

        public Node Root { get; }
    }
}