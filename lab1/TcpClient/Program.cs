using System.Net;
using System.Net.Sockets;
using System.Text;

using var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    await tcpClient.ConnectAsync(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888);

    string expression = GetExpression("Enter mathematical expression: ");
    byte[] data = Encoding.UTF8.GetBytes(expression);
    await tcpClient.SendAsync(data);

    double xStart = GetDoubleValue("Enter x start(numeric value): ");
    await tcpClient.SendAsync(Encoding.UTF8.GetBytes(xStart.ToString()));

    double learningRate = GetDoubleValue("Enter learning rate(double value): ");
    await tcpClient.SendAsync(Encoding.UTF8.GetBytes(learningRate.ToString()));

    int numIteration = GetIntValue("Enter number of iterations(int value): ");
    await tcpClient.SendAsync(Encoding.UTF8.GetBytes(numIteration.ToString()));

    Console.WriteLine("Message was sent. Waiting for answer...");

    var bytesRead = new byte[512];

    tcpClient.Receive(bytesRead);

    Console.WriteLine($"Server answer: {Encoding.UTF8.GetString(bytesRead)}");
    Console.WriteLine("Press any key...");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

string GetExpression(string text)
{
    string expression = string.Empty;
    while (true)
    {
        Console.Write(text);
        expression = Console.ReadLine();

        if (expression != string.Empty)
            break;

        Console.WriteLine("Invalid input. String must be not empty.");
        Console.WriteLine("Press any button to try again...");
        Console.ReadKey();
    }
    return expression;
}

double GetDoubleValue(string text)
{
    double number;
    while (true)
    {
        Console.Write(text);

        if (double.TryParse(Console.ReadLine(), out number))
        {
            break;
        }
        Console.WriteLine("Invalid input. You must to write double value.");
        Console.WriteLine("Press any button to try again...");
        Console.ReadKey();
    }
    return number;
}

int GetIntValue(string text)
{
    int number;
    while (true)
    {
        Console.Write(text);

        if (int.TryParse(Console.ReadLine(), out number))
        {
            break;
        }
        Console.WriteLine("Invalid input. You must to write int value.");
        Console.WriteLine("Press any button to try again...");
        Console.ReadKey();
    }
    return number;
}
