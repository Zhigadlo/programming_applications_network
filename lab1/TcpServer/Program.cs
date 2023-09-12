using System.Net;
using System.Net.Sockets;
using System.Text;

using Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    tcpListener.Bind(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888));
    tcpListener.Listen();    // запускаем сервер
    Console.WriteLine("Сервер запущен. Ожидание подключений... ");

    while (true)
    {
        // получаем подключение в виде TcpClient
        var tcpClient = await tcpListener.AcceptAsync();

        // создаем новую задачу для обслуживания нового клиента
        Task.Run(async () => await ProcessClientAsync(tcpClient));
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

async Task ProcessClientAsync(Socket tcpClient)
{
    var bytesRead = new byte[512];

    await tcpClient.ReceiveAsync(bytesRead);
    Mathos.Parser.MathParser parser = new Mathos.Parser.MathParser();
    var expression = Encoding.UTF8.GetString(bytesRead);
    Console.WriteLine(expression);
    await tcpClient.ReceiveAsync(bytesRead);
    double x = Convert.ToDouble(Encoding.UTF8.GetString(bytesRead));
    Console.WriteLine(x);
    parser.LocalVariables.Add("x", x);
    double result = parser.Parse(expression);

    await tcpClient.SendAsync(Encoding.UTF8.GetBytes(result.ToString()));
}
