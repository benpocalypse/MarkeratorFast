using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using MarkeratorFast.ViewModels;

namespace MarkeratorFast.Views;

public partial class MainView : Window
{
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
            var vm = this.DataContext as MainViewModel;
            vm.CurrentPage = new WelcomePageViewModel();

            e.Handled = true;
        };

        SiteDetailsButton.Click += (s, e) =>
        {
            WelcomeButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            SiteDetailsButton[!BackgroundProperty] = new DynamicResourceExtension("ButtonBackground");
            var vm = this.DataContext as MainViewModel;
            vm.CurrentPage = new SiteDetailsViewModel();

            e.Handled = true;
        };

        PreviewButton.Click += (s, e) =>
        {
            WelcomeButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            SiteDetailsButton[!BackgroundProperty] = new DynamicResourceExtension("FlyoutBackground");
            PreviewButton[!BackgroundProperty] = new DynamicResourceExtension("ButtonBackground");
            var vm = this.DataContext as MainViewModel;
            vm.CurrentPage = new PreviewViewModel();

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

        AboutButton.Click += (s, e) =>
        {
            diagHost.IsOpen = true;
        };
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
