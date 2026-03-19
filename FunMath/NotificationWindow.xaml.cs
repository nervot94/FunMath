using System.Windows;

namespace FunMath;

/// <summary>
/// Interaction logic for NotificationWindow.xaml
/// </summary>
public partial class NotificationWindow : Window
{
    public NotificationWindow(string message)
    {
        InitializeComponent();
        MessageTextBlock.Text = message;
        Loaded += NotificationWindow_Loaded;
    }

    private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Activate();
        Focus();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
