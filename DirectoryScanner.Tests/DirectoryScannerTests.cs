using System.IO;
using DirectoryScanner.Application;
using Xunit;

namespace DirectoryScanner.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var sut = new Scanner();
        sut.StartScanning(@"C:\Users\Asus\Desktop\5 семестр\МОптим");
    }
}