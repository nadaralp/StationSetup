// Schema for packages_spec.yml file

using YamlDotNet.Serialization;

namespace WingetInstallerManager.Libs.PackageInstaller;

public class PackagesSpec
{
    public string Description { get; set; }

    public IEnumerable<PackageInfo> Packages { get; set; }

    public static PackagesSpec Initialize()
    {
        var yamlDeserializer = new DeserializerBuilder()
            .Build();

        var yamlFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "packages_spec.yml");
        using var streamReader = new StreamReader(yamlFilePath);
        var yamlString = streamReader.ReadToEnd();
        var packageSpec = yamlDeserializer.Deserialize<PackagesSpec>(yamlString);
        return packageSpec;
    }
}

public class PackageInfo
{
    public string PackageName { get; set; }

    public IEnumerable<InstallationCommand> InstallationCommands { get; set; }
    
    public int Id { get; set; }
}

public class InstallationCommand
{
    public string Description { get; set; }
    
    public string Command { get; set; }
}