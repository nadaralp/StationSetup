namespace FolderNestedCopy.Services.IO;

public class DirectoryContent
{
    private readonly AppSettings _appSettings;

    public DirectoryContent(string currentDirectory, AppSettings appSettings)
    {
        _appSettings = appSettings;
        CurrentDirectory = currentDirectory;
        Files = Directory.GetFiles(currentDirectory);
        Directories = RemoveExcludedDirectories(Directory.GetDirectories(currentDirectory));
    }


    /// <summary>
    ///     Current Directory
    /// </summary>
    public string CurrentDirectory { get; }


    /// <summary>
    ///     Directories inside the current directory
    /// </summary>
    public string[] Directories { get; }


    /// <summary>
    ///     Files inside the current directory
    /// </summary>
    public string[] Files { get; }

    private string[] RemoveExcludedDirectories(string[] directories)
    {
        return directories
            .Where(x => _appSettings.ExcludedDirectories.All(d => d != new DirectoryInfo(x).Name))
            .ToArray();
    }
}