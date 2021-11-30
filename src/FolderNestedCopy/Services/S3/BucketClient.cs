using Amazon.S3;
using Amazon.S3.Model;

namespace FolderNestedCopy.Services.S3;

public class BucketClient
{
    private readonly IAmazonS3 _amazonS3;

    public BucketClient(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public string InputBucketName { get; private set; }
    
    public string FilesPrefix { get; private set; }

    public void AskForInputs()
    {
        Console.WriteLine("Enter the S3 bucket that you want to copy to? (doesn't have to exist)", Color.Blue);
        var s3Bucket = Console.ReadLine();
        if (s3Bucket is null)
        {
            throw new Exception("Empty bucket name is not a valid input");
        }
        InputBucketName = s3Bucket;
        
        Console.WriteLine("Choose a directory (doesn't have to exist) in which to store all the files in S3", Color.Blue);
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
            string fileKey = string.IsNullOrEmpty(FilesPrefix) ? file : $"{FilesPrefix}/{file}";
            using var streamReader = new StreamReader(file);
            var request = await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                Key = fileKey,
                BucketName = InputBucketName,
                TagSet = new() { new Tag { Key = "User", Value = Environment.UserName } },
                InputStream = streamReader.BaseStream
            });
            
            Console.WriteLine($"Uploaded file:{file}", Color.Green);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occured while tried to upload file {file}", Color.Red);
            Console.WriteLine(e.Message);
        }
        
    }
}