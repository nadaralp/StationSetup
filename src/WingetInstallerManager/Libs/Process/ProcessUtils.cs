namespace WingetInstallerManager.Libs.Process;

using System.Diagnostics;

public class ProcessUtils
{
    public static void ExecuteCommand(string Command)
    {
        ProcessStartInfo ProcessInfo;
        ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
        ProcessInfo.CreateNoWindow = true;
        ProcessInfo.UseShellExecute = true;

        Process.Start(ProcessInfo);
    }
}