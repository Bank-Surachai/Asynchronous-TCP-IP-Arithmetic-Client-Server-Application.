using System.Net;
using System.Net.Sockets;
using System.Text;

internal class TempClient
{
    public static async Task Run()
    {
        
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);

        using Socket client = new(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        await client.ConnectAsync(ipEndPoint);

        Console.WriteLine("Connected to Temperature/Arithmetic Server");
        //  คำแนะนำสำหรับอุณหภูมิ (เดิม)
        Console.WriteLine("Type 'C <value>' to convert Celsius → Fahrenheit");
        Console.WriteLine("Type 'F <value>' to convert Fahrenheit → Celsius");
        //  คำแนะนำสำหรับคณิตศาสตร์ (ใหม่)
        Console.WriteLine("Type a mathematical expression (e.g., 10*5+2)");
        Console.WriteLine("Type 'quit' to exit");

        while (true)
        {
            // ... (โค้ดรับ input และส่ง/รับข้อความเหมือนเดิม)
            Console.Write(">> ");
            string? message = Console.ReadLine();
            // ...
            
            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message.ToLower() == "quit")
            {
                await client.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);
                break;
            }

            await client.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);

            var buffer = new byte[1024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            var i = response.IndexOf("<ACK>");
            if (i > -1) response = response[..i];
            Console.WriteLine(response);
        }

        client.Shutdown(SocketShutdown.Both);
    }
}
