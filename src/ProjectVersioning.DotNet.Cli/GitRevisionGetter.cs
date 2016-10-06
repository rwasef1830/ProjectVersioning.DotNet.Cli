using System;
using System.Diagnostics;

namespace ProjectVersioning.DotNet.Cli
{
    class GitRevisionGetter : IRevisionGetter
    {
        public WorkingCopyVersion GetVersion(string path)
        {
            var revisionNumber = uint.Parse(GetOutput(path, "git", "rev-list --count HEAD").Trim());
            var revisionId = GetOutput(path, "git", "rev-parse --verify --short HEAD").Trim();
            var isDirty = GetOutput(path, "git", "status --porcelain").Trim().Length > 0;

            return new WorkingCopyVersion(revisionNumber, revisionId, isDirty);
        }

        static string GetOutput(string path, string command, string arguments)
        {
            using (var process = CreateProcess(path, command, arguments))
            {
                process.Start();

                string stdOut = process.StandardOutput.ReadToEnd().Trim();
                string stdError = process.StandardError.ReadToEnd().Trim();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command failed. Output: {stdError}");
                }

                return stdOut;
            }
        }

        static Process CreateProcess(string path, string command, string arguments)
        {
            return new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = command,
                    Arguments = arguments,
                    WorkingDirectory = path
                }
            };
        }
    }
}