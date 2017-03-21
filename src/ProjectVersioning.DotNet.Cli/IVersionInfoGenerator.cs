using System;

namespace ProjectVersioning.DotNet.Cli
{
    interface IVersionInfoGenerator
    {
        void Generate(string projectPath, Version version, string versionString);
    }
}