using System.Net.Sockets;
using System.Net;
using netChat.shared.data;
using utils.debug;
using System.Text;
using System.Text.Json;

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

		Debug.Info($"Server started on {port}");

		Debug.MinLevel = LogLevel.Trace;
		while (true){
			ListenForClients();
			CheckClientsAlive();
			ListenForMessages();
			// Sleeep to not overwork.
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
		TcpClient client = listener.AcceptTcpClient();
		clients.Add(client);
		Debug.Info("Client found");
	}

	public void ListenForMessages()	{
		Debug.Trace("Listsen: Listening for messages");
		byte[] buffer = new byte[1024];

		foreach(TcpClient client in clients){
			if ( client.Available == 0 ){
				Debug.Trace("Listen: No data Available");
				continue;
			}

			Debug.Trace("Listen: Reading stream");
			int length = client.GetStream().Read(buffer, 0, buffer.Length);
			Debug.Trace($"Listen: Buffer length: {length}");
			if ( length == 0 ) continue;
			string jsonData = Encoding.Unicode.GetString(buffer, 0, length);
			Debug.Info($"Listen: JsonData: {jsonData}");
			Message message = JsonSerializer.Deserialize<Message>(jsonData);
			Debug.Trace($"Listen: Message: {message}");
			Console.WriteLine(message.UserID + ": " + message.Msg);
			SendMesagesToClients(message.UserID, message.Msg);
		}
	}
	public void SendMesagesToClients(int fromUserID, string msg){
		Debug.Trace("Send: Sends measage to clients");
		Message message = new Message("", fromUserID, msg);
		string jsonData = JsonSerializer.Serialize(message);
		Debug.Trace($"Send: jsonData: {jsonData}");

		byte[] outData = Encoding.Unicode.GetBytes(jsonData);

		foreach(TcpClient client in clients){
			client.GetStream().Write(outData, 0, outData.Length);
		}
		Debug.Trace("Send: Sent to evrey client");
	}
}
