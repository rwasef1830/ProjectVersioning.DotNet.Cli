using System;
using System.Diagnostics;

namespace DotNet.Tool.Version.Project
{
    class HgRevisionGetter : IRevisionGetter
    {
        public WorkingCopyVersion GetVersion(string path)
        {
            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = "hg",
                    Arguments = "id -ni",
                    WorkingDirectory = path
                }
            };
            process.Start();

            string stdOut = process.StandardOutput.ReadToEnd().Trim();
            string stdError = process.StandardError.ReadToEnd().Trim();
            process.WaitForExit();

            try
            {
                string[] outputComponents = stdOut.Split(' ');
                if (outputComponents.Length != 2)
                {
                    throw new Exception("Expected format changesetId:localRevNumber");
                }

                var revisionId = outputComponents[0].TrimEnd('+');
                var revisionNumber = uint.Parse(outputComponents[1].TrimEnd('+'));
                var isDirty = outputComponents[0].EndsWith("+");

                return new WorkingCopyVersion(revisionNumber, revisionId, isDirty);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to parse hg output: {0}", stdError), ex);
            }
        }
    }
}