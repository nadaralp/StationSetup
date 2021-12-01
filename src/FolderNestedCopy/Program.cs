var appSettings = await AppSettings.Configure();

Console.WriteLine("Enter the folder path you want to copy", Color.LightSkyBlue);
var userGivenFolderPathToCopy = Console.ReadLine();
if (!Directory.Exists(userGivenFolderPathToCopy))
{
    Console.WriteLine("Provided directory doesn't exist. Exiting application", Color.Red);
    return;
}

var bucketClient = new BucketClient(new AmazonS3Client());
bucketClient.AskForInputs();
await bucketClient.CreateBucketIfDoesntExist();

DirectoryContent directoryContent = new(userGivenFolderPathToCopy, appSettings);
DirectoryIterator directoryIterator = new DirectoryIterator(directoryContent, appSettings);

List<Task> tasks = new();
await directoryIterator.IterateFiles(IterationType.Recursive, (fileInfo) =>
{
    tasks.Add(bucketClient.PutFileToBucketAsync(fileInfo.FullName, userGivenFolderPathToCopy));
    return Task.CompletedTask;
});
await Task.WhenAll(tasks);

Console.WriteLine("All done. Uploaded successfully", Color.White);