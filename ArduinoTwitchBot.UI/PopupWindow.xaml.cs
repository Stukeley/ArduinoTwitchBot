using System.Windows;
using System.Windows.Controls;

namespace ArduinoTwitchBot.UI
{
	public partial class PopupWindow : Window
	{
		public PopupWindow(Page content)
		{
			InitializeComponent();

			this.PageFrame.Content = content;
		}
	}
}
