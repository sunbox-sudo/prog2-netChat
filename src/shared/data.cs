using System.Net.Mail;
using System.Text.Json.Serialization;

namespace netChat.shared.data;

public class User{
	MailAddress? eMail;
	int userID; // snowflake
	string? displayname;
	string? passwd;
	string? token;
	public string? username;
}

public class Message{
	[JsonInclude]
	public string Token;
	[JsonInclude]
	public int UserID;
	[JsonInclude]
	public string Msg;
	public DateTime time;

	public Message(string token, int userID, string msg){
		this.Token = token;
		this.UserID = userID;
		this.Msg = msg;
		this.time = DateTime.UtcNow;
	}
}
