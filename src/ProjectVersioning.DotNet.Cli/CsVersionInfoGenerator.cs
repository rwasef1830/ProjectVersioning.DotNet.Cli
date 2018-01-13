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
                $@"using System.Reflection;

[assembly: AssemblyVersion(""{version}"")]
[assembly: AssemblyFileVersion(""{version}"")]
[assembly: AssemblyInformationalVersion(""{versionString}"")]";

            if (File.Exists(versionInfoPath))
            {
                var existingFileContents = File.ReadAllText(versionInfoPath);
                if (string.Equals(newFileContents, existingFileContents, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(versionInfoPath));
            File.WriteAllText(versionInfoPath, newFileContents);
        }
    }
}