using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiangGeRen.Data;
using LiangGeRen.Extension;

namespace LiangGeRen
{
	/// <summary>
	/// MainPage.xaml 的交互逻辑
	/// </summary>
	public partial class MainPage : Window
	{
		private MoodDataManager _moodDataManager;
		private InfoDataManager _infoDataManger;
		public MainPage()
		{
			InitializeComponent();
			List<MessageItem> abc = new List<MessageItem>();
			abc.Add(new MessageItem("Eric", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			abc.Add(new MessageItem("Kitty", "fewfefijivjivjieoffeof", "2 messages"));
			abc.Add(new MessageItem("Eric", ";ojojonihuuhnlijioo", "2 messages"));
			abc.Add(new MessageItem("Kitty", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			abc.Add(new MessageItem("Eric", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			messageListBox.ItemsSource = abc;
		}

		internal MainPage(MoodDataManager mdManager)
		{
			_moodDataManager = mdManager;
			_infoDataManger = new InfoDataManager(mdManager.HttpUtil);
			InitializeComponent();
			InitialList();
		}

		internal static String FirstComingInGuy
		{ get; set; }

		private void InitialList()
		{
			if (_moodDataManager.MessageItems.Count > 0)
			{
				messageListBox.ItemsSource = _moodDataManager.MessageItems;
				FirstComingInGuy = _moodDataManager.MessageItems.First().Name;
				messageListBox.DataContext = this;
			}
			UpdateInfo();
			InfoItemsProvider idf = new InfoItemsProvider();
			ObservableCollection<MessageItem> sss = new ObservableCollection<MessageItem>();
			FileStream fileStream = new FileStream(@"C:\Users\Eric\Desktop\new  2.html", FileMode.Open);
			try
			{
				sss.AddRange(idf.GetInfoItems(fileStream));
				// read from file or write to file
			}
			finally
			{
				fileStream.Close();
			}

			//infoListBox.ItemsSource = _infoDataManger.MessageItems;
			infoListBox.ItemsSource = sss;
			//Action InitialInfo = UpdateInfo;
			//InitialInfo.BeginInvoke(UpdateInfoCompleted, InitialInfo);
		}

		private void UpdateInfo()
		{
			_infoDataManger.UpdateInfo();
		}

		private void UpdateInfoCompleted(IAsyncResult asynResult)
		{
			if (asynResult == null)
				return;
			(asynResult.AsyncState as Action).EndInvoke(asynResult);
			//infoListBox.ItemsSource = _infoDataManger.MessageItems;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void minimumBtn_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Minimized;
		}

		private void DragWindow(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void infoButton_Click(object sender, RoutedEventArgs e)
		{
			messageListBox.Visibility = System.Windows.Visibility.Collapsed;
			infoListBox.Visibility = System.Windows.Visibility.Visible;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			_moodDataManager.GetMoreMessage();
		}

		const string InitailText = "回复:";
		private void replyTB_GotFocus(object sender, RoutedEventArgs e)
		{
			var textBox = (TextBox)sender;
			if (textBox.Text == InitailText)
			{
				textBox.Text = string.Empty;
			}
		}

		private void submit(object sender, RoutedEventArgs e)
		{
			var replyContentTextBox = messageListBox.ItemTemplate.FindName("replyContent", messageListBox) as TextBox;
			if (replyContentTextBox != null
				&& replyContentTextBox.Text != string.Empty
				&& replyContentTextBox.Text != InitailText)
			{
 
			}
		}
	}

	public class ColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var name = (string)value;
			var firstComingGuy = MainPage.FirstComingInGuy;
			SolidColorBrush solidColorBrush = new SolidColorBrush();
			
			if (name.Equals(firstComingGuy))
				solidColorBrush.Color = Color.FromRgb(0, 159, 214); 
			else
				solidColorBrush.Color = Color.FromRgb(0, 193, 164);
			return solidColorBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new NotImplementedException();
		}
	}
}
