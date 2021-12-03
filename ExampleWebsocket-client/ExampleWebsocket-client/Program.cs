using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExampleWebsocket_client
{
    class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        static void StartClient()
        {
            try
            {
                // Establish remote endpoint for connection
                IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddress = iPHost.AddressList[0];
                IPEndPoint endpointLocal = new IPEndPoint(iPAddress, 42069);

                // Create a TCP socket
                Socket sendSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // CONNECT
                    sendSocket.Connect(endpointLocal);

                    // Output socket information
                    Console.WriteLine("Socket Created: {0} ", sendSocket.RemoteEndPoint.ToString());

                    // Create message to send and send it
                    byte[] messageSent = Encoding.ASCII.GetBytes("Test Client <EOF>");
                    int byteSent = sendSocket.Send(messageSent);

                    // Data buffer
                    byte[] messageReceived = new byte[1024];

                    // Check for received messages
                    int byteReceived = sendSocket.Receive(messageReceived);
                    Console.WriteLine("Message Received: {0} ", Encoding.ASCII.GetString(messageReceived, 0, byteReceived));

                    // Close sockets
                    sendSocket.Shutdown(SocketShutdown.Both);
                    sendSocket.Close();
                }
                catch (ArgumentNullException execp)
                {
                    Console.WriteLine("Arguement Null Exeception: {0}", execp.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("Socket Exception: {0}", se.ToString());
                }
                catch (Exception generic)
                {
                    Console.WriteLine("Generic Exception: {0}", generic.ToString());
                }

            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: {0}", exp.ToString());
            }
        }
    }
}
