using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using TcpServer;

using Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    tcpListener.Bind(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888));
    tcpListener.Listen();
    Console.WriteLine("Server is running. Waiting for connection... ");

    while (true)
    {
        var tcpClient = await tcpListener.AcceptAsync();

        Task.Run(async () => await ProcessClientAsync(tcpClient));
        
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

async Task ProcessClientAsync(Socket tcpClient)
{
    Console.WriteLine("Server is waiting for client messages");

    var bytesRead = new byte[64];

    await tcpClient.ReceiveAsync(bytesRead);
    var expression = Encoding.UTF8.GetString(bytesRead);
    Console.WriteLine($"expression: {expression}");
    bytesRead = new byte[64];

    await tcpClient.ReceiveAsync(bytesRead);
    double xStart = double.Parse(Encoding.UTF8.GetString(bytesRead));
    Console.WriteLine($"xStart: {xStart}");
    bytesRead = new byte[64];

    await tcpClient.ReceiveAsync(bytesRead);
    double learningRate = double.Parse(Encoding.UTF8.GetString(bytesRead));
    Console.WriteLine($"learningRate: {learningRate}");
    bytesRead = new byte[64];

    await tcpClient.ReceiveAsync(bytesRead);
    int numIteration = int.Parse(Encoding.UTF8.GetString(bytesRead));
    Console.WriteLine($"numOfIteration: {numIteration}");

    GradientDescentSolver solver = new GradientDescentSolver(expression);
    solver.SetxStart(xStart);
    solver.SetLearningRate(learningRate);
    solver.SetNumberOfIteration(numIteration);

    double result = solver.Solve();

    await tcpClient.SendAsync(Encoding.UTF8.GetBytes(result.ToString()));
}