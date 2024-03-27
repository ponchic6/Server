using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace TCPServerTest;

public class Server
{
    private TcpListener _tcpListener = new TcpListener(IPAddress.Loopback, 5555);
    private List<Client> _clients = new List<Client>();
    private Queue<TransformProperties> _recievedMessage = new Queue<TransformProperties>();
    

    public List<Client> Clients => _clients;
    public Queue<TransformProperties> RecievedMessage => _recievedMessage;

    public async Task ListenAsync()
    {   
        int id = 1;
        _tcpListener.Start();
        Console.WriteLine("Server started...");
        Task.Run(StartAnswer);

        while (true)
        {
            TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
            Client client = new Client(tcpClient, id, this);
            _clients.Add(client);
            Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} was connected");
            Task.Run(client.RecievAsync);
            id++;
        }
    }

    private async void StartAnswer()
    {
        while (true)
        {
            while (_recievedMessage.Count != 0)
            {
                TransformProperties transformProperties = _recievedMessage.Dequeue();
                string messageString = JsonSerializer.Serialize(transformProperties);

                foreach (Client client in _clients)
                {
                    if (transformProperties.Id != client.Id)
                    {   
                        await client.StreamWriter.WriteLineAsync(messageString);
                    }
                }
            }
        }
    }
}