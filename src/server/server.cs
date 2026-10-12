using System.Net.Sockets;
using System.Net;
using System.Net.Sockets;
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

		Debug.MinLevel = LogLevel.Trace;
		while (true){
			ListenForClients();
			CheckClientsAlive();
			ListenForMessages();
			// this so it don't max out the core.
			Thread.Sleep(50);
		}
	}

	public bool IsAlaive(TcpClient client){
		try {
			Socket s = client.Client;
			int ms = 1000;
			return !(s.Poll(ms, SelectMode.SelectRead) && s.Available == 0);
		}
		catch (SocketException) { return false; }
		catch (ObjectDisposedException) { return false; }
	}

	public void CheckClientsAlive(){
		for (int i = 0; i < clients.Count ; i++){
			try {
				TcpClient client = clients[i];
				if (IsAlaive(client)){
					Debug.Trace($"{client.Client.RemoteEndPoint} Is Alive");
					continue;
				}

				Debug.Info($"Client \"{client.Client.RemoteEndPoint}\" disconected");
				client.Close();
				clients.RemoveAt(i);
			}
			catch (SocketException) { continue; }
			catch (ObjectDisposedException) { continue; }
		}
	}

	public void ListenForClients(){
		if (!listener.Pending()){ return; }
		Debug.Info("Listening for clients");
		TcpClient client = listener.AcceptTcpClient(); // CAN not be found where to locate?
		clients.Add(client);
		Debug.Info("Client found");
	}
	public void ListenForMessages()	{
		Debug.Trace("Listening for messages...");
		byte[] buffer = new byte[1024];

		Debug.Trace("start of foreach");
		foreach(TcpClient client in clients)
		{
			if ( client.Available == 0 ){
				Debug.Trace("no data Available");
				continue;
			}

			Debug.Trace("getting stream");
			int length = client.GetStream().Read(buffer, 0, buffer.Length);
			Debug.Trace($"length: {length}");
			if ( length == 0 ) continue;
			string jsonData = Encoding.Unicode.GetString(buffer, 0, length);
			Debug.Trace($"jsonData: {jsonData}");
			Message message = JsonSerializer.Deserialize<Message>(jsonData);
			Debug.Trace($"message: {message}");
			Console.WriteLine(message.UserID + ": " + message.Msg);
		}
	}
}
