using sysDiag = System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using netChat.shared.data;
using utils.debug;

namespace netChat.client;

public class Client{
	private string ip = "178.218.153.104";
	private int port = 4444;
	private TcpClient client;
	private string userName;
	private int userID;
	private string token;

	public void Start(){
		Debug.Info("Client has started");
		Debug.Info("Atemppting to connect to server");
		client = new TcpClient();
		IPAddress address = IPAddress.Parse(ip);
		client.Connect(address, port);
		Debug.Info($"Client connected to server with {address}:{port}");

		userName = getString("Enter username: ");
		Console.WriteLine(userName);
		userID = getInt("Enter UserID: ");
		Console.WriteLine(userID);

		Debug.MinLevel = LogLevel.Trace;
		while (true){
			CollectDataFromUser();
			getMessages();
		}
	}

	public void CollectDataFromUser(){

		sysDiag.Stopwatch timer = sysDiag.Stopwatch.StartNew();
		int max = 50;

		Debug.Trace("Collect: Collect loop");
		while ( timer.ElapsedMilliseconds < max){
			if ( !Console.KeyAvailable ){
				Debug.Trace("key is not avaible");
				continue;
			}
			ConsoleKey key = Console.ReadKey(intercept: true).Key;
			if(key == ConsoleKey.D1)
			{
				SendMessage();
			}
			Thread.Sleep(20);
		}
	}

	// Methods for geting data
	string getString(string question){
		string? foo = "";

		Console.Clear();
		Console.WriteLine(question);
		foo = Console.ReadLine();
		return foo;
	}

	float getFloat(string question){
		string? foo = "";
		float bar = 0f;


		while (true){
			Console.WriteLine(question);
			foo = Console.ReadLine();

			if (float.TryParse(foo, out bar))
			{
				return bar;
			}
			Console.WriteLine("Invalid input, you comemierda. Please enter a valid float value.");
			Console.WriteLine("Invalid input! Press any key to try again, you absolute comemierda..");
		}
	}

	int getInt(string question){
		string? foo = "";
		int bar = 0;
		while (true){
			Console.WriteLine(question);
			foo = Console.ReadLine();
			if (int.TryParse(foo, out bar))
			{
				return bar;
			}
			Console.WriteLine("Invalid input, you comemierda. Please enter a valid integer value.");
			Console.WriteLine("Invalid input! Press any key to try again, you absolute comemierda..");
			Console.ReadKey();
		}
	}

	public void SendMessage(){
		string msg = "Hello world";

		Message message = new Message(token, userID, msg);
		string jsonData = JsonSerializer.Serialize(message);

		byte[] outData = Encoding.Unicode.GetBytes(jsonData);
		client.GetStream().Write(outData, 0, outData.Length);
	}

	private void getMessages(){
		Debug.Trace("Listening for messages");
		byte[] buffer = new byte[1024];

		if ( client.Available == 0 ) return;
		Debug.Trace("getting stream");
		int length = client.GetStream().Read(buffer, 0, buffer.Length);
		Debug.Trace($"length: {length}");
		if ( length == 0 ) return;
		string jsonData = Encoding.Unicode.GetString(buffer, 0, length);
		Debug.Info($"JsonData: {jsonData}");
		// Debug.Trace($"jsonData: {jsonData}");
		Message message = JsonSerializer.Deserialize<Message>(jsonData);
		Debug.Trace($"message: {message}");
		Console.WriteLine(message.UserID + ": " + message.Msg);
	}
}
