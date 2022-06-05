using System;
using System.Diagnostics;
using System.Threading;

namespace GitMulltyFetch.Model
{
    static class CommandRunner
    {
        internal class ExecutionResult
        {
            public bool IsSuccessful { get;}
            public string Output { get;}

            public ExecutionResult(bool result, string output="")
            {
                IsSuccessful = result;
                this.Output = output;
            }
        }

        public static async void RunCommand(SynchronizationContext context, string command, Action<ExecutionResult> callback, string workingDirector="")
        {
            Process process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + command;

            if(!string.IsNullOrEmpty(workingDirector))
            {
                process.StartInfo.WorkingDirectory = workingDirector;
            }

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            
            string output = await process.StandardOutput.ReadToEndAsync();

            context.Send(state =>
            {
                ExecutionResult result = new ExecutionResult(true, output);
                callback?.Invoke(result);
            }, null);
            
            process.WaitForExit();
        }
    }
}
