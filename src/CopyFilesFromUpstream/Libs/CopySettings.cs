namespace CopyFilesFromUpstream.Libs;

public record CopySettings(string BucketName, string ObjectsPrefix, string LocalOutputDirectory);