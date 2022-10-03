using DirectoryScanner.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner.Application.Services
{
    public class TreeTraverseService
    {
        public void Traverse(Tree tree, Action<Node> action)
        {
            RecursiveTraverse(tree.Root, action);
        }

        private void RecursiveTraverse(Node node, Action<Node> action)
        {
            foreach(var child in node.Children)
            {
                RecursiveTraverse(child, action);
            }
            action.Invoke(node);
        }
    }
}
