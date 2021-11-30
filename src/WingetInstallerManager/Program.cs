using WingetInstallerManager.Libs.PackageInstaller;
using WingetInstallerManager.Libs.Process;

PackagesSpec packagesSpec = PackagesSpec.Initialize();
IPackageInstallerDriver packageInstallerDriver = new PackageInstallerDriver(packagesSpec);

try
{
    var selectedPackagesToInstall = packageInstallerDriver.AskUserForPackagesToInstall().ToList();
    packageInstallerDriver.RequireConfirmationOrThrow(selectedPackagesToInstall);
    await packageInstallerDriver.InstallAsync(selectedPackagesToInstall);
}
catch (Exception e)
{
    Console.WriteLine("Application exited.");
    Console.WriteLine(e.Message);
}

