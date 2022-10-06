using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DirectoryScanner.Application;
using DirectoryScanner.Application.Services;
using Xunit;

namespace DirectoryScanner.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1Async()
    {
        long bestTime = 10000000;
        long bestThreadsCount = 0;
        var directory = @"D:\";
        var service = new TreeTraverseService();
        Scanner sut = new Scanner(service);
        var watch = new Stopwatch();
        for (int i = 50; i <= 400; i +=50)
        {
            watch.Start();
            sut.StartScanning(directory, directory, i);
            watch.Stop();
            Console.WriteLine($"Execution Time multi-threading: {watch.ElapsedMilliseconds} ms. Threads count - {i}");
            if (watch.ElapsedMilliseconds < bestTime)
            {
                bestTime = watch.ElapsedMilliseconds;
                bestThreadsCount = i;
            }
            watch.Reset();
        }
        System.Console.WriteLine($@"Best execution time for directory {directory} is {bestTime} with {bestThreadsCount} threads");
    }
}