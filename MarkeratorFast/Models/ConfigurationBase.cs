using System;
using System.Collections.Immutable;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MarkeratorFast.Models;

public partial class ConfigurationBase : ObservableObject
{
    private const int Version = 1;
    
    [ObservableProperty]
    public string _projectName = string.Empty;
    [ObservableProperty]
    public string _rootDirectory = string.Empty;

    [ObservableProperty]
    public string _siteTitle = string.Empty;
    [ObservableProperty]
    public Uri _siteAddress = default;
    [ObservableProperty]
    public FileInfo _indexFile = default;

    [ObservableProperty]
    public ImmutableList<string> _feedTitles = default;
    [ObservableProperty]
    public int? _paginationLimit = null;
    [ObservableProperty]
    public FileInfo? _favIcon = null;
    [ObservableProperty]
    public bool _generateRss = false;
    [ObservableProperty]
    public FileInfo? _rssIcon = null;
    [ObservableProperty]
    public string? _customCss = null;
}
