using DirectoryScanner.Application.Models;
using DirectoryScanner.Presentation.Interfaces;
using DirectoryScanner.Presentation.Models;

namespace DirectoryScanner.Presentation.Extensions
{
    public static class TreeExtensions
    {
        public static IFileSystemObject ToDto(this Tree tree)
        {
            var result = ToTreeViewRecusive(tree.Root);
            return result;
        }

        private static IFileSystemObject ToTreeViewRecusive(Node node)
        {
            var item = new Directory(
                node.Directory.Name,
                node.Directory.Size,
                ((float)node.Directory.Size / node.Parent?.Directory?.Size ?? 1) * 100
            );
     
            foreach(var child in node.Children)
            {
                var childResult = ToTreeViewRecusive(child); 
                item.Directories.Add(childResult);
            }

            foreach (var file in node.Directory.Files)
            {
                var childItem = new File(
                    file.Name,
                    file.Size,
                    (float)file.Size * 100 / item.Size
                );
                item.Directories.Add(childItem);
            }
            return item;
        }
    }
}
