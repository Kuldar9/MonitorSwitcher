using System.Text.RegularExpressions;

namespace AutoMonitorSwitcher.Components;
class VcpCodeEntry
{
    public required string Code { get; set; }
    public string? CodeName { get; set; }
    public string? ReadWrite { get; set; }
    public string? CurrentValue { get; set; }
    public string? MaximumValue { get; set; }
    public string? PossibleValues { get; set; }

    public DateTime LastWriteTime { get; set; }
    public string GetValue(string part)
    {
        return part switch
        {
            "Code" => Code,
            "CodeName" => CodeName ?? string.Empty,
            "ReadWrite" => ReadWrite ?? string.Empty,
            "CurrentValue" => CurrentValue ?? string.Empty,
            "MaximumValue" => MaximumValue ?? string.Empty,
            "PossibleValues" => PossibleValues ?? string.Empty,
            _ => throw new ArgumentException("Invalid part name")
        };
    }
}

public class Reader
{
    private static Dictionary<string, (List<VcpCodeEntry>, DateTime)> cache = new Dictionary<string, (List<VcpCodeEntry>, DateTime)>();

    public static async Task Reading(string shortMonitorId, string targetCode, string targetPart)
    {
        string directoryPath = @"C:\Program Files\ControlMyMonitor\Monitors";
        string filePath = Path.Combine(directoryPath, $"{shortMonitorId}.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File {filePath} not found.");
            return;
        }

        DateTime lastWriteTime = File.GetLastWriteTime(filePath);
        List<VcpCodeEntry> vcpCodes;

        if (cache.ContainsKey(filePath) && cache[filePath].Item1.Count > 0)
        {
            // Check if cached data is up-to-date
            if (lastWriteTime == cache[filePath].Item2)
            {
                vcpCodes = cache[filePath].Item1;
                Console.WriteLine($"Using cached data for {shortMonitorId}.");
            }
            else
            {
                // Cache is outdated, reload from file
                Console.WriteLine($"Cache for {shortMonitorId} is outdated, reloading from file.");
                (vcpCodes, lastWriteTime) = await LoadVcpCodesFromFile(filePath);
            }
        }
        else
        {
            // No cache exists, load from file
            Console.WriteLine($"No cache found for {shortMonitorId}, loading from file.");
            (vcpCodes, lastWriteTime) = await LoadVcpCodesFromFile(filePath);
        }

        // Fetch and print Current Value of the specified VCP Code
        VcpCodeEntry? targetEntry = vcpCodes.Find(entry => entry.Code == targetCode);
        if (targetEntry != null)
        {
            string value = targetEntry.GetValue(targetPart);
            Console.WriteLine($"Code={targetCode}, {targetPart}={value}");
        }
        else
        {
            Console.WriteLine($"VCP Code {targetCode} not found.");
        }

        // Update cache with the latest data
        cache[filePath] = (vcpCodes, lastWriteTime);
    }

    private static async Task<(List<VcpCodeEntry>, DateTime)> LoadVcpCodesFromFile(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath);
        string fileContent = string.Join("\n", lines);

        // Regular expression to match each VCP code block
        string pattern = @"VCP Code\s*:\s*(?<Code>.+?)\s*\nVCP Code Name\s*:\s*(?<CodeName>.+?)\s*\nRead-Write\s*:\s*(?<ReadWrite>.+?)\s*\nCurrent Value\s*:\s*(?<CurrentValue>.*?)\s*(?=\n|Maximum Value\s*:\s*(?<MaximumValue>.*?)\s*(?=\n|Possible Values\s*:\s*(?<PossibleValues>.*?)\s*==|(?=\n\s*VCP Code)))";

        Regex regex = new Regex(pattern, RegexOptions.Singleline);
        MatchCollection matches = regex.Matches(fileContent);

        List<VcpCodeEntry> vcpCodes = new List<VcpCodeEntry>();

        foreach (Match match in matches)
        {
            VcpCodeEntry entry = new VcpCodeEntry
            {
                Code = match.Groups["Code"].Value.Trim(),
                CodeName = match.Groups["CodeName"].Value.Trim(),
                ReadWrite = match.Groups["ReadWrite"].Value.Trim(),
                CurrentValue = match.Groups["CurrentValue"].Value.Trim(),
                MaximumValue = match.Groups["MaximumValue"].Success ? match.Groups["MaximumValue"].Value.Trim() : string.Empty,
                PossibleValues = match.Groups["PossibleValues"].Success ? match.Groups["PossibleValues"].Value.Trim() : string.Empty
            };

            // Store last write time for cache validation
            entry.LastWriteTime = File.GetLastWriteTime(filePath);

            vcpCodes.Add(entry);
        }

        return (vcpCodes, File.GetLastWriteTime(filePath));
    }
}