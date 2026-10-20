using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkeratorFast.Helpers;

public static class ProgramRunnerHelper
{
    
    /// <summary>
    /// Runs an external program and captures the output. Thanks to: https://sqlpey.com/c%23/execute-external-programs-csharp/
    /// </summary>
    /// <param name="filename">The name of the executable to be run</param>
    /// <param name="arguments">The CLI arguments to be passed to the executable</param>
    /// <returns>The response StdOut or StdErr if an error is encountered</returns>
    /// <exception cref="Exception"></exception>
    public static string RunExternalProgram(string filename, string arguments = null!)
    {
        var process = new Process();

        process.StartInfo.FileName = filename;
        
        if (!string.IsNullOrEmpty(arguments))
        {
            process.StartInfo.Arguments = arguments;
        }

        process.StartInfo.CreateNoWindow = true; // Hide the window
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Ensure it's hidden
        process.StartInfo.UseShellExecute = false; // Required for redirection

        // Redirect both standard output and standard error
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;

        // Use a StringBuilder to capture output line by line
        var stdOutput = new StringBuilder();
        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                stdOutput.AppendLine(args.Data); // Append each line of output
            }
        };

        string stdError = null!;
        try
        {
            process.Start(); // Start the process
            process.BeginOutputReadLine(); // Begin asynchronous reading of output
            stdError = process.StandardError.ReadToEnd(); // Read all standard error
            process.WaitForExit(); // Wait for the process to complete
        }
        catch (Exception e)
        {
            // Handle exceptions during process execution
            throw new Exception($"OS error while executing '{FormatCommand(filename, arguments)}': {e.Message}", e);
        }

        if (process.ExitCode == 0)
        {
            return Regex.Replace(stdOutput.ToString(), "(?<!\r)\n", "\r\n"); // return output
        }
        else
        {
            // If exit code is non-zero, indicate an error
            var errorMessage = new StringBuilder();
            if (!string.IsNullOrEmpty(stdError))
            {
                errorMessage.AppendLine(stdError);
            }
            if (stdOutput.Length > 0)
            {
                errorMessage.AppendLine("Standard Output:\n" + stdOutput.ToString());
            }
            
            throw new Exception($"{FormatCommand(filename, arguments)} finished with exit code {process.ExitCode}: {errorMessage}");
        }
    }

    // Helper to format command for display
    private static string FormatCommand(string filename, string arguments)
    {
        return $"'{filename}{(string.IsNullOrEmpty(arguments) ? "" : " " + arguments)}'";
    }
}