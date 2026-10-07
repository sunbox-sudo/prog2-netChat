using System.Net.Sockets;
using System.Net;
using netChat.shared.data;
using utils.debug;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace netChat.server;

public class Server{
	private TcpListener listener;
	private List<TcpClient> clients;
	private int port = 4444;

	public void Start(){
		Debug.Info("Server starting");

		clients = new List<TcpClient>();
		listener = new TcpListener(IPAddress.Any, port);
		listener.Start();

		ListenForClients();
		ListenForMessages();
	}

	public void ListenForClients(){
		Debug.Info("Listening for clients");
		TcpClient client = listener.AcceptTcpClient();
		clients.Add(client);
		Debug.Info("CLient found");
	}
	public void ListenForMessages()
        {
            Console.WriteLine("Listening for messages...");
            byte[] buffer = new byte[1024];
            foreach(TcpClient client in clients)
            {
                int length = client.GetStream().Read(buffer, 0, buffer.Length);
                string jsonData = Encoding.Unicode.GetString(buffer, 0, length);
                Message message = JsonSerializer.Deserialize<Message>(jsonData);
                Console.WriteLine(message.UserID + ": " + message.Msg);
            }
        }

}
