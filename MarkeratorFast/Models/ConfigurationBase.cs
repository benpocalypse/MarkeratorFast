using System;
using System.Collections.Immutable;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MarkeratorFast.Models;

public partial class ConfigurationBase : ObservableObject
{
    private const int Version = 1;
    
    [ObservableProperty]
    private string _projectName = string.Empty;
    [ObservableProperty]
    private string _rootDirectory = string.Empty;

    [ObservableProperty]
    private string _siteTitle = string.Empty;
    [ObservableProperty]
    private Uri _siteAddress = default;
    [ObservableProperty]
    private FileInfo _indexFile = default;

    [ObservableProperty]
    private ImmutableList<string> _feedTitles = default;
    [ObservableProperty]
    private int? _paginationLimit = null;
    [ObservableProperty]
    private FileInfo? _favIcon = null;
    [ObservableProperty]
    private bool _generateRss = false;
    [ObservableProperty]
    private FileInfo? _rssIcon = null;
    [ObservableProperty]
    private string? _customCss = null;
}
