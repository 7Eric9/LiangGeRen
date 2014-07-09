using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiangGeRen.Data
{
	class InfoItemsProvider
	{
		ObservableCollection<MessageItem> _messageItems;
		public ObservableCollection<MessageItem> MessageItems
		{
			get
			{
				if (_messageItems == null)
					_messageItems = new ObservableCollection<MessageItem>();
				return _messageItems;
			}
			set
			{
				_messageItems = value;
			}
		}

		public List<MessageItem> GetInfoItems(Stream dataStream)
		{
				dataStream.Seek(0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader(dataStream);

			bool readyToReadName = false;
			string name = string.Empty;
			string message = string.Empty;
			string time = string.Empty;
			List<MessageItem> messageItems = new List<MessageItem>();
			string content;
			while ((content = reader.ReadLine()) != null)
			{
				if (readyToReadName)
				{
					name = content.Trim();
					readyToReadName = false;
				}
				if (content.Contains("feed_story_wrapper clearfix"))
				{
					readyToReadName = true;
				}
				if (content.Contains("class=\"c_tx\""))
				{
					int startIndex = content.IndexOf(">") + 1;
					int endIndex = content.IndexOf("</a>");
					int length = endIndex - startIndex;
					message = content.Substring(startIndex, length);
				}

				if (content.Contains("timeago c_tx3"))
				{
					int startIndex = content.IndexOf(">") + 1;
					int endIndex = content.IndexOf("</span>");
					int length = endIndex - startIndex - 1;
					time = content.Substring(startIndex, length);
					messageItems.Add(new MessageItem(name, message, time));

					name = string.Empty;
					message = string.Empty;
					time = string.Empty;
				}
			}
			return messageItems;
			
		}
	}
}
