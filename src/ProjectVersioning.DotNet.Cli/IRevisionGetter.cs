namespace ProjectVersioning.DotNet.Cli
{
    interface IRevisionGetter
    {
        WorkingCopyVersion GetVersion(string path);
    }
}