﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiangGeRen.Data
{
	internal class MessageItem
	{
		public MessageItem() { }

		public MessageItem(string name, string message, string replyMessage, string time)
		{
			_name = name;
			_message = message;
			_replyMessage = replyMessage;
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

		private string _replyMessage;
		public string ReplyMessage
		{ get { return _replyMessage; } }

		private string _time;
		public string Time
		{
			get { return _time; }
		}
	}
}
