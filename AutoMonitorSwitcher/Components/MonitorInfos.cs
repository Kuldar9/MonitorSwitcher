
using System.Text.RegularExpressions;

namespace AutoMonitorSwitcher.Components
{
    public class MonitorInfos
    {
        // Define a static field to cache monitor information
        private static List<MonitorInfo> cachedMonitorInfos = new List<MonitorInfo>();

        public class MonitorInfo
        {
            public string MonitorDeviceName { get; set; } = string.Empty;
            public string MonitorName { get; set; } = string.Empty;
            public string SerialNumber { get; set; } = string.Empty;
            public string AdapterName { get; set; } = string.Empty;
            public string MonitorID { get; set; } = string.Empty;
            public string ShortMonitorID { get; set; } = string.Empty;
        }

        // Method to read monitor info from file and cache it
        public static async Task<List<MonitorInfo>> ReadMonitorInfoAsync(string filePath)
        {
            var monitorInfos = new List<MonitorInfo>();

            if (cachedMonitorInfos.Count > 0)
            {
                Console.WriteLine("Returning cached monitor information.");
                return cachedMonitorInfos;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File '{filePath}' not found.");
                return monitorInfos;
            }

            try
            {
                var fileContent = await File.ReadAllTextAsync(filePath);
                var monitors = Regex.Split(fileContent, @"(?=Monitor Device Name:)");

                foreach (var monitor in monitors)
                {
                    if (string.IsNullOrWhiteSpace(monitor)) continue;

                    var monitorInfo = new MonitorInfo
                    {
                        MonitorDeviceName = ExtractValue(monitor, @"Monitor Device Name: ""([^""]+)"""),
                        MonitorName = ExtractValue(monitor, @"Monitor Name: ""([^""]+)"""),
                        SerialNumber = ExtractValue(monitor, @"Serial Number: ""([^""]*)"""),
                        AdapterName = ExtractValue(monitor, @"Adapter Name: ""([^""]+)"""),
                        MonitorID = ExtractValue(monitor, @"Monitor ID: ""([^""]+)"""),
                        ShortMonitorID = ExtractValue(monitor, @"Short Monitor ID: ""([^""]+)""")
                    };

                    monitorInfos.Add(monitorInfo);
                }

                // Cache the monitor information
                cachedMonitorInfos = monitorInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading or parsing file: {e.Message}");
            }

            return monitorInfos;
        }

        private static string ExtractValue(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        // Method to get monitor info by Monitor Device Name
        public static MonitorInfo? GetMonitorInfoByDeviceName(string monitorDeviceName)
        {
            return cachedMonitorInfos.FirstOrDefault(m => m.MonitorDeviceName.Equals(monitorDeviceName, StringComparison.OrdinalIgnoreCase));
        }

        // Method to get all cached monitor info
        public static List<MonitorInfo> GetAllMonitorInfos()
        {
            return cachedMonitorInfos;
        }
    }
}