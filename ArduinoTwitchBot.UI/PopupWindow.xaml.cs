namespace ArduinoTwitchBot.UI;

using System.Windows;
using System.Windows.Controls;

public partial class PopupWindow : Window
{
	public PopupWindow(Page content)
	{
		InitializeComponent();

		this.PageFrame.Content = content;
	}
}