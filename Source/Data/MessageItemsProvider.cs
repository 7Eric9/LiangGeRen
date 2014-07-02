﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiangGeRen.Data
{
	internal class MessageItemsProvider
	{
		public bool IsValid { get; set; }


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

		public List<MessageItem> GetMessageItems(Stream dataStream)
		{
			dataStream.Seek(0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader(dataStream);

			bool isValid = true;
			bool readyToReadMessage = false;
			bool closeMessageType = false;
			bool readyToReadReplyMessage = false;
			string name = string.Empty;
			string message = string.Empty;
			string replyMessage = string.Empty;
			List<MessageItem> messageItems = new List<MessageItem>();
			string content;
			while ((content = reader.ReadLine()) != null)
			{
				if (content.Contains("你的邮箱或密码不符，请再试一次"))
				{
					isValid = false;
					break;
				}
				if (closeMessageType)
				{
					messageItems.Add(new MessageItem(name, message, replyMessage));
					name = string.Empty;
					message = string.Empty;
					replyMessage = string.Empty;
					closeMessageType = false;
				}
				if (readyToReadMessage)
				{
					message = content.Trim();
					readyToReadMessage = false;
				}
				if (readyToReadReplyMessage)
				{
					replyMessage = content.Trim();
					readyToReadReplyMessage = false;
					closeMessageType = true;
				}
				if (content.Contains("m_qualifier q_says"))
				{
					int startIndex = content.IndexOf('>') + 1;
					int endIdex = content.IndexOf("</span>");
					int length = endIdex - startIndex;
					name = content.Substring(startIndex, length);

				}
				if (content.Contains("entry-content"))
				{
					readyToReadMessage = true;
				}
				if (content.Contains("dataType: 'script'});return false;"))
				{
					readyToReadReplyMessage = true;
				}
			}
			IsValid = isValid;
			return messageItems;
		}
	}
}