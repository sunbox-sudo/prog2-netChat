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
	private string userName;

	public void Start(){
		Debug.Info("Client has started");
		Debug.Info("Atemppting to connect to server");
		client = new TcpClient();
		IPAddress address = IPAddress.Parse(ip);
		client.Connect(address, port);
		Debug.Info($"Client connected to server with {address}:{port}");

		Console.Write("enter a username: ");
		userName = Console.ReadLine();

		while (true){
			CollectDataFromUser();
		}
	}

	public void CollectDataFromUser(){
		ConsoleKey key = Console.ReadKey(intercept: true).Key;
		if(key == ConsoleKey.D1)
		{
			SendMessage();
		}
	}

	public void SendMessage(){
		int userID = 000001;
		string msg = "Hello world";

		Message message = new Message(userID, msg);
		string jsonData = JsonSerializer.Serialize(message);

		byte[] outData = Encoding.Unicode.GetBytes(jsonData);
		client.GetStream().Write(outData, 0, outData.Length);
	}


}
