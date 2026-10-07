using System.Net.Mail;

namespace netChat.shared.data;

public class User{
	MailAddress? eMail;
	int userID; // snowflake
	string? displayname;
	string? passwd;
	string? token;
	string? username;
}

public class Message{
	int UserID;
	string Msg;
	public DateTime time;

	public Message(int userID, string msg){
		this.UserID = userID;
		this.Msg = msg;
		this.time = DateTime.UtcNow;
	}
}
