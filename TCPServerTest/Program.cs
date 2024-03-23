using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    private static Dictionary<StreamWriter, int> _streamWriters = new Dictionary<StreamWriter, int>();
    private static List<Client> _clients = new List<Client>();
    private static Server _server;

    public static async Task Main(string[] args)
    {
        _server = new Server();
        await _server.ListenAsync();
    }
}