using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ProjectVersioning.DotNet.Cli
{
    // ReSharper disable UnusedMember.Global
    public class Program
    {
        static readonly string s_Version = typeof(Program).GetTypeInfo()
            .Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "<unknown>";

        public static int Main(string[] args)
        {
            var showHelp = false;
            string directory = Directory.GetCurrentDirectory();
            string type = null;
            string scm = null;
            string version = null;
            string marker = null;

            try
            {
                var p = new OptionSet
                {
                    { "s|scm=", "{SCM} must be one of [hg, git]", v => scm = v },
                    { "t|type=", "{TYPE} can be one of [cs] (if omitted, will output to stdout)", v => type = v },
                    {
                        "v|version=",
                        "version to use. Format: {n.n.n.n}, {n.n.n}, {n.n} or {n}",
                        v => version = v
                    },
                    {
                        "m|marker=",
                        "marker (eg: -m=alpha will produce 1.2.3.4-alpha) (optional)",
                        v => marker = v
                    },
                    { "d|directory=", "work in {DIR} instead of current dir", v => directory = v },
                    { "h|?|help", "show this message and exit", v => showHelp = v != null }
                };

                var unhandled = p.Parse(args);
                if (unhandled.Any())
                {
                    throw new OptionException(
                        $"Unknown parameter{(unhandled.Count > 0 ? "s" : string.Empty)}: {string.Join(", ", unhandled)}",
                        string.Empty);
                }

                if (showHelp)
                {
                    Console.WriteLine("** Project versioning tool version {0}", s_Version);
                    Console.WriteLine(
                        "Generates a version info file for projects based on their working copy's revision.");
                    Console.WriteLine();
                    Console.WriteLine("Options:");
                    p.WriteOptionDescriptions(Console.Out);
                    return -98;
                }

                if (scm == null)
                {
                    throw new OptionException("Missing value required for source control type.", nameof(scm));
                }

                if (version == null)
                {
                    throw new OptionException(
                        "Missing value required for version.",
                        nameof(version));
                }

                var match = Regex.Match(
                    version,
                    @"^(?<Major>\d{1,5})(?:\.(?<Minor>\d{1,5}))?(?:\.(?<Patch>\d{1,5}))?(?:\.\d{1,5})?$");
                if (!match.Success)
                {
                    throw new OptionException(
                        "Invalid version template. Must be in format 1.2.3.4, 1.2.3, 1.2 or 1",
                        nameof(version));
                }

                int major;
                int.TryParse(match.Groups["Major"].Value, out major);

                int minor;
                int.TryParse(match.Groups["Minor"].Value, out minor);

                int patch;
                int.TryParse(match.Groups["Patch"].Value, out patch);

                if (major > ushort.MaxValue)
                {
                    throw new OptionException(
                        $"Invalid major version '{major}'. Maximum is {ushort.MaxValue}.",
                        nameof(version));
                }

                if (minor > ushort.MaxValue)
                {
                    throw new OptionException(
                        $"Invalid minor version '{minor}'. Maximum is {ushort.MaxValue}.",
                        nameof(version));
                }

                if (patch > ushort.MaxValue)
                {
                    throw new OptionException(
                        $"Invalid patch version '{patch}'. Maximum is {ushort.MaxValue}.",
                        nameof(version));
                }

                var versionGetter = RevisionGetter.ForSourceControl(scm);
                var versionInfoGenerator = VersionInfoGenerator.ForLanguage(type ?? string.Empty);
                var wcVersion = versionGetter.GetVersion(directory);

                var versionObj = wcVersion.ToVersion(major, minor);
                var versionString = wcVersion.ToVersionString(major, minor, patch, marker);
                versionInfoGenerator.Generate(directory, versionObj, versionString);

                return 0;
            }
            catch (OptionException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try --help for more information.");
                return -99;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled error: {0}", ex);
                return -1;
            }
        }
    }
}
