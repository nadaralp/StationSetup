namespace FolderNestedCopy.Libs.IO;

public enum IterationType
{
    OnlyCurrentDirectory,
    Recursive
}

public class DirectoryIterator
{
    private readonly AppSettings _appSettings;

    public DirectoryIterator(DirectoryContent directoryContent, AppSettings appSettings)
    {
        _appSettings = appSettings;
        DirectoryContent = directoryContent;
    }

    public DirectoryContent DirectoryContent { get; }

    public async Task IterateFiles(IterationType iterationType, Func<FileInfo, Task> fileAction)
    {
        await Iterate(DirectoryContent, fileAction);
        if (iterationType != IterationType.Recursive) return;

        foreach (var directory in DirectoryContent.Directories)
        {
            DirectoryContent nestedDirectoryContent = new(directory, _appSettings);
            DirectoryIterator iterator = new(nestedDirectoryContent, _appSettings);
            await iterator.IterateFiles(iterationType, fileAction);
        }
    }

    private async Task Iterate(DirectoryContent directoryContent, Func<FileInfo, Task> fileAction)
    {
        foreach (var file in directoryContent.Files)
        {
            FileInfo fileInfo = new(file);
            await fileAction(fileInfo);
        }
    }
}