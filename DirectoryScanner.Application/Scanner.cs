using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryScanner.Application.Interfaces;
using DirectoryScanner.Application.Models;

namespace DirectoryScanner.Application
{
    public class Scanner : IScanner
    {
        private Tree _tree;
        private const int _threadCount = 10;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(_threadCount, _threadCount);
        private ConcurrentQueue<Node> _queue = new ConcurrentQueue<Node>();
        public Tree StartScanning(string pathToDirectory)
        {
            if (!Directory.Exists(pathToDirectory))
            {
                throw new Exception("Path does not exist");
            }

            _tree = new Tree(pathToDirectory);
            ScanRecursive(_tree.Root);
            Node node;
            do
            {
                var dequeueSuccess = _queue.TryDequeue(out node);
                _semaphore.Wait();
                if (dequeueSuccess)
                {
                    Task.Run(() => {
                        ScanRecursive(node);
                        _semaphore.Release();
                    });
                }

                Thread.Sleep(1000);
            } while (!_queue.IsEmpty);
            return _tree;
        }

        private void ScanRecursive(Node node)
        {
            var directoryInfo = new DirectoryInfo(node.Directory.FullPath);
            var directories = directoryInfo.GetDirectories();
            foreach(var directory in directories)
            {
                var childNode = new Node(directory.FullName);
                node.Children.Add(childNode);
                _queue.Enqueue(childNode);
            }

            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                node.Directory.Files.Add(new FileModel(file.Name, file.Length));
                node.Directory.Size += file.Length;
            }
        }
    }
}