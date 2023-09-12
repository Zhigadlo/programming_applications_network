using System.Net;
using System.Net.Sockets;
using System.Text;

using var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    await tcpClient.ConnectAsync(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888);
    while (true)
    {
        string expression = GetExpression();

        byte[] data = Encoding.UTF8.GetBytes(expression);
        // отправляем данные
        await tcpClient.SendAsync(data);
        Thread.Sleep(1000);
        double x = GetDouble();

        await tcpClient.SendAsync(Encoding.UTF8.GetBytes(x.ToString()));
        Console.WriteLine("Message was sent. Waiting for answer...");
        // буфер для считывания одного байта
        var bytesRead = new byte[512];

        tcpClient.Receive(bytesRead);

        Console.WriteLine(Encoding.UTF8.GetString(bytesRead));
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

string GetExpression()
{
    //string expression = String.Empty;
    //while (true)
    //{
    //    Console.Write("Enter mathematical expression: ");
    //    expression = Console.ReadLine();

    //    if (expression != String.Empty)
    //        break;
    //}
    //return expression;
    return "x^2";
}

double GetDouble()
{
    return 9.1;
}
