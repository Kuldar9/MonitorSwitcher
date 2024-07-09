using System.Diagnostics;

namespace AutoMonitorSwitcher.Components
{
    public class MonitorInfoExtractor
    {
        public static async Task Monitors()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ControlMyMonitor");
            string exeFileName = "ControlMyMonitor.exe";
            string exeFullPath = Path.Combine(folderPath, exeFileName);

            if (!File.Exists(exeFullPath))
            {
                Console.WriteLine($"Executable file '{exeFileName}' not found in '{folderPath}'.");
                return;
            }

            try
            {
                // Define the output file path for the command
                string outputFilePath = Path.Combine(folderPath, "monitors.txt");

                // Create the command argument
                string arguments = $"/smonitors \"{outputFilePath}\"";  // Ensure the path is quoted

                // Set up the process start info
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exeFullPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    // Start the process
                    process.Start();

                    // Asynchronously read the standard output and error
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    // Wait for the process to exit
                    await process.WaitForExitAsync();

                    // Log outputs
                    if (!string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("Output:");
                        Console.WriteLine(output);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(error);
                    }

                    // Check the output file for the results
                    if (File.Exists(outputFilePath))
                    {
                        string result = await File.ReadAllTextAsync(outputFilePath);
                        Console.WriteLine($"Monitor information saved to '{outputFilePath}':");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create the monitors.txt file.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error executing ControlMyMonitor: {e.Message}");
            }
        }
    }

    // Extension method to await process completion
    public static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process)
        {
            if (process.HasExited)
            {
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<object?>();

            void ProcessExited(object? sender, EventArgs e)
            {
                process.Exited -= ProcessExited;
                tcs.TrySetResult(null);
            }

            process.Exited += ProcessExited;
            return tcs.Task;
        }
    }
}