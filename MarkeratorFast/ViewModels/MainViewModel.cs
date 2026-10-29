using CommunityToolkit.Mvvm.ComponentModel;

namespace MarkeratorFast.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isOpenPanel = true;

    [ObservableProperty] private ViewModelBase _currentPage = new WelcomePageViewModel();

    public void UpdateLayout(double windowWidth)
    {
        IsOpenPanel = windowWidth < 640 ? false : true;
    }
}
