using System.Net.Sockets;

public class Client
{
    private readonly TcpClient _tcpClient;
    private readonly int _id;
    private StreamWriter _streamWriter;
    private Server _server;
    
    public int Id => _id;
    public StreamWriter StreamWriter => _streamWriter;

    public Client(TcpClient tcpClient, int id, Server server)
    {
        _tcpClient = tcpClient;
        _id = id;
        _streamWriter = new StreamWriter(_tcpClient.GetStream());
        _streamWriter.WriteLine(_id);
        _streamWriter.Flush();
        _server = server;
    }

    public async Task ProcessAsync()
    {
        StreamReader streamReader = new StreamReader(_tcpClient.GetStream());

        while (true)
        {
            if (streamReader.EndOfStream)
            {
                Console.WriteLine("StreamReader was closed");
                _server.Clients.Remove(this);
                break;
            }

            string? message = await streamReader.ReadLineAsync();
            message += ", " + _id;
            await _server.BroadcastMessageAsync(message, _id);
        }
    }
}