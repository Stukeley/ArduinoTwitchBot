using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI.Pages
{
	public partial class EmotesPage : Page
	{
		public EmotesPage()
		{
			InitializeComponent();
		}

		private void MainPageButton_Click(object sender, RoutedEventArgs e)
		{
			// Save emotes.


			(Application.Current.MainWindow as MainWindow).PageFrame.Navigate(new MainPage());
		}
	}
}
