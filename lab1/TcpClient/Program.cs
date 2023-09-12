using System.Net;
using System.Net.Sockets;
using System.Text;

using var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    await tcpClient.ConnectAsync(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888);

    byte[] data = Encoding.UTF8.GetBytes("Сосново шишечко, навишчо ты пьешь?");
    // отправляем данные
    await tcpClient.SendAsync(data);
    // буфер для считывания одного байта
    var bytesRead = new byte[512];
    
    tcpClient.Receive(bytesRead);

    Console.WriteLine(Encoding.UTF8.GetString(bytesRead));
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.ReadKey();
