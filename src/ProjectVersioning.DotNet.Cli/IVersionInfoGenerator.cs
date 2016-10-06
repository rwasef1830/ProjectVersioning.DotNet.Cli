namespace ProjectVersioning.DotNet.Cli
{
    interface IVersionInfoGenerator
    {
        void Generate(string projectPath, System.Version version, string versionString);
    }
}