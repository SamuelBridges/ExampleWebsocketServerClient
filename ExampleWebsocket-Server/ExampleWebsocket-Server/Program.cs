using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExampleWebsocket_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            // Get name of host running the server
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            // Get IP of current host
            IPAddress ipAddress = ipHost.AddressList[0];

            // Create new end point with IP and Port
            IPEndPoint endPointLocal = new IPEndPoint(ipAddress, 42069);

            // Create a socket listener with a TCP Stream
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Bind the listener to the local end point
                listener.Bind(endPointLocal);

                // Listen to that end point with a maximum length of pending connections
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for connection");

                    // Suspend while waiting for a connection and use accept to accept a connection

                    Socket clientSocket = listener.Accept();

                    // Create data buffer
                    byte[] bytes = new byte[1024];
                    string data = null;

                    while (true) 
                    {
                        // Parse data to buffer
                        int numByte = clientSocket.Receive(bytes);

                        // Convert buffer into ascii
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        // If End of File tag, finish converting
                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    // Output text received
                    Console.WriteLine("Text Received: {0} ", data);

                    // Create response
                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    // Send response
                    clientSocket.Send(message);

                    // Close cliemt sockets
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            } catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }
    }
}
