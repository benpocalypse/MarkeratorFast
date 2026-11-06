using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using DialogHostAvalonia;
using MarkeratorFast.ViewModels;

namespace MarkeratorFast.Views;

public partial class MainView : Window
{
    private string? _filename = null;
    
    public MainView()
    {
        InitializeComponent();

        WindowChrome.PointerPressed += (i, e) => { BeginMoveDrag(e); };

        SetupSide(Left, StandardCursorType.LeftSide, WindowEdge.West);
        SetupSide(Right, StandardCursorType.RightSide, WindowEdge.East);
        SetupSide(Top, StandardCursorType.TopSide, WindowEdge.North);
        SetupSide(Bottom, StandardCursorType.BottomSide, WindowEdge.South);
        SetupSide(TopLeft, StandardCursorType.TopLeftCorner, WindowEdge.NorthWest);
        SetupSide(TopRight, StandardCursorType.TopRightCorner, WindowEdge.NorthEast);
        SetupSide(BottomLeft, StandardCursorType.BottomLeftCorner, WindowEdge.SouthWest);
        SetupSide(BottomRight, StandardCursorType.BottomRightCorner, WindowEdge.SouthEast);

        MinimizeButton.Click += delegate { this.WindowState = WindowState.Minimized; };

        MaximizeButton.Click += delegate
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };

        CloseButton.Click += delegate { Close(); };

        WelcomeButton.Click += (s, e) =>
        {
            WelcomeButton[!BackgroundProperty] = new DynamicResourceExtension("ButtonBackground");
            SiteDetailsButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            PreviewButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            var vm = this.DataContext as MainViewModel;
            vm?.CurrentPage = new WelcomePageViewModel();

            e.Handled = true;
        };

        SiteDetailsButton.Click += (s, e) =>
        {
            WelcomeButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            SiteDetailsButton[!BackgroundProperty] = new DynamicResourceExtension("ButtonBackground");
            PreviewButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            var vm = this.DataContext as MainViewModel;
            vm?.CurrentPage = new SiteDetailsViewModel();

            e.Handled = true;
        };

        PreviewButton.Click += (s, e) =>
        {
            WelcomeButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            SiteDetailsButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            PreviewButton[!BackgroundProperty] = new DynamicResourceExtension("ButtonBackground");
            var vm = this.DataContext as MainViewModel;
            vm?.CurrentPage = new PreviewViewModel();

            e.Handled = true;
        };

        SwitchPaneButton.Click += (s, e) =>
        {
            MainSplitView.CompactPaneLength = MainSplitView.CompactPaneLength == 0 ? 36 : 0;
            SwitchPaneButton[!BackgroundProperty] = MainSplitView.CompactPaneLength == 0 ? 
                new DynamicResourceExtension("ButtonBackground") :
                new DynamicResourceExtension("FlyoutBackground");
            
            e.Handled = true;
        };

        DownloadButton.Click += (s, e) =>
        {
            using (var client = new WebClient())
            {
                DialogHost.Show(new DownloadProgressDialogView(), "MainDialogHost");
                client.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                client.DownloadDataCompleted += WebClient_DownloadCompleted;
                client.DownloadDataAsync(new Uri("https://github.com/benpocalypse/Markerator/releases/download/1.0.0/markerator-linux-x64-1.0.0.zip"), "markerator-linux-x64-1.0.0.zip");
                
                e.Handled = true;
            }
        };

        AboutButton.Click += (s, e) =>
        {
            diagHost.IsOpen = true;
            e.Handled = true;
        };
        
        _ = new Timer(CheckForUpdates_Callback, null, TimeSpan.FromMilliseconds(100), TimeSpan.Zero);
    }

    private void CheckForUpdates_Callback(object? state)
    {
        using (var client = new WebClient())
        {
            _filename = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "markerator-linux-x64-1.0.0.zip" :
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "markerator-windows-x64-1.0.0.zip" :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "markerator-osx-arm64-1.0.0.zip" :
                null;

            if (_filename is null)
            { // TODO - surface a user facing error message in the future
                return;
            }

            try
            {
                FileStream fStream = new  FileStream(_filename, FileMode.Open);
                fStream.Position = 0;
            
                var newMarkeratorSha = SHA256Managed.Create().ComputeHash(client.OpenRead("https://github.com/benpocalypse/Markerator/releases/download/1.0.0/markerator-linux-x64-1.0.0.zip"));
                var currentMarkeratorSha = SHA256Managed.Create().ComputeHash(fStream);

                StringBuilder newMarkeratorShaString = new StringBuilder();
                foreach (var b in newMarkeratorSha)
                {
                    newMarkeratorShaString.Append(b.ToString("x2"));
                }
            
                StringBuilder currentMarkeratorShaString = new StringBuilder();
                foreach (var b in currentMarkeratorSha)
                {
                    currentMarkeratorShaString.Append(b.ToString("x2"));
                }

                if (!newMarkeratorShaString.Equals(currentMarkeratorShaString))
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogHost.Show(new DownloadProgressDialogView(), "MainDialogHost");
                        client.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                        client.DownloadDataCompleted += WebClient_DownloadCompleted;
                        client.DownloadDataAsync(
                            new Uri(
                                $"https://github.com/benpocalypse/Markerator/releases/download/1.0.0/{_filename}"),
                                _filename);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // TODO - gulp the exception for now, but fix this in the future.
                
                Dispatcher.Invoke(() =>
                {
                    DialogHost.Show(new DownloadProgressDialogView(), "MainDialogHost");
                    client.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    client.DownloadDataCompleted += WebClient_DownloadCompleted;
                    client.DownloadDataAsync(
                        new Uri(
                            $"https://github.com/benpocalypse/Markerator/releases/download/1.0.0/{_filename}"),
                        _filename);
                });
            }
            
        }
    }

    private void WebClient_DownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
    {
        var children = ((DownloadProgressDialogView)DialogHost.GetDialogSession("MainDialogHost")?.Content)
            .GetLogicalChildren();
        
        var progressText = (TextBlock)children.ElementAt(0).LogicalChildren[1];
        var progressBar = (ProgressBar)children.ElementAt(0).LogicalChildren[0];
        
        progressText.Text = "Done!";
        DownloadButton.IsEnabled = false;
        
        //DialogHost.GetDialogSession("MainDialogHost")?.Close(true);
        
        _  = new Timer(UnzipFile_Callback, null, TimeSpan.FromMilliseconds(1000), TimeSpan.Zero);
    }

    private void UnzipFile_Callback(object? state)
    {
        Dispatcher.Invoke(() =>
        {
            var children = ((DownloadProgressDialogView)DialogHost.GetDialogSession("MainDialogHost")?.Content)
                .GetLogicalChildren();
            var progressText = (TextBlock)children.ElementAt(0).LogicalChildren[1];
            var progressBar = (ProgressBar)children.ElementAt(0).LogicalChildren[0];

            progressText.Text = "Unzipping...";
            progressBar.Value = 0;

            ZipFile.ExtractToDirectory(_filename, Environment.CurrentDirectory, Encoding.UTF8, true);

            progressText.Text = "Done!";
            progressBar.Value = 100;

            DialogHost.GetDialogSession("MainDialogHost")?.Close(true);
        });
    }

    private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        var children = ((DownloadProgressDialogView)DialogHost.GetDialogSession("MainDialogHost")?.Content)
            .GetLogicalChildren();
        
        var progressBar = (ProgressBar)children.ElementAt(0).LogicalChildren[0];
        var progressText = (TextBlock)children.ElementAt(0).LogicalChildren[1];
        
        progressText.Text = "Downloading...";
        progressBar.Value = e.ProgressPercentage;
    }

    private void SetupSide(Control ctl, StandardCursorType cursor, WindowEdge edge)
    {
        ctl.Cursor = new Cursor(cursor);
        
        ctl.PointerPressed += (i, e) =>
        {
            if (WindowState == WindowState.Normal)
                BeginResizeDrag(edge, e);
        };
    }
    
    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        
        if (DataContext is MainViewModel vm)
        {
            vm.UpdateLayout(e.NewSize.Width);
        }
    }
}
