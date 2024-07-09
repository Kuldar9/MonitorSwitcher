
namespace AutoMonitorSwitcher.Components
{
    public class Installer
    {
        public static async Task Install()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ControlMyMonitor");
            string exeFileName = "ControlMyMonitor.exe";
            string exeFullPath = Path.Combine(folderPath, exeFileName);

            // Check if the directory exists
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    // Attempt to create the directory
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine("Directory created successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error creating directory: {e.Message}");
                    return; // Exit method if directory creation fails
                }
            }
            else
            {
                Console.WriteLine("Directory already exists.");
            }

            // Check if the executable file exists
            if (File.Exists(exeFullPath))
            {
                Console.WriteLine($"Executable file '{exeFileName}' found in '{folderPath}'.");
            }
            else
            {
                Console.WriteLine($"Executable file '{exeFileName}' not found in '{folderPath}'.");
                // Initiate download
                string downloadUrl = "https://www.nirsoft.net/utils/controlmymonitor.zip";
                string downloadFile = "controlmymonitor.zip";
                string downloadFileFullPath = Path.Combine(folderPath, downloadFile);

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        Console.WriteLine($"Downloading '{downloadFile}'...");
                        // Download file asynchronously
                        HttpResponseMessage response = await client.GetAsync(downloadUrl);
                        response.EnsureSuccessStatusCode(); // Throw on error code.

                        // Save downloaded file
                        using (FileStream fileStream = new FileStream(downloadFileFullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        Console.WriteLine("Download complete.");

                        // Extract downloaded ZIP file
                        Console.WriteLine("Extracting files...");
                        System.IO.Compression.ZipFile.ExtractToDirectory(downloadFileFullPath, folderPath);
                        Console.WriteLine("Extraction complete.");

                        // Optionally delete the downloaded ZIP file after extraction
                        File.Delete(downloadFileFullPath);

                        Console.WriteLine($"Executable file '{exeFileName}' downloaded and installed successfully.");


                        Console.WriteLine("Gathering monitor input information");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error downloading or installing: {e.Message}");
                    }
                }
            }
        }
    }
}