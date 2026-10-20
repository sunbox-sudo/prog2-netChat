using utils.debug;

namespace netChat.client;

class Program
{
	static void Main(string[] args){

		// TODO: procces args & flags

		// Debug
		debugInit();

		// Start Client
		Client client = new();
		client.Start();
	}

	static private void debugInit(){
		// File
		Debug.LogFileName = "client.log";
		Debug.LogFileDir = Path.Combine(Path.GetTempPath(), "netChat");
		// General
		Debug.MinLevel = LogLevel.Info;

		Debug.Start(); //should be named run
	}
}
