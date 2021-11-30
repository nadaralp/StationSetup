namespace FolderNestedCopy;

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

    public void IterateFiles(IterationType iterationType, Func<FileInfo, Task> fileAction)
    {
        Iterate(DirectoryContent, fileAction);
        if (iterationType != IterationType.Recursive) return;

        foreach (var directory in DirectoryContent.Directories)
        {
            DirectoryContent nestedDirectoryContent = new(directory, _appSettings);
            DirectoryIterator iterator = new(nestedDirectoryContent, _appSettings);
            iterator.IterateFiles(iterationType, fileAction);
        }
    }

    private void Iterate(DirectoryContent directoryContent, Func<FileInfo, Task> fileAction)
    {
        foreach (var file in directoryContent.Files)
        {
            FileInfo fileInfo = new(file);
            fileAction(fileInfo);
        }
    }
}