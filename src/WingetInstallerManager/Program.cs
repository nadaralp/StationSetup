using WingetInstallerManager.Libs.Process;
using WingetInstallerManager.Libs.Winget;

var packagesSpec = PackagesSpec.Initialize();

Console.WriteLine("Choose package to install");
foreach (var packageInfo in packagesSpec.Packages)
{
    string packageOptionText = $"* {packageInfo.PackageName} ({packageInfo.Id})";
    Console.WriteLine(packageOptionText);
}

string chosenOption = Console.ReadLine();
if (!int.TryParse(chosenOption, out int chosenOptionInt))
{
    Console.WriteLine("Please choose by specifying the option ID in the parenthesis. For example: 1");
    return;
}

var packageToInstall = packagesSpec.Packages
    .FirstOrDefault(x => x.Id == chosenOptionInt);

if (packageToInstall is null)
{
    Console.WriteLine("Specified invalid ID for installation.");
    return;
}

var installationTasks = new List<Task>();
foreach (var installationCommand in packageToInstall.InstallationCommands)
{
    Task installationTask = Task.Run(() =>
    {
        Console.WriteLine(installationCommand.Description);
        ProcessUtils.ExecuteCommand(installationCommand.Command);
    });
    installationTasks.Add(installationTask);
}

await Task.WhenAll(installationTasks);