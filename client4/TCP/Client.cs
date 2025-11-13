using System.Net.Sockets;
using System.Text;
using System.Net;

internal partial class Program
{
   
        public static async Task SimpleArithClient()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // local host
            IPEndPoint ipEndPoint = new(ipAddress, 11_000); // 11_000 = 11000

            using Socket client = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            await client.ConnectAsync(ipEndPoint);

            Console.WriteLine("Please enter 'quit' to or");
            Console.WriteLine("a mathematical expression possibly with the +,-,*,/ operators");

            while (true)
            {
                // Send message.
                string? message;
                byte[] messageBytes;
                // to prevent deadlock (i.e., sending ""), we do not send
                // an empty message
                do
                {
                    Console.Write(">>");
                    message = Console.ReadLine();
#pragma warning disable CS8604 // Possible null reference argument.
                messageBytes = Encoding.UTF8.GetBytes(message);
#pragma warning restore CS8604 // Possible null reference argument.
            } while (messageBytes.Length == 0);

                if (message.IndexOf("quit") >= 0)
                {
                    break;
                }


                _= await client.SendAsync(messageBytes, SocketFlags.None);
                Console.WriteLine($"Socket client sent message: \"{message}\"");

                //Receive ack.
                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                var i = response.IndexOf("<ACK>");
                response = response.Substring(0, i);
                Console.WriteLine(response);

            }
            client.Shutdown(SocketShutdown.Both);
        }
    }
