
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AutoMonitorSwitcher.Components
{
    public class Server
    {
        public static void StartServer(string[] args)
        {
            TcpListener? server = null; // Allow server to be nullable
            try
            {
                Int32 port = 13000;
                IPAddress? localAddr = GetLocalIPAddress(); // Allow localAddr to be nullable

                if (localAddr == null)
                {
                    Console.WriteLine("No network adapters with an IPv4 address in the system!");
                    return;
                }

                server = new TcpListener(localAddr, port);
                server.Start();

                Console.WriteLine($"Server started on {localAddr}:{port}. Waiting for a connection...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server?.Stop();
            }
        }

        static void HandleClient(object? obj) // Allow obj to be nullable
        {
            if (obj == null) return; // Handle null obj

            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] bytes = new byte[256];
            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes("Echo: " + data);
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", Encoding.ASCII.GetString(msg));
            }

            client.Close();
        }

        static IPAddress? GetLocalIPAddress() // Allow return type to be nullable
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }
    }
}