using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DirectoryScanner.Application;
using DirectoryScanner.Application.Models;
using DirectoryScanner.Application.Services;
using FluentAssertions;
using Xunit;

namespace DirectoryScanner.Tests;

public class ScannerTests
{
    // [Fact]
    // public void FindOptimalThreadsCount()
    // {
    //     long bestTime = 10000000;
    //     long bestThreadsCount = 0;
    //     var directory = @"D:\";
    //     var service = new TreeTraverseService();
    //     Scanner sut = new Scanner(service);
    //     var watch = new Stopwatch();
    //     for (int i = 50; i <= 400; i +=50)
    //     {
    //         watch.Start();
    //         sut.StartScanning(directory, i);
    //         watch.Stop();
    //         Console.WriteLine($"Execution Time multi-threading: {watch.ElapsedMilliseconds} ms. Threads count - {i}");
    //         if (watch.ElapsedMilliseconds < bestTime)
    //         {
    //             bestTime = watch.ElapsedMilliseconds;
    //             bestThreadsCount = i;
    //         }
    //         watch.Reset();
    //     }
    //     System.Console.WriteLine($@"Best execution time for directory {directory} is {bestTime} with {bestThreadsCount} threads");
    // }

    [Fact]
    public void ShouldWorkIfValid()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var service = new TreeTraverseService();
        var sut = new Scanner(service);

        var result = sut.StartScanning(path, 50);

        result.Should().NotBeNull();
        result.Root.Directory.FullPath.Should().BeEquivalentTo(path);
        result.Root.Directory.Name.Should().BeEquivalentTo(new DirectoryInfo(path).Name);
    }

    [Fact]
    public void ShouldThrowIfPathNotValid()
    {
        var path = "never gonna give you up";
        var service = new TreeTraverseService();
        var sut = new Scanner(service);

        Action result = () => sut.StartScanning(path, 50);
        result.Should().Throw<Exception>();
    }

    [Fact]
    public void ShouldThrowIfThreadsAmountNotValid()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var service = new TreeTraverseService();
        var sut = new Scanner(service);

        Action result = () => sut.StartScanning(path, 0);
        result.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ShouldCancelExecution()
    {
        var path = @"D:\";
        Tree resultWithCancellation = null;
        var service = new TreeTraverseService();
        var sutWithoutCancellation = new Scanner(service);
        var sutWithCancellation = new Scanner(service);

        var resultWithoutCancellation = sutWithoutCancellation.StartScanning(path, 50);
        var task = Task.Run(() => resultWithCancellation = sutWithCancellation.StartScanning(path, 50));
        sutWithCancellation.CancelScanning();
        Task.WaitAll(task);

        resultWithoutCancellation.Root.Directory.Size.Should().BeGreaterThanOrEqualTo(
            resultWithCancellation.Root.Directory.Size
        );
    }
}