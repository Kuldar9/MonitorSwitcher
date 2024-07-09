
using System.Diagnostics;

namespace AutoMonitorSwitcher.Components
{
    public class MonitorInfoExtractor2
    {
        public static async Task TxtFileCreation()
        {
            // Define the path to your monitors.txt file
            string monitorFilePath = "C:\\Program Files\\ControlMyMonitor\\monitors.txt";

            // Load monitor info and cache it
            await MonitorInfos.ReadMonitorInfoAsync(monitorFilePath);

            // Retrieve all cached monitor info
            var allMonitorInfos = MonitorInfos.GetAllMonitorInfos();

            // Path to ControlMyMonitor executable and JSON output directory
            string controlMyMonitorPath = "C:\\Program Files\\ControlMyMonitor\\ControlMyMonitor.exe";
            string outputDirectory = "C:\\Program Files\\ControlMyMonitor\\Monitors";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
                Console.WriteLine($"Directory created at {outputDirectory}");
            }
            else
            {
                Console.WriteLine($"Directory already exists at {outputDirectory}");
            }

            // Loop through each MonitorInfo
            foreach (var monitorInfo in allMonitorInfos)
            {
                // Construct the command line arguments with the full path for the TXT output file
                string txtFilePath = Path.Combine(outputDirectory, $"{monitorInfo.ShortMonitorID}.txt");
                string arguments = $"/stext \"{txtFilePath}\" \"{monitorInfo.MonitorDeviceName}\"";

                // Execute the ControlMyMonitor.exe process
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = controlMyMonitorPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try
                {
                    using (Process? process = Process.Start(processStartInfo)) // The '?' makes 'process' a nullable type
                    {
                        if (process == null)
                        {
                            Console.WriteLine($"Failed to start process for {monitorInfo.ShortMonitorID}");
                            continue;
                        }

                        // Read the output (if needed)
                        string output = await process.StandardOutput.ReadToEndAsync();
                        string error = await process.StandardError.ReadToEndAsync();

                        // Wait for the process to exit
                        await process.WaitForExitAsync();

                        // Check the exit code
                        if (process.ExitCode == 0)
                        {
                            Console.WriteLine($"Successfully created txt file for {monitorInfo.ShortMonitorID}");
                        }
                        else
                        {
                            Console.WriteLine($"Error executing command for {monitorInfo.ShortMonitorID}: {error}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception executing command for {monitorInfo.ShortMonitorID}: {ex.Message}");
                }
            }
        }
    }
}








/*

// Example: Accessing individual monitor info
string desiredDeviceName = "\\\\.\\DISPLAY7\\Monitor0"; // Ensure correct format and escaping
var monitorInfo = MonitorInfos.GetMonitorInfoByDeviceName(desiredDeviceName);

if (monitorInfo != null)
{
    Console.WriteLine($"Monitor Device Name: {monitorInfo.MonitorDeviceName}");
    Console.WriteLine($"Monitor Name: {monitorInfo.MonitorName}");
    Console.WriteLine($"Serial Number: {monitorInfo.SerialNumber}");
    Console.WriteLine($"Adapter Name: {monitorInfo.AdapterName}");
    Console.WriteLine($"Monitor ID: {monitorInfo.MonitorID}");
    Console.WriteLine($"Short Monitor ID: {monitorInfo.ShortMonitorID}");
}
else
{
    Console.WriteLine($"Monitor with Device Name '{desiredDeviceName}' not found.");
}


//  Example: Accessing all cached monitor info
var allMonitorInfos = MonitorInfos.GetAllMonitorInfos();
foreach (var info in allMonitorInfos)
{
    Console.WriteLine($"Monitor Device Name: {info.MonitorDeviceName}");
    Console.WriteLine($"Monitor Name: {info.MonitorName}");
    Console.WriteLine($"Serial Number: {info.SerialNumber}");
    Console.WriteLine($"Adapter Name: {info.AdapterName}");
    Console.WriteLine($"Monitor ID: {info.MonitorID}");
    Console.WriteLine($"Short Monitor ID: {info.ShortMonitorID}");
    Console.WriteLine();
}

*/