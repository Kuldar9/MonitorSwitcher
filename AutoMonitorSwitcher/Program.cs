
namespace AutoMonitorSwitcher
{
    class Program
    {
        static async Task Main(string[] args)
        {   
           await Components.ComponentHandler.Handling(args);
        }
    }
}