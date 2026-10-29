using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DialogHostAvalonia;

namespace MarkeratorFast.Views;

public partial class AboutDialogView : UserControl
{
    public AboutDialogView()
    {
        InitializeComponent();
        MarkeratorFastVersionText.Text = Globals.Version;
    }

    private void CloseButton_Clicked(object? sender, RoutedEventArgs e)
    {
        DialogHost.GetDialogSession("MainDialogHost")?.Close(true);
    }

    private void WebsiteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://www.github.com/benpocalypse/markeratorfast") { UseShellExecute = true });
    }

    private void IssuesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/benpocalypse/MarkeratorFast/issues") { UseShellExecute = true });
    }
}