using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner.Presentation.Interfaces
{
    public interface IFileSystemObject
    {
        string Name { get; }
        long Size { get; }
        float SizeInPercents { get; }
    }
}
