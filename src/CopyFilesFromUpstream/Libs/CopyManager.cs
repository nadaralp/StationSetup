namespace CopyFilesFromUpstream.Libs;

public record CopySettings(string BucketName, string ObjectsPrefix, string LocalOutputDirectory);


public interface ICopyManager
{
    /// <summary>
    /// Asks for user S3 bucket, prefix(optional), and local output path.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when one of the inputs is invalid</exception>
    CopySettings AskForUserInputs();


    /// <summary>
    /// Copies files from upstream locally
    /// </summary>
    /// <param name="copySettings">The copy settings to copy by</param>
    /// <returns>Task</returns>
    Task CopyLocallyFromUpstream(CopySettings copySettings);
}


public class CopyManager : ICopyManager
{
    public CopySettings AskForUserInputs()
    {
        throw new NotImplementedException();
    }

    public Task CopyLocallyFromUpstream(CopySettings copySettings)
    {
        throw new NotImplementedException();
    }
}