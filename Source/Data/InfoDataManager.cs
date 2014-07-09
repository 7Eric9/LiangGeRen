using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiangGeRen.Extension;

namespace LiangGeRen.Data
{
	class InfoDataManager
	{
		const string InfoURL = "http://old.liageren.com/feed_items";

		InfoItemsProvider _provider;
		HttpDataUtil _util;
		public InfoDataManager(HttpDataUtil HttpUtil)
		{
			_provider = new InfoItemsProvider();
			_util = HttpUtil;
		}

		internal ObservableCollection<MessageItem> MessageItems
		{
			get
			{
				return _provider.MessageItems;
			}
		}

		internal void UpdateInfo()
		{
			MessageItems.Clear();
			var message = _provider.GetInfoItems(_util.Request(InfoURL));
			MessageItems.AddRange(message);
		}

	}
}
