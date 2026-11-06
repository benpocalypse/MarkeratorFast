using System;
using System.IO;
using System.Reflection;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MarkeratorFast.Helpers;
using MarkeratorFast.Models;
using MarkeratorFast.ViewModels;

namespace MarkeratorFast.Views;

public partial class SiteDetailsView : UserControl
{
    public SiteDetailsView()
    {
        InitializeComponent();
        //GenerateSiteButton.IsEnabled = false;
    }

    private void GenerateSite_Clicked(object? sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            var config = (DataContext as SiteDetailsViewModel).Configuration;

            StringBuilder arguments = new StringBuilder();
            Type type = typeof(ConfigurationV1);
            FieldInfo[] properties = type.GetFields();

            foreach (FieldInfo property in properties)
            {
                Console.WriteLine(property.Name + " = " + property.GetValue(config));
            }
            
            // TODO - wire up these arguments to markerator itself, with error checking and validation

            var result = ProgramRunnerHelper.RunExternalProgram("markerator", "--help");
            Console.WriteLine(result);
        });
    }

    private async void IndexFilePicker_Clicked(object? sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);
        
        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Index Markdown File",
            AllowMultiple = false,
            SuggestedFileType = Globals.MarkdownFile,
            FileTypeFilter = new[] { Globals.MarkdownFile }
        });
        
        if (files.Count == 1)
        {
            Console.WriteLine("cwd = " + Directory.GetCurrentDirectory());
            var config = (DataContext as SiteDetailsViewModel).Configuration;
            var test = new FileInfo("input/index.md");
            try
            {
                config.IndexFile = test;
            }
            catch (Exception ex)
            {
                // GULP!
            }
            //new FileInfo("input/index.md");
            //config.IndexFile = new FileInfo(files[0].Path.ToString());
        }
    }
}
