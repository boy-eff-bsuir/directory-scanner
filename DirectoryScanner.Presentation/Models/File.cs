using DirectoryScanner.Presentation.Interfaces;

namespace DirectoryScanner.Presentation.Models
{
    public class File : IFileSystemObject
    {
        public File(string name, long size, float sizeInPercents)
        {
            Name = name;
            Size = size;
            SizeInPercents = sizeInPercents;
        }

        public string Name { get; }
        public long Size { get; }
        public float SizeInPercents { get; }
    }
}