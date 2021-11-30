using Amazon.S3.Model;
using FolderNestedCopy.Libs.IO;

namespace FolderNestedCopy.Libs.S3;

public class BucketClient
{
    private readonly IAmazonS3 _amazonS3;

    public BucketClient(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public string InputBucketName { get; private set; } = default!;

    public string FilesPrefix { get; private set; } = "";

    public void AskForInputs()
    {
        Console.WriteLine("Enter the S3 bucket that you want to copy to? (doesn't have to exist)", Color.LightSkyBlue);
        var s3Bucket = Console.ReadLine();
        if (s3Bucket is null)
        {
            throw new Exception("Empty bucket name is not a valid input");
        }

        InputBucketName = s3Bucket;

        Console.WriteLine("Choose a directory in the bucket (doesn't have to exist) where to store the files",
            Color.LightSkyBlue);
        var filesPrefix = Console.ReadLine();
        FilesPrefix = filesPrefix ?? "";
    }

    public async Task<bool> DoesBucketExistAsync()
    {
        if (InputBucketName is null)
            throw new Exception($"Incorrect method invocation. {InputBucketName} doesn't exist");

        var doesExist = await _amazonS3.DoesS3BucketExistAsync(InputBucketName);
        return doesExist;
    }

    public async Task CreateBucketAsync()
    {
        if (InputBucketName is null)
            throw new Exception($"Incorrect method invocation. {InputBucketName} doesn't exist");

        var response = await _amazonS3.PutBucketAsync(InputBucketName);
    }

    public async Task CreateBucketIfDoesntExist()
    {
        if (await DoesBucketExistAsync())
        {
            Console.WriteLine($"{InputBucketName} bucket exists. Continuing", Color.Yellow);
            return;
        }

        Console.WriteLine($"{InputBucketName} doesn't exist... Creating bucket", Color.Yellow);
        await CreateBucketAsync();
        Console.WriteLine($"{InputBucketName} Created successfully", Color.GreenYellow);
    }

    public async Task PutFileToBucketAsync(string file)
    {
        try
        {
            
            string fileWithoutDiskPartition = FileHelper.RemoveDiskPartitionFromKey(file);
            string fileWithForwardSlashes = FileHelper.ReplaceBackSlashWithForwardSlash(fileWithoutDiskPartition);

            string filePathSanitized = fileWithForwardSlashes;
            string filePath = string.IsNullOrEmpty(FilesPrefix) ? filePathSanitized : $"{FilesPrefix}/{filePathSanitized}";
                
            using var streamReader = new StreamReader(file);
            var request = await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                Key =  filePath,
                BucketName = InputBucketName,
                TagSet = new() { new Tag { Key = "User", Value = Environment.UserName } },
                InputStream = streamReader.BaseStream
            });

            Console.WriteLine($"Uploaded file:{file}", Color.GreenYellow);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occured while tried to upload file {file}", Color.Red);
            Console.WriteLine(e.Message);
        }
    }


}