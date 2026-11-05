using MarkeratorFast.Models;

namespace MarkeratorFast.ViewModels;

public class SiteDetailsViewModel : ViewModelBase
{
    public ConfigurationV1 Configuration { get; set; } = new ConfigurationV1();
}
