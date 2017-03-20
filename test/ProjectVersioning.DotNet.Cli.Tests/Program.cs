using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnitLite;

namespace ProjectVersioning.DotNet.Cli.Tests
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    static class Program
    {
        static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly).Execute(args);
        }
    }
}