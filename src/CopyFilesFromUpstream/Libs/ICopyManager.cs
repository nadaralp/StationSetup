namespace CopyFilesFromUpstream.Libs;

public interface ICopyManager
{
    /// <summary>
    ///     Asks for user S3 bucket, prefix(optional), and local output path.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when one of the inputs is invalid</exception>
    CopySettings AskForUserInputs();


    /// <summary>
    ///     Copies files from upstream locally
    /// </summary>
    /// <param name="copySettings">The copy settings to copy by</param>
    /// <returns>Task</returns>
    Task CopyLocallyFromUpstreamAsync(CopySettings copySettings);
}