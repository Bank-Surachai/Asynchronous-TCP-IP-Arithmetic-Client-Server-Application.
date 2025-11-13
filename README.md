📌 Asynchronous TCP/IP Arithmetic Client–Server Application

This project is a .NET asynchronous TCP client–server application capable of performing temperature conversion and evaluating arithmetic expressions in real time. The server handles all incoming commands and responds with computed results, while the client sends user input asynchronously using a lightweight custom protocol.

🚀 Features

Asynchronous TCP/IP communication

Temperature conversion (Celsius ↔ Fahrenheit)

Dynamic arithmetic expression evaluation using DataTable.Compute()

Custom ACK-based message protocol

Clean separation between Client, Server, and Program logic

🧠 How It Works
1️⃣ Server (TempServer.cs)

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
2️⃣ Client (TempClient.cs)

The client connects to the server and sends commands entered by the user. It receives responses and strips the <ACK> tag before displaying output.
```csharp
await client.ConnectAsync(ipEndPoint);
await client.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);
```
3️⃣ Program Entry (Program_Temp.cs)

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
🛠 Technologies Used

C# (.NET 9)

TCP Socket Programming

Async/Await

DataTable.Compute()
