using System.Threading;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MarkeratorFast.ViewModels;

namespace MarkeratorFast.Views;

public partial class SiteDetailsView : UserControl
{
    public SiteDetailsView()
    {
        InitializeComponent();
    }

    private void GenerateSite_Clicked(object? sender, RoutedEventArgs e)
    {
        Dispatcher? dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
        if (dispatcher is not null)
        {
            dispatcher.Post(() =>
            {
                var config = (DataContext as SiteDetailsViewModel).Configuration;
            });
        }
    }
}
