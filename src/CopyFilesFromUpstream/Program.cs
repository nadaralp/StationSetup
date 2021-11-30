using Amazon.S3;
using CopyFilesFromUpstream.Libs;

var s3Client = new AmazonS3Client();
ICopyManager copyManager = new CopyManager(s3Client);
try
{
    CopySettings copySettings = copyManager.AskForUserInputs();
    await copyManager.CopyLocallyFromUpstreamAsync(copySettings);
}
catch (Exception e)
{
    Console.WriteLine("Application exited");
    Console.WriteLine(e.Message);
}
