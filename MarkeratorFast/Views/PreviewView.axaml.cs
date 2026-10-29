using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MarkeratorFast.Views;

public partial class PreviewView : UserControl
{
    public PreviewView()
    {
        InitializeComponent();
        
        //TODO - have this follow the config
        webView.Source = new Uri($@"file:///home/flecko/Projects/Markerator/bin/Debug/net10.0/output/index.html");
    }
    
    private void WebView_NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs args)
    {
        if (args.IsSuccess)
        {
            // Navigation completed successfully
        }
    }
}