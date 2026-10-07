using netChat.shared.data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Sockets;

using utils.debug;

namespace netChat.client;

public class Client{
	private string ip = "127.0.0.1";
	private int port = 4444;
	private TcpClient client;

	public void Start(){
		Debug.Info("Client has started");
		Debug.Info("Atemppting to connect to server");
		client = new TcpClient();
		IPAddress address = IPAddress.Parse(ip);
		client.Connect(address, port);
		Debug.Info($"Client connected to server with {address}:{port}");

		SendMessage();
	}

	public void SendMessage(){
		int userID = 1;
		string msg = "Hello World!";

		Message message = new Message(userID, msg);
		string jsonData = JsonSerializer.Serialize(message);

		byte[] outData = Encoding.Unicode.GetBytes(jsonData);
		client.GetStream().Write(outData, 0, outData.Length);
	}


}
