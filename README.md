📌 Asynchronous TCP/IP Arithmetic Client–Server Application

This project is a .NET asynchronous TCP client–server application capable of performing temperature conversion and evaluating arithmetic expressions in real time. The server handles all incoming commands and responds with computed results, while the client sends user input asynchronously using a lightweight custom protocol.

🚀 Features

Asynchronous TCP/IP communication

Temperature conversion (Celsius ↔ Fahrenheit)

Dynamic arithmetic expression evaluation using DataTable.Compute()

Custom ACK-based message protocol

Clean separation between Client, Server, and Program logic

🧠 How It Works                                                                  
1.) Server (TempServer.cs)     
The server listens on port 11000, accepts client connections, and interprets incoming commands.
Commands starting with C or F are processed as temperature conversion operations.
Any other input is evaluated as a mathematical expression.
```csharp
using Socket listener = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);
listener.Bind(ipEndPoint);
listener.Listen(100);
```
2.) Client (TempClient.cs)

The client connects to the server and sends commands entered by the user. It receives responses and strips the <ACK> tag before displaying output.
```csharp
await client.ConnectAsync(ipEndPoint);
await client.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);
```
3.) Program Entry (Program_Temp.cs)

Starts the server or client depending on configuration.

🧪 Example Usage
```csharp
Temperature
Input:  C 100
Output: Result: 212 °F
```
Arithmetic
```csharp
Input: 10*5-3
Output: Arithmetic Result: 47
```
4.) This represents the operation used to transmit data (Client–Server) between two computers.

- The computer who is Client need to comment out of the server first to stop the server working on that device.
```csharp
        //var taskServer = TempServer.Run();
        var taskClient = TempClient.Run();

        taskClient.Wait();
        //taskServer.Wait();
```
- And the client must be replace the default localhost address with the actual IP of the server machine:
```csharp
using System.Net;
using System.Net.Sockets;
using System.Text;
internal class TempClient
{
    public static async Task Run()
    {
        IPAddress ipAddress = IPAddress.Parse("10.39.xxx.xxx");
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);
```
- For part of the server the computer who is the server need to comment out of the client first to stop the client working on that device.
```csharp
 var taskServer = TempServer.Run();
 //var taskClient = TempClient.Run();

//taskClient.Wait();
taskServer.Wait();
```
- And then the server application must be executed before the client so it can open the listening socket on port 11000 and wait for incoming connections.
```csharp
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
```
Using ```IPAddress.Any``` to allows the server to accept connections from any network interface on the machine.

- And the last step is to run a program the server need to run first to creates a listening socket and bind to port 11000 to starts listening for the incoming connections and wait in an ```AcceptAsync()``` loop
```csharp
 listener.Bind(ipEndPoint);
        listener.Listen(100);

        Console.WriteLine(" Temperature/Arithmetic Server started... waiting for connection...");
        var handler = await listener.AcceptAsync();
```
- And then the client can run to connect to the server and begins sending thew commands.
🛠 Technologies Used

C# (.NET 9)

TCP Socket Programming

Async/Await

DataTable.Compute()
