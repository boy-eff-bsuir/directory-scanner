using System;
using System.Diagnostics;
using System.IO;
using DirectoryScanner.Application;
using Xunit;

namespace DirectoryScanner.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        long bestTime = 10000000;
        long bestThreadsCount = 0;
        var directory = @"D:\";
        Scanner sut;
        var watch = new Stopwatch();
        for (int i = 50; i <= 400; i +=50)
        {
            sut = new Scanner();
            watch.Start();
            sut.StartScanning(directory, i);
            watch.Stop();
            Console.WriteLine($"Execution Time multi-threading: {watch.ElapsedMilliseconds} ms. Threads count - {i}");
            if (watch.ElapsedMilliseconds < bestTime)
            {
                bestTime = watch.ElapsedMilliseconds;
                bestThreadsCount = i;
            }
            watch.Reset();
        }
        sut = new Scanner();
        watch.Start();
        sut.StartScanningInOneThread(directory);
        watch.Stop();
        Console.WriteLine($"Execution Time in one thread: {watch.ElapsedMilliseconds} ms.");
        watch.Reset();
        System.Console.WriteLine($@"Best execution time for directory {directory} is {bestTime} with {bestThreadsCount} threads");
    }
}