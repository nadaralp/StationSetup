using WingetInstallerManager.Libs.Process;

namespace WingetInstallerManager.Libs.PackageInstaller;

public class PackageInstallerDriver : IPackageInstallerDriver
{
    private readonly PackagesSpec _packagesSpec;

    public PackageInstallerDriver(PackagesSpec packagesSpec)
    {
        _packagesSpec = packagesSpec;
    }

    public IEnumerable<PackageInfo> AskUserForPackagesToInstall()
    {
        Console.WriteLine(
            "Choose package to install by specifying the package ID. You can choose multiple by separating with commas: example: 1,2,3");

        foreach (var packageInfo in _packagesSpec.Packages)
        {
            string packageOptionText = $"* {packageInfo.PackageName} ({packageInfo.Id})";
            Console.WriteLine(packageOptionText);
        }

        string chosenPackagesText = Console.ReadLine();
        var chosenPackagesId = chosenPackagesText
            .Split(",")
            .Select(x => x.Trim())
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse);

        var selectedPackagesToInstall = _packagesSpec.Packages
            .Where(x => chosenPackagesId.Any(chosenId => chosenId == x.Id));

        if (!selectedPackagesToInstall.Any())
        {
            throw new Exception("Didn't select any packages. Stopping");
        }

        return selectedPackagesToInstall;
    }

    public void RequireConfirmationOrThrow(IEnumerable<PackageInfo> selectedPackagesToInstall)
    {
        string selectedPackagesCommaSeparated = string.Join(',', selectedPackagesToInstall.Select(x => x.PackageName));
        Console.WriteLine($"Selected packages: {selectedPackagesCommaSeparated}");
        Console.WriteLine("Do you wish to continue? Y/n");
        string doesWantToContinue = Console.ReadLine()?.ToLower() ?? "n";
        if (doesWantToContinue != "y")
        {
            throw new Exception("User request abortion");
        }
    }

    public async Task InstallAsync(IEnumerable<PackageInfo> selectedPackagesToInstall)
    {
        var tasks = new List<Task>();
        foreach (PackageInfo packageToInstall in selectedPackagesToInstall)
        {
            Console.WriteLine($"Installing: {packageToInstall.PackageName}");
            tasks.Add(InstallPackage(packageToInstall));
        }

        await Task.WhenAll(tasks);
    }

    private static async Task InstallPackage(PackageInfo packageToInstall)
    {
        var installations = new List<Task>();
        foreach (var installationCommand in packageToInstall.InstallationCommands)
        {
            Task installationTask = Task.Run(() =>
            {
                Console.WriteLine(installationCommand.Description);
                ProcessUtils.ExecuteCommand(installationCommand.Command);
            });
            installations.Add(installationTask);
        }

        await Task.WhenAll(installations);
    }
}