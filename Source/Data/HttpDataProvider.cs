using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LiangGeRen.Data
{
	class HttpDataUtil
	{
		CookieContainer _cookieContainer;
		internal CookieContainer CookieContainer
		{
			get
			{
				if (_cookieContainer == null)
					_cookieContainer = new CookieContainer();
				return _cookieContainer;
			}
			set
			{
				_cookieContainer = value;
			}
		}

		internal string AuthenticityToken { get; set; }

		const string LGR_URL = "http://old.liageren.com/login";
		const string LGR_REFERURL = "http://old.liageren.com/login";

		internal void InitailRequest()
		{
			string loginPage = Encoding.UTF8.GetString(Request(LGR_URL).ToArray());
			AuthenticityToken = GetAuthenticityToken(loginPage);
		}

		internal Stream Login(string userName, string password)
		{
			StringBuilder data = new StringBuilder();
			Encoding encode = new UTF8Encoding();
			data.AppendFormat("authenticity_token={0}&email={1}&password={2}", HttpUtility.UrlEncode(AuthenticityToken, encode),
				HttpUtility.UrlEncode(userName, encode),
				HttpUtility.UrlEncode(password, encode));

			return Post(LGR_URL, data.ToString());
		}

		internal MemoryStream Request(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.CookieContainer = CookieContainer;
				request.Credentials = CredentialCache.DefaultCredentials;

				request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
				//浏览器类型，如果Servlet返回的内容与浏览器类型有关则该值非常有用
				request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:30.0) Gecko/20100101 Firefox/30.0";
				request.ContentType = "application/x-www-form-urlencoded";
				//请求方式
				request.Method = "Get";
				//是否保持常连接
				request.KeepAlive = false;
				request.Headers.Add("Accept-Encoding", "gzip, deflate");

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				MemoryStream ms = new MemoryStream();
				if (response.ContentEncoding == "gzip")
				{

					GZipStream zip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
					byte[] buffer = new byte[1024];
					int l = zip.Read(buffer, 0, buffer.Length);
					while (l > 0)
					{
						ms.Write(buffer, 0, l);
						l = zip.Read(buffer, 0, buffer.Length);
					}

					zip.Dispose();
				}

				CookieContainer = request.CookieContainer;
				response.Close();
				return ms;
			}
			catch (Exception)
			{

				throw;
			}
		}

		private MemoryStream Post(string url, string data)
		{
			WebRequest request = WebRequest.Create(url);
			((HttpWebRequest)request).CookieContainer = CookieContainer;
			// Set the Method property of the request to POST.
			request.Method = "POST";
			// Create POST data and convert it to a byte array.
			string postData = data;
			byte[] byteArray = Encoding.UTF8.GetBytes(postData);
			// Set the ContentType property of the WebRequest.
			request.ContentType = "application/x-www-form-urlencoded";
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = byteArray.Length;
			// Get the request stream.
			Stream dataStream = request.GetRequestStream();
			// Write the data to the request stream.
			dataStream.Write(byteArray, 0, byteArray.Length);
			// Close the Stream object.
			dataStream.Close();
			// Get the response.
			WebResponse response = request.GetResponse();
			MemoryStream ms = new MemoryStream();
			response.GetResponseStream().CopyTo(ms);
			response.Close();
			Console.Write((response as HttpWebResponse).StatusCode);
			return ms;
		}

		private string GetAuthenticityToken(string html)
		{
			int from = html.IndexOf("authenticity_token");
			int end = html.IndexOf(" />", from);
			string temp = html.Substring(from, end - from - 1);

			string keyWord = "value=";
			int tokenIndex = temp.IndexOf(keyWord);
			string authenticityToken = temp.Substring(tokenIndex + keyWord.Length + 1);
			return authenticityToken;
		}
	}
}
