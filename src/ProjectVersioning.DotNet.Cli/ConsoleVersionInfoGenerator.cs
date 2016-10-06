using System;

namespace ProjectVersioning.DotNet.Cli
{
    class ConsoleVersionInfoGenerator : IVersionInfoGenerator
    {
        public void Generate(string projectPath, Version version, string versionString)
        {
            Console.WriteLine("Numeric version: {0}", version.ToString());
            Console.WriteLine("Textual version: {0}", versionString);
        }
    }
}