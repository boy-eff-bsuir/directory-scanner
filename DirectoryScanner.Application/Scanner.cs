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
        private readonly object sync = new object();
        private static int waitingTime = 100;
        private Tree _tree;
        private const int _threadCount = 100;
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
            do
            {
                Node node;
                var dequeueSuccess = _queue.TryDequeue(out node);
                if (dequeueSuccess && node != null)
                {
                    _semaphore.Wait();
                    Task.Run(() => {
                        ScanRecursive(node);
                        _semaphore.Release();
                        lock (sync)
                        {
                            Monitor.Pulse(sync);
                        }
                    });
                }
                if (_queue.IsEmpty)
                {
                    var waitingResult = true;
                    while (waitingResult)
                    {
                        lock (sync)
                        {
                            waitingResult = Monitor.Wait(sync, waitingTime);
                        }
                    }
                }
            } while (!_queue.IsEmpty);
            return _tree;
        }

        private void ScanRecursive(Node node)
        {
            try
            {
                DirectoryInfo directoryInfo;
                DirectoryInfo[] directories;
                var path = node.Directory.FullPath;
                directoryInfo = new DirectoryInfo(path);
                directories = directoryInfo.GetDirectories();
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
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return;
            }
                
        }
    }
}