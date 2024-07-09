
using System.Net.Sockets;
using System.Text;

namespace AutoMonitorSwitcher.Components
{
    public class Client
    {
        public static void StartClient(string[] args)
        {
            try
            {
                Int32 port = 13000;
                String server = "192.168.1.160"; // Use your IPv4 Address to connect

                if (args.Length > 1)
                {
                    server = args[1]; // Override with provided server address
                }

                TcpClient client = new TcpClient(server, port);

                NetworkStream stream = client.GetStream();

                Console.WriteLine("Enter message to send to the server:");
                string? message = Console.ReadLine(); // Allow message to be nullable

                if (string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Message cannot be null or empty.");
                    return;
                }

                byte[] data = Encoding.ASCII.GetBytes(message);

                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                data = new byte[256];
                string responseData = string.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.ReadLine();
        }
    }
}