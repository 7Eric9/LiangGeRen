using System;
using System.Collections.Generic;
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

namespace LiangGeRen
{
	/// <summary>
	/// MainPage.xaml 的交互逻辑
	/// </summary>
	public partial class MainPage : Window
	{
		private MoodDataManager _moodDataManager;
		public MainPage()
		{
			InitializeComponent();
			List<MessageItem> abc = new List<MessageItem>();
			abc.Add(new MessageItem("Eric", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			abc.Add(new MessageItem("Kitty", "fewfefijivjivjieoffeof", "2 messages"));
			abc.Add(new MessageItem("Eric", ";ojojonihuuhnlijioo", "2 messages"));
			abc.Add(new MessageItem("Kitty", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			abc.Add(new MessageItem("Eric", "fjiowfejowjfkefjweifjifjowjflwf", "2 messages"));
			listBox1.ItemsSource = abc;
		}

		internal MainPage(MoodDataManager mdManager)
		{
			_moodDataManager = mdManager;
			InitializeComponent();
			InitialList();
		}

		private void InitialList()
		{
			listBox1.ItemsSource = _moodDataManager.MessageItems;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void minimumBtn_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Minimized;
		}

		private void DragWindow(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
	}
}
