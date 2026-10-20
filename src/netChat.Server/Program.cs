using utils.debug;

namespace netChat.server;

class Program
{
	static void Main(string[] args){

		// TODO: procces args & flags

		// Debug
		debugInit();

		// Server
		Server server = new();
		server.Start();
	}

	static private void debugInit(){
		// File
		Debug.LogFileName = "server.log";
		Debug.LogFileDir = Path.Combine(Path.GetTempPath(), "netChat");
		// General
		Debug.MinLevel = LogLevel.Info;

		Debug.Start(); //should be named run
	}
}
