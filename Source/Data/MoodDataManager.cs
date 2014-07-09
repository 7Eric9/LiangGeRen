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
			}
			return _provider.IsValid;
		}

		public void Update() 
		{
			var updatedMessages = _provider.GetMessageItems(_util.Request(MoodURL));
			_provider.MessageItems.Clear();
			_provider.MessageItems.AddRange(updatedMessages);
		}

		public void GetMoreMessage()
		{
			string actualURL = MoodURL + "?page=" + _startPage.ToString();
			var moreMessages = _provider.GetMessageItems(_util.Request(actualURL));
			_provider.MessageItems.AddRange(moreMessages);
		}

	}
}
