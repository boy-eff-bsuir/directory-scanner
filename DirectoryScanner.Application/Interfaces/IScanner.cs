using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryScanner.Application.Models;

namespace DirectoryScanner.Application.Interfaces
{
    public interface IScanner
    {
        Tree StartScanning(string pathToDirectory, int threadsCount);
    }
}