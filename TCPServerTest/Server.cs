using System.Net;
using System.Net.Sockets;

public class Server
{
    private TcpListener _tcpListener = new TcpListener(IPAddress.Parse("192.168.1.121"), 5555);
    private List<Client> _clients = new List<Client>();

    public List<Client> Clients => _clients;

    public async Task ListenAsync()
    {   
        int id = 1;
        _tcpListener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
            Client client = new Client(tcpClient, id, this);
            _clients.Add(client);
            Console.WriteLine($"{tcpClient} was connected");
            Task.Run(client.ProcessAsync);
            id++;
        }
    }

    public async Task BroadcastMessageAsync(string message, int exceptId)
    {
        foreach (Client client in _clients)
        {
            if (client.Id != exceptId)
            {
                await client.StreamWriter.WriteLineAsync(message);
                await client.StreamWriter.FlushAsync();
            }
        }
    }
}