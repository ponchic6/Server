using System.Net.Sockets;
using System.Text.Json;
using TCPServerTest;

public class Client
{
    private readonly TcpClient _tcpClient;
    private readonly int _id;
    private StreamWriter _streamWriter;
    private Server _server;
    private Queue<string> _recievedMessages;

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

    public async Task RecievAsync()
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

            string message = streamReader.ReadLine();
            TransformProperties transformProperties = JsonSerializer.Deserialize<TransformProperties>(message);
            transformProperties.Id = _id;
            string sendMessage = JsonSerializer.Serialize(transformProperties);
            Console.WriteLine(sendMessage);
            await _server.BroadcastMessageAsync(sendMessage, _id);
        }
    }
}