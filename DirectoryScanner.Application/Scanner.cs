using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectoryScanner.Application.Interfaces;
using DirectoryScanner.Application.Models;

namespace DirectoryScanner.Application
{
    public class Scanner : IScanner
    {
        private readonly object sync = new object();
        private const int waitingTime = 50;
        private Stopwatch stopwatch = new Stopwatch();
        private Tree _tree;
        private SemaphoreSlim _semaphore;
        private ConcurrentQueue<Node> _queue = new ConcurrentQueue<Node>();
        public Tree StartScanning(string pathToDirectory, int threadsCount)
        {
            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);
            if (!Directory.Exists(pathToDirectory))
            {
                throw new Exception("Path does not exist");
            }

            _tree = new Tree(pathToDirectory, null);
            ScanRecursive(_tree.Root);
            do
            {
                for (int i = 0; i < _queue.Count; i++)
                {
                    Node node;
                    var dequeueSuccess = _queue.TryDequeue(out node);
                    if (dequeueSuccess)
                    {
                        _semaphore.Wait();
                        Task.Run(() => {
                            ScanRecursive(node);
                            _semaphore.Release();
                        });
                    }
                }
            } while (!_queue.IsEmpty || _semaphore.CurrentCount != threadsCount);
            return _tree;
        }

        public Tree StartScanningInOneThread(string pathToDirectory)
        {
            if (!Directory.Exists(pathToDirectory))
            {
                throw new Exception("Path does not exist");
            }

            _tree = new Tree(pathToDirectory, null);
            ScanRecursive(_tree.Root);
            Node node;
            do
            {
                var dequeueSuccess = _queue.TryDequeue(out node);
                ScanRecursive(node);
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
                    var childNode = new Node(directory.FullName, node);
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