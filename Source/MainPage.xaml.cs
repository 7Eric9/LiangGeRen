﻿using System;
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

		private void InitialList()
		{
			messageListBox.ItemsSource = _moodDataManager.MessageItems;
			UpdateInfo();
			infoListBox.ItemsSource = _infoDataManger.MessageItems;
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

		private void infoButton_Click(object sender, RoutedEventArgs e)
		{
			messageListBox.Visibility = System.Windows.Visibility.Collapsed;
			infoListBox.Visibility = System.Windows.Visibility.Visible;
		}
	}
}
