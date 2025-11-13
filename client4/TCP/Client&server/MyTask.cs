using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
internal class MyTasks
{

    // public static void Task02()
    // {
    //     string? expression;

    //     Console.WriteLine("Please enter 'quit' to quit or");
    //     Console.WriteLine("a mathematical expression possibly with the +, -, *, / operators");
    //     Console.Write(">> ");
    //     expression = Console.ReadLine();

    //     DataTable d = new DataTable();
    //     object x;
    //     x = d.Compute("1*10+2/5", null);
    //     Console.WriteLine(x);

    //     while (expression != null && !expression.Equals("quit"))
    //     {
    //         try
    //         {
    //             x = d.Compute(expression, null);
    //             Console.WriteLine(x);
    //         }
    //         catch
    //         {
    //             Console.WriteLine("Unsupported expression");
    //         }
    //         Console.Write(">> ");
    //         expression = Console.ReadLine();
    //     }
    // }
    /// <summary>
    /// Sends a message to and wait for an acknowledgement message from a server,
    /// located at the local host at port 11000, over a TCP/IP connection
    /// </summary>
    /// <returns>a task </returns>

    public static async Task SimpleClient()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // local host
        IPEndPoint ipEndPoint = new(ipAddress, 11_000); // 11_000 = 11000
        using Socket client = new(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        await client.ConnectAsync(ipEndPoint);
        while (true)
        {
            // Send message.
            var message = "Morning 👋!<|EOM|>";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"Socket client sent message: \"{message}\"");

            // Receive ack.
            var buffer = new byte[1_024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            if (response == "<|ACK|>")
            {
                Console.WriteLine(
                    $"Socket client received acknowledgment: \"{response}\"");
                break;
            }
            // Sample output:
            //     Socket client sent message: "Hi friends 👋!<|EOM|>"
            //     Socket client received acknowledgment: "<|ACK|>"
        }

        client.Shutdown(SocketShutdown.Both);
    }
    public static async Task SimpleServer()
    {
        // IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("host.contoso.com");
        // IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEndPoint = new(ipAddress, 11_000); // 11_000 = 11000 port means basic HS

        using Socket listener = new(
        ipEndPoint.AddressFamily,
        SocketType.Stream,
        ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
        listener.Listen(100);

        var handler = await listener.AcceptAsync();
        while (true)
        {
            // Receive message.
            var buffer = new byte[1_024];
            var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            var eom = "<|EOM|>";
            if (response.IndexOf(eom) > -1 /* is end of message */)
            {
                Console.WriteLine(
                $"Socket server received message: \"{response.Replace(eom, "")}\"");

                var ackMessage = "<|ACK|>";
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, 0);
                Console.WriteLine(
                    $"Socket server sent acknowledgment: \"{ackMessage}\"");

                break;
            }
            // Sample output:
            //    Socket server received message: "Hi friends 👋!"
            //    Socket server sent acknowledgment: "<|ACK|>"
        }
    }

 private static void Main3(string[] args)
  {

    // with async type we can run both in parallel despite it being run on separate line
    var taskserver = MyTasks.SimpleServer();
    var taskclient = MyTasks.SimpleClient();

    taskclient.Wait(); // wait for client to finish executing
    taskserver.Wait(); 

  }
}
