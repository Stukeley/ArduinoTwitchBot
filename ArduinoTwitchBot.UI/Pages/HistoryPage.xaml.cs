using System;
using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class HistoryPage : Page
	{
		public HistoryPage()
		{
			InitializeComponent();
#if DEBUG
			Code.EventHistory.TwitchEvents.Insert(0, new Code.TwitchEvent("test1", DateTime.Now, Code.TwitchEventType.Follow));
			Code.EventHistory.TwitchEvents.Insert(0, new Code.TwitchEvent("test2", DateTime.Now, Code.TwitchEventType.Sub));
			Code.EventHistory.TwitchEvents.Insert(0, new Code.TwitchEvent("test3", DateTime.Now, Code.TwitchEventType.Bits));
#endif

			EventHistoryListView.ItemsSource = Code.EventHistory.TwitchEvents;
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}

		private void PopupButton_Click(object sender, RoutedEventArgs e)
		{
			// Create a second window only if it's not already open.
			foreach (var window in Application.Current.Windows)
			{
				if (window is PopupWindow)
				{
					return;
				}
			}

			var historyWindow = new PopupWindow(this);
			historyWindow.Show();

			// Hide unnecessary buttons.
			this.PopupButton.Visibility = Visibility.Collapsed;
			this.MainPageButton.Visibility = Visibility.Collapsed;

			// Go back to HomePage.
			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}
	}
}
