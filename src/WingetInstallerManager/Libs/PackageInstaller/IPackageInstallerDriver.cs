namespace WingetInstallerManager.Libs.PackageInstaller;

public interface IPackageInstallerDriver
{
    /// <summary>
    /// Asks user for packages that he wishes to install
    /// </summary>
    /// <returns>The selected packages to install</returns>
    IEnumerable<PackageInfo> AskUserForPackagesToInstall();

    
    /// <summary>
    /// Requires user confirmation to install the packages.
    /// </summary>
    /// <exception cref="InvalidOperationException">When user doesn't confirm installation</exception>
    /// <param name="selectedPackagesToInstall">The selected packages to install</param>
    void RequireConfirmationOrThrow(IEnumerable<PackageInfo> selectedPackagesToInstall);


    /// <summary>
    /// Installs the selected packages
    /// </summary>
    /// <param name="selectedPackagesToInstall">The selected packages to install</param>
    /// <returns>Async</returns>
    Task InstallAsync(IEnumerable<PackageInfo> selectedPackagesToInstall);
}