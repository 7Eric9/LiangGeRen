using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiangGeRen.Extension;

namespace LiangGeRen.Data
{
	internal class MoodDataManager
	{
		const string MoodURL = "http://old.liageren.com/couples/307268/says";

		private MessageItemsProvider _provider;
		private HttpDataUtil _util;
		int _startPage = 2;
		public MoodDataManager() 
		{
			_util = new HttpDataUtil();
			_provider = new MessageItemsProvider();
		}

		internal HttpDataUtil HttpUtil
		{
			get { return _util; }
		}

		internal ObservableCollection<MessageItem> MessageItems
		{
			get
			{
				return _provider.MessageItems;
			}
		}

		public void InitailRequest()
		{
			_util.InitailRequest();
		}

		public bool Login(string userName, string passWord) 
		{
			var messages = _provider.GetMessageItems(_util.Login(userName, passWord));
			if (_provider.IsValid)
			{
				_provider.MessageItems.AddRange(messages);
				ProcessReplyMessage(_provider.MessageItems);
			}
			return _provider.IsValid;
		}

		public void Update() 
		{
			var updatedMessages = _provider.GetMessageItems(_util.Request(MoodURL));
			_provider.MessageItems.Clear();
			_provider.MessageItems.AddRange(updatedMessages);
			_startPage = 2;
		}

		public void GetMoreMessage()
		{
			string actualURL = MoodURL + "?page=" + _startPage.ToString();
			var moreMessages = _provider.GetMessageItems(_util.Request(actualURL));
			ProcessReplyMessage(moreMessages);
			_provider.MessageItems.AddRange(moreMessages);
			_startPage++;
		}

		public void ProcessReplyMessage(IList<MessageItem> messgeItems)
		{
			foreach (var messageItem in messgeItems)
			{
				var stream = _util.Request(messageItem.ReplyMessageURL); 
				messageItem.ReplyMessages = _provider.PocessReplyMessages(stream);
			}
		}

		public void PostMessage()
		{
			List<string> parameters 
				= new List<string>() { "public", "说", "couple", "该消息来自客户端！" };
			_util.PostMessage(parameters);
		}

		public void PostReply(MessageItem messageItem, string content)
		{
			const string prefixUrl = "http://old.liageren.com/say/";
			const string pieceUrl = "/comments/create_say_comment?feed_id=";
			var keys = messageItem.ReplyMessageKey.Split(',');
			string url = prefixUrl + keys[0] + pieceUrl + keys[1];
			List<string> ms = new List<string>(){content, _provider.CoupleID};
			_util.PostReply(url, ms);
		}

	}
}
