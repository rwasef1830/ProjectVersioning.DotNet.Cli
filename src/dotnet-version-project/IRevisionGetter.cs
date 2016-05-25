namespace DotNet.Tool.Version.Project
{
    interface IRevisionGetter
    {
        WorkingCopyVersion GetVersion(string path);
    }
}