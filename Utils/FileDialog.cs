using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class FileDialog
{
    private static Window? GetMainWindow()
    {
        return Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
    }

    public static async Task<string?> OpenImageDialog(string? title = "Wybierz obraz")
    {
        var dialog = new OpenFileDialog
        {
            Title = title,
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Obrazy", Extensions = { "png", "jpg", "jpeg", "bmp" } }
            },
            AllowMultiple = false
        };

        var result = await dialog.ShowAsync(GetMainWindow());
        return result?.Length > 0 ? result[0] : null;
    }

    public static async Task<string?> OpenFileDialog(List<(string Name, List<string> Extensions)> filters, string? title = "Wybierz plik")
    {
        var fileDialogFilters = new List<FileDialogFilter>();
        foreach (var (name, extensions) in filters)
        {
            var filter = new FileDialogFilter { Name = name };
            foreach (var ext in extensions)
                filter.Extensions.Add(ext);
            fileDialogFilters.Add(filter);
        }

        var dialog = new OpenFileDialog
        {
            Title = title,
            Filters = fileDialogFilters,
            AllowMultiple = false
        };

        var result = await dialog.ShowAsync(GetMainWindow());
        return result?.Length > 0 ? result[0] : null;
    }

    public static async Task<string?> OpenSaveDialog(List<(string Name, List<string> Extensions)> filters, string defaultFileName = "", string? title = "Zapisz plik")
    {
        var fileDialogFilters = new List<FileDialogFilter>();
        foreach (var (name, extensions) in filters)
        {
            var filter = new FileDialogFilter { Name = name };
            foreach (var ext in extensions)
                filter.Extensions.Add(ext);
            fileDialogFilters.Add(filter);
        }

        var dialog = new SaveFileDialog
        {
            Title = title,
            InitialFileName = defaultFileName,
            Filters = fileDialogFilters
        };

        return await dialog.ShowAsync(GetMainWindow());
    }
}
