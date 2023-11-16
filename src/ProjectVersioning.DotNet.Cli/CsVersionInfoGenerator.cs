using System;
using System.IO;

namespace ProjectVersioning.DotNet.Cli
{
    class CsVersionInfoGenerator : IVersionInfoGenerator
    {
        public void Generate(string projectPath, Version version, string versionString)
        {
            if (projectPath == null) throw new ArgumentNullException(nameof(projectPath));
            if (version == null) throw new ArgumentNullException(nameof(version));
            if (versionString == null) throw new ArgumentNullException(nameof(versionString));

            if (!File.Exists(projectPath) && !Directory.Exists(projectPath))
            {
                throw new FileNotFoundException("Could not find path", projectPath);
            }

            if (File.Exists(projectPath))
            {
                projectPath = Path.GetDirectoryName(projectPath);
            }

            if (projectPath == null)
            {
                throw new ArgumentException("Path became null");
            }

            string versionInfoPath = Path.Combine(projectPath, "Properties", "VersionInfo.cs");

            var newFileContents =
                $@"// ReSharper disable All
#pragma warning disable 0436

using System.Reflection;

[assembly: AssemblyVersion(AssemblyVersionConstants.Version)]
[assembly: AssemblyFileVersion(AssemblyVersionConstants.FileVersion)]
[assembly: AssemblyInformationalVersion(AssemblyVersionConstants.InformationalVersion)]

#ifdef NET7_0
file
#endif
class AssemblyVersionConstants
{{
    public const string Version = ""{version}"";
    public const string FileVersion = ""{version}"";
    public const string InformationalVersion = ""{versionString}"";
}}
";

            if (File.Exists(versionInfoPath))
            {
                var existingFileContents = File.ReadAllText(versionInfoPath);
                if (string.Equals(newFileContents, existingFileContents, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            if (Path.GetDirectoryName(versionInfoPath) is { } directoryName)
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(versionInfoPath, newFileContents);
        }
    }
}
