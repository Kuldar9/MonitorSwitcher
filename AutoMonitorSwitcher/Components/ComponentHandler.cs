
namespace AutoMonitorSwitcher.Components
{
    public class ComponentHandler
    {
        public static async Task Handling(string[] args)
        {
            try
            {
                Console.WriteLine("Performing install:");
                await Installer.Install();

                // Assuming MonitorInfoExtractor and MonitorInfoExtractor2 are invoked correctly
                await MonitorInfoExtractor.Monitors();
                await MonitorInfoExtractor2.TxtFileCreation();

                // Example usage: Fetch Current Value of VCP Code 60 for monitor ID "MSI3CA9"
                string shortMonitorId = "MSI3CA9";
                string targetCode = "60";
                string targetPart = "CurrentValue";
                await Reader.Reading(shortMonitorId, targetCode, targetPart);

                if (args.Length == 0)
                {
                    Console.WriteLine("Please specify 'client' or 'server' as an argument.");
                    return;
                }

                string mode = args[0].ToLower();
                if (mode == "server")
                {
                    Console.WriteLine("Starting server...");
                    Server.StartServer(args);
                }
                else if (mode == "client")
                {
                    Console.WriteLine("Starting client...");
                    Client.StartClient(args);
                }
                else
                {
                    Console.WriteLine("Invalid argument. Please specify 'client' or 'server'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in handling components: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}