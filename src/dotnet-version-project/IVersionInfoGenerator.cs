namespace DotNet.Tool.Version.Project
{
    interface IVersionInfoGenerator
    {
        void Generate(string projectPath, System.Version version, string versionString);
    }
}