using System.Text.RegularExpressions;

namespace FolderNestedCopy.Libs.IO;

public static class FileHelper
{
    public static string ReplaceBackSlashWithForwardSlash(string path) => path.Replace("\\", "/");

    public static string RemoveDiskPartitionFromKey(string path)
    {
        var diskPartitionRegex = new Regex(@"\w:\\");
        var match = diskPartitionRegex.Match(path);
        return path.Replace(match.Value, "");
    }

    public static string RemoveUserGivenFolderPathExceptLastFolder(string file, string userGivenFolderPathToCopy)
    {
        var userGivenFolderForwardSlashes = ReplaceBackSlashWithForwardSlash(userGivenFolderPathToCopy);
        var fileForwardSlashes = ReplaceBackSlashWithForwardSlash(file);
        string lastDirectoryInUserGivenPath = userGivenFolderForwardSlashes.Split("/")[^1];

        string filePathWithRemovedUserPath = fileForwardSlashes.Replace(userGivenFolderForwardSlashes, "");
        string filePathAppendedFirstDirectory = Path.Join(lastDirectoryInUserGivenPath, filePathWithRemovedUserPath);
        return filePathAppendedFirstDirectory;
    }
}