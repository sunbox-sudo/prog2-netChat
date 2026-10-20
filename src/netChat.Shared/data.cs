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
	[JsonInclude]
	public string UserName;
	[JsonInclude]
	public DateTime time;

	public Message(string token, int userID, string userName, string msg){
		this.Token = token;
		this.UserID = userID;
		this.UserName = userName;
		this.Msg = msg;
		this.time = DateTime.UtcNow;
	}
}
