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
}