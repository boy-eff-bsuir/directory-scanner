using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DirectoryScanner.Application.Interfaces;
using DirectoryScanner.Application.Models;
using DirectoryScanner.Application.Services;

namespace DirectoryScanner.Application
{
    public class Scanner : IScanner
    {
        private ConcurrentQueue<Node> _queue = new ConcurrentQueue<Node>();
        private CancellationTokenSource _cancellationTokenSource;
        private TreeTraverseService _treeTraverseService;

        public Scanner(TreeTraverseService treeTraverseService)
        {
            _treeTraverseService = treeTraverseService;
        }

        public void CancelScanning()
        {
            _cancellationTokenSource?.Cancel();
        }

        public Tree StartScanning(string name, string path, int threadsCount)
        {
            var semaphore = new SemaphoreSlim(threadsCount, threadsCount);
            _cancellationTokenSource = new CancellationTokenSource();
            if (!Directory.Exists(path))
            {
                throw new Exception("Path does not exist");
            }

            var tree = new Tree(name, path, null);
            Scan(tree.Root, _cancellationTokenSource.Token);
            do
            {
                for (int i = 0; i < _queue.Count; i++)
                {
                    Node node;
                    var dequeueSuccess = _queue.TryDequeue(out node);
                    if (dequeueSuccess)
                    {
                        semaphore.Wait();
                        Task.Run(() => {
                            Scan(node, _cancellationTokenSource.Token);
                            semaphore.Release();
                        });
                    }
                }
            } while ((!_queue.IsEmpty 
                || semaphore.CurrentCount != threadsCount)
                && !_cancellationTokenSource.IsCancellationRequested
                );
                
            _treeTraverseService.PostorderTraverse(tree, (node) => {
                if (node.Parent != null)
                node.Parent.Directory.Size += node.Directory.Size;
            });
            return tree;
        }

        private void Scan(Node node, CancellationToken token)
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
                    token.ThrowIfCancellationRequested();
                    var childNode = new Node(directory.Name, directory.FullName, node);
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