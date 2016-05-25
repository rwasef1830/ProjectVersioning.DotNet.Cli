using System;
using System.IO;
using System.Text;

namespace DotNet.Tool.Version.Project
{
    class CsVersionInfoGenerator : IVersionInfoGenerator
    {
        public void Generate(string projectPath, System.Version version, string versionString)
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

            var versionInfoFileContents =
                $@"using System.Reflection;

[assembly: AssemblyVersion(""{version}"")]
[assembly: AssemblyFileVersion(""{version}"")]
[assembly: AssemblyInformationalVersion(""{versionString}"")]";

            File.WriteAllText(versionInfoPath, versionInfoFileContents, Encoding.UTF8);
        }
    }
}