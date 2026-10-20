using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MarkeratorFast.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] 
    private bool _isOpenPanel = true;

    [ObservableProperty]
    private ViewModelBase _currentPage = new WelcomePageViewModel();
    
    public void UpdateLayout(double windowWidth)
    {
        IsOpenPanel = windowWidth < 640 ? false : true;
    }

    [RelayCommand]
    private void TriggerPanel()
    {
        IsOpenPanel = !IsOpenPanel; 
    }
}
