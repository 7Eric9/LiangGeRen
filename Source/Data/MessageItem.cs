using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiangGeRen.Data
{
	internal class MessageItem
	{
		internal class ReplyMessage
		{
			public string UserName { get; set; }
			public string Content { get; set; }

			public ReplyMessage(string userName, string content)
			{
				UserName = userName;
				Content = content;
			}
		}

		public MessageItem() { }

		public MessageItem(string name, string message, 
			string replyMessageCount, string time, string replyMessageKey)
		{
			_name = name;
			_message = message;
			_replyMessageCount = replyMessageCount;
			_time = time;
			_replyMessageKey = replyMessageKey;
		}

		public MessageItem(string name, string message, string time)
		{
			_name = name;
			_message = message;
			_time = time;
		}

		private string _name;
		public string Name
		{
			get { return _name; }
		}

		private string _message;
		public string Message
		{
			get { return _message; }
		}

		private string _replyMessageCount;
		public string ReplyMessageCount
		{ get { return _replyMessageCount; } }

		private string _time;
		public string Time
		{
			get { return _time; }
		}

		public List<ReplyMessage> ReplyMessages
		{ get; set; }

		private string _replyMessageKey;
		public string ReplyMessageKey
		{
			get { return _replyMessageKey; }
		}
		const string prefixURL = "http://old.liageren.com/say/";
		const string urlPiece = "/comments?feed_id=";
		public string ReplyMessageURL
		{
			get 
			{
				var keys = _replyMessageKey.Split(',');
				return prefixURL + keys[0] + urlPiece + keys[1]; 
			} 
		}
	}
}
