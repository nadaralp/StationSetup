using Amazon.S3;
using FolderNestedCopy.Services.S3;

var appSettings = await AppSettings.Configure();

// Console.WriteLine("Enter the folder path you want to copy", Color.Blue);
// var folderPathToCopy = Console.ReadLine();
// if (!Directory.Exists(folderPathToCopy))
// {
//     Console.WriteLine("Provided directory doesn't exist. Exiting application", Color.Red);
//     return;
// }
var folderPathToCopy = @"C:\Projects\StationSetup\StationSetup";



var bucketClient = new BucketClient(new AmazonS3Client());
bucketClient.AskForInputs();
await bucketClient.CreateBucketIfDoesntExist();


DirectoryContent directoryContent = new(folderPathToCopy, appSettings);
DirectoryIterator directoryIterator = new DirectoryIterator(directoryContent, appSettings);

var tasks = new List<Task>();
await directoryIterator.IterateFiles(IterationType.Recursive, (fileInfo) =>
{
        tasks.Add(bucketClient.PutFileToBucketAsync(fileInfo.FullName));
        return Task.CompletedTask;
});

await Task.WhenAll(tasks);

Console.WriteLine("All done. Uploaded successfully", Color.White);