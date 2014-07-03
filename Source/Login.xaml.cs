using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiangGeRen.Data;
using LiangGeRen.Extension;

namespace LiangGeRen
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		MoodDataManager _moodeDataManager;
		User user;
		public MainWindow()
		{
			InitializeComponent();
			InitializeRequest();
			InitialBinding();
		}

		private void InitialBinding()
		{
			user = new User();
			Binding binding = new Binding("IsLoginSucceed") { Source = user };
			binding.Converter = new VisibiltyConverter();
			errorData.SetBinding(Label.VisibilityProperty, binding);
		}

		private void InitializeRequest()
		{
			_moodeDataManager = new MoodDataManager();
			_moodeDataManager.InitailRequest();
		}

		private bool isProcessLogin = false;
		private void OnClick(object sender, RoutedEventArgs e)
		{
			if (!isProcessLogin)
			{
				isProcessLogin = true;
				Func<string, bool> login = Login;
				string un = userName.Text;
				IAsyncResult asynResult = login.BeginInvoke(un, LoginCompleted, login);
			}
		}

		private delegate bool LoginHandler();
		private void LoginCompleted(IAsyncResult asynResult)
		{
			if (asynResult == null)
				return;
			bool isSuccessed = (asynResult.AsyncState as Func<string, bool>).EndInvoke(asynResult);
			if (isSuccessed)
			{
				user.IsLoginSucceed = true;
				Action action = () => {
					MainPage mainPage = new MainPage(_moodeDataManager);
					this.Hide();
					mainPage.Show();
				};
				Dispatcher.Invoke(DispatcherPriority.Normal, action);
			}
			else
			{
				userName.Dispatcher.BeginInvoke(new Action(() => { userName.Text = string.Empty; }), null);
				user.IsLoginSucceed = false;
			}
		}



		internal delegate void DeleFunc();
		internal void Func()
		{
			MainPage mainPage = new MainPage(_moodeDataManager);
			this.Hide();
			mainPage.Show();
		}



		private bool Login(string userName)
		{
			IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(passWord.SecurePassword);  
            string pw = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
			return _moodeDataManager.Login(userName, pw);
		}

		const string UserNameKeyWords = "邮箱";
		private void userName_GotFocus(object sender, RoutedEventArgs e)
		{
			if (userName.Text == UserNameKeyWords)
			{
				userName.Text = string.Empty;
				userName.Foreground = Brushes.Black;
			}
			
		}

		private void userName_LostFocus(object sender, RoutedEventArgs e)
		{
			if (userName.Text.Trim().Length == 0)
			{
				userName.Foreground = Brushes.Gray;
				userName.Text = UserNameKeyWords;
			}
		}
	}

	internal class User : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public string PassWord { get; set; }

		private bool _isLoginSucceed = true;
		public bool IsLoginSucceed 
		{
			get
			{
				return _isLoginSucceed;
			}
			set
			{
				_isLoginSucceed = value;
				OnPropertyChnaged("IsLoginSucceed");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChnaged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class VisibiltyConverter : IValueConverter
	{
 		 public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		 {
			 var isNotVisible = (bool)value;
			 if (isNotVisible)
				 return Visibility.Collapsed;
			 return Visibility.Visible;
		 }

		 public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		 {
			 throw new NotImplementedException();
		 }
	}
}
