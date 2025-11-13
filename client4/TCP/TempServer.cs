using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data; 

internal class TempServer
{
    public static async Task Run()
    {
        IPAddress ipAddress = IPAddress.Any;
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);

        using Socket listener = new(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
        listener.Listen(100);

        Console.WriteLine(" Temperature/Arithmetic Server started... waiting for connection...");
        var handler = await listener.AcceptAsync();
        
        
        DataTable dataTable = new DataTable();

        while (true)
        {
            var buffer = new byte[1024];
            var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            var message = Encoding.UTF8.GetString(buffer, 0, received).Trim();

            if (message.ToLower() == "quit")
            {
                Console.WriteLine("Server stopped.");
                break;
            }

            string reply;
            
            // ตรวจสอบว่าข้อความเป็นคำสั่งอุณหภูมิหรือไม่
            if (message.StartsWith("C ") || message.StartsWith("F ") || 
                message.StartsWith("c ") || message.StartsWith("f "))
            {
                // ⚙️ Logic การแปลงอุณหภูมิ (โค้ดเดิม)
                try
                {
                    var parts = message.Split(' ');
                    if (parts.Length != 2)
                        reply = "Invalid input. Use: C 100 or F 212";
                    else
                    {
                        char mode = char.ToUpper(parts[0][0]);
                        double value = double.Parse(parts[1]);
                        double result = 0;

                        if (mode == 'C')
                            result = value * 9 / 5 + 32;
                        else if (mode == 'F')
                            result = (value - 32) * 5 / 9;
                        else
                            throw new Exception(); // จะไม่ถึงตรงนี้ถ้า parts[0] ถูกต้อง

                        reply = $"Result: {result:F2} ({(mode == 'C' ? "°F" : "°C")})";
                    }
                }
                catch
                {
                    reply = "Error: Invalid input format for temperature.";
                }
            }
            else // ➕ Logic การคำนวณคณิตศาสตร์ (โค้ดใหม่ที่เพิ่มเข้ามา)
            {
                try
                {
                    // ใช้ DataTable.Compute ในการประมวลผลสมการคณิตศาสตร์
                    object result = dataTable.Compute(message, null);
                    reply = $"Arithmetic Result: {result}";
                }
                catch
                {
                    reply = "Error: Unsupported mathematical expression or command.";
                }
            }
            
            reply += "<ACK>";
            var replyBytes = Encoding.UTF8.GetBytes(reply);
            await handler.SendAsync(replyBytes, SocketFlags.None);
            Console.WriteLine($"  Sent: {reply}");
        }

        handler.Shutdown(SocketShutdown.Both);
    }
}
