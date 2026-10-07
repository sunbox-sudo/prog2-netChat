using netChat.server;
using netChat.client;
using utils.debug;

namespace netChat;

class Program
{
	static void Main(string[] args){
		Console.Clear();
		debugInit();

		// call last server / client take over
		selectType(args);
	}
	static private void debugInit(){
		// File
		Debug.LogFileName = "netChat.log";
		Debug.LogFileDir = Path.Combine(Path.GetTempPath(), "netChat");
		// General
		Debug.MinLevel = LogLevel.Info;

		Debug.Start(); //should be named run
	}

	static private void selectType(string[] args){
		if (args.Length == 0){
			Client client = new();
			client.Start();
			Environment.Exit(0);
		}

		// Procecs arguments.

		Server server = new();
		server.Start();
	}
}
