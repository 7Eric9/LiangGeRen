using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReplyMessage = LiangGeRen.Data.MessageItem.ReplyMessage;

namespace LiangGeRen.Data
{
	internal class MessageItemsProvider
	{
		public bool IsValid { get; set; }
		public string CoupleID { get; set; }


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
			bool readyToReadReplyMessage = false;
			string name = string.Empty;
			string message = string.Empty;
			string replyMessage = string.Empty;
			string replyMessageKey = string.Empty;
			string time = string.Empty;
			string coupleIdKeyWords = "<span id='qzoneurl'>http://old.liageren.com/couples/";
			List<MessageItem> messageItems = new List<MessageItem>();
			string content;
			while ((content = reader.ReadLine()) != null)
			{
				if (content.Contains("你的邮箱或密码不符，请再试一次")
					|| content.Contains("邮箱格式不正确"))
				{
					isValid = false;
					break;
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
				}
				if (content.Contains(coupleIdKeyWords))
				{
					var temp = AbstractContent(coupleIdKeyWords, "</span>", content);
					CoupleID = temp.Replace(coupleIdKeyWords.Substring(1), string.Empty);
				}
				if (content.Contains("m_qualifier q_says"))
				{
					name = AbstractContent(">","</span>",content);

				}
				if (content.Contains("entry-content"))
				{
					readyToReadMessage = true;
				}
				if (content.Contains("<a class=\"c_tx3\""))
				{
					var rawID = AbstractContent("id=\"","\"",content);
					var id = rawID.Substring(rawID.LastIndexOf("_") + 1);
					var rawHref = AbstractContent("href=\"", "\"", content);
					var href = rawHref.Split('/');
					replyMessageKey = href.Last() + "," + id;
				}
				if (content.Contains("dataType: 'script'});return false;"))
				{
					readyToReadReplyMessage = true;
				}
				if (content.Contains("timeago c_tx3"))
				{
					time= AbstractContent(">","</span>",content);

					messageItems.Add(new MessageItem(name, message, replyMessage,string.Empty, replyMessageKey));
					name = string.Empty;
					message = string.Empty;
					replyMessage = string.Empty;
					replyMessageKey = string.Empty;
					time = string.Empty;
				}
			}
			IsValid = isValid;
			return messageItems;
		}

		private string AbstractContent(string startContent,int fromIndex,
			string endContent, string content)
		{
			int startIndex = content.IndexOf(startContent, fromIndex) + 1;
			int endIdex = content.IndexOf(endContent, startIndex+startContent.Length);
			int length = endIdex - startIndex;
			return content.Substring(startIndex, length).Trim();
		}

		private string AbstractContent(string startContent,
			string endContent, string content)
		{
			return AbstractContent(startContent, 0, endContent, content);
		}

		public List<ReplyMessage> PocessReplyMessages(Stream dataStream)
		{
			dataStream.Seek(0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader(dataStream);
			string name = string.Empty;
			string message = string.Empty;
			string content;
			string replynameKeyWord = "replyername";
			string replymessageKeyWord = "replycontent";
			List<ReplyMessage> replyMessages = new List<ReplyMessage>();
			while ((content = reader.ReadLine()) != null)
			{
				if (content.Contains("html += \"<div id="))
				{
					var fromIndex = content.IndexOf(replynameKeyWord) 
						+ replynameKeyWord.Length;
					name = AbstractContent(">", fromIndex, "<\\/a>", content);

					var fromIndexForMessage = content.IndexOf(replymessageKeyWord)
						+ replymessageKeyWord.Length;

					int endIndex = content.IndexOf(">", fromIndex);
					int length = endIndex - fromIndex;
					var temp = content.Substring(fromIndex, length);
					bool isSlef = temp.Contains("blank");
					string endContent = isSlef ? "<\\/span>" : "<a class";

					message = AbstractContent(">", fromIndexForMessage, endContent, content);
					
					if (message.Contains("<a class=\\\"c_tx"))
					{
						message = message.Substring(0, message.IndexOf("<a class=\\\"c_tx"));
					}
					message = 
						(message.Replace("\\n", string.Empty)
						.Replace("\n", string.Empty)
						.Replace("\t", string.Empty)).Trim();

					replyMessages.Add(new ReplyMessage(name, message));
					name = string.Empty;
					message = string.Empty;
				}
			}
			return replyMessages;
		}
	}
}
