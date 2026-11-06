using Avalonia.Platform.Storage;

namespace MarkeratorFast;

public static class Globals
{
    public static readonly string Version = "0.0.4";
    public const int CurrentConfigurationVersion = 1;
    
    public static FilePickerFileType MarkdownFile { get; } = new("Markdown Files")
    {
        Patterns = new[] { "*.md" },
        AppleUniformTypeIdentifiers = new[] { "markdown.file" },
        MimeTypes = new[] { "text/*" }
    };
}