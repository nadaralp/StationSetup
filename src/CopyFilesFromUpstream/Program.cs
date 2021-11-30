using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

Console.WriteLine("Enter the S3 bucket to copy from");
var s3Bucket = Console.ReadLine();
if (s3Bucket is null)
{
    throw new Exception("Bucket must have a value");
}


Console.WriteLine("Enter bucket prefix (optional). Which means the directory to copy from in the S3 bucket");
var objectPrefix = Console.ReadLine();


Console.WriteLine("Enter local path to output to");
var outputLocation = Console.ReadLine();
if (outputLocation is null || !Directory.Exists(outputLocation))
{
    throw new Exception("Output location provided doesn't exist in the system");
}


var listObjectsResponse = await s3Client.ListObjectsV2Async(new ListObjectsV2Request
{
    BucketName = s3Bucket,
    Prefix = objectPrefix
});

var tasks = new List<Task>();
foreach (var s3Object in listObjectsResponse.S3Objects) tasks.Add(ProcessObject(s3Object));
await Task.WhenAll(tasks);

Console.WriteLine("All done. Copy finished");

#region Util Methods

async Task ProcessObject(S3Object s3Object)
{
    string fullCopyPath = Path.Join(outputLocation,
        !string.IsNullOrEmpty(objectPrefix)
            ? s3Object.Key.Replace(objectPrefix, "")
            : s3Object.Key);

    CreateDirectoryForFileIfDoesntExist(new FileInfo(fullCopyPath));
    await CopyS3Object(s3Object, fullCopyPath);
}

void CreateDirectoryForFileIfDoesntExist(FileInfo fileInfo)
{
    if (!Directory.Exists(fileInfo.DirectoryName))
        // Automatically creates nested directories
        Directory.CreateDirectory(fileInfo.DirectoryName);
}

async Task CopyS3Object(S3Object s3Object, string path)
{
    var s3ObjectResponse = await s3Client.GetObjectAsync(s3Object.BucketName, s3Object.Key);
    await using var fileStream = File.Create(path);
    await s3ObjectResponse.ResponseStream.CopyToAsync(fileStream);
    Console.WriteLine($"Copied file: {path}");
}

#endregion