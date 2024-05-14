using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ReviveLauncher;
using ReviveLauncher.CheckSuspendedProcess;
using ReviveLauncher.Interop;
using ReviveLauncher.Pages;
using ReviveLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using WinUIEx;

namespace ReviveLauncher
{
    public class Fortnite
    {
        static bool ProcessCheckLoop(string processName)
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    return true;
                } 
                else
                {
                    return false;
                }
            }
        }

        static bool IsProcessSuspended(string processName)
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName(processName);

                foreach (var process in processes)
                {
                    IntPtr handle = process.Handle;
                    int suspendCount;
                    if (NativeMethodsProc.NtQueryInformationProcess(handle, 0x22, out suspendCount, sizeof(int), IntPtr.Zero) == 0 && suspendCount > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public static async Task Launch(string GamePath, string Email, string Password)
        {
            FontIcon icon = new FontIcon();
            icon.Glyph = "\uE783";
            icon.MinWidth = 80;
            icon.MaxWidth = 80;
            PlayPage.STATIC_MainLaunchCard.HeaderIcon = icon;
            try
            {
                KillFnProc();

                CloseEpicGames();

                await Task.Delay(5000); // this is to ensure everything is closed before installing the required files.
                if (!Directory.Exists(Definitions.RootDirectory + "\\Launcher"))
                {
                    Directory.CreateDirectory(Definitions.RootDirectory + "\\Launcher");
                }

                if (!File.Exists(Definitions.RootDirectory + "\\Launcher\\FortniteClient-Win64-Shipping_BE.exe"))
                    await DownloadFile(Definitions.BELauncherurl, Definitions.RootDirectory + "\\Launcher\\FortniteClient-Win64-Shipping_BE.exe");

                if (!File.Exists(Definitions.RootDirectory + "\\Launcher\\FortniteLauncher.exe"))
                    await DownloadFile(Definitions.Launcherurl, Definitions.RootDirectory + "\\Launcher\\FortniteLauncher.exe");

                    File.Delete(GamePath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                    await DownloadFile(Definitions.RedirectUrl, GamePath + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");

                Process.Start(new ProcessStartInfo()
                {
                    FileName = Definitions.RootDirectory + "\\Launcher\\FortniteLauncher.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                Process.Start(new ProcessStartInfo()
                {
                    FileName = Definitions.RootDirectory + "\\Launcher\\FortniteClient-Win64-Shipping_BE.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                Process Game = new Process();
                Process AntiCheat = new Process()
                {
                    StartInfo = new ProcessStartInfo(GamePath + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe")
                };
                AntiCheat.StartInfo.Arguments = "install \"ef7b6dadbcdf42c6872aa4ad596bbeaf\"";
                AntiCheat.StartInfo.UseShellExecute = false;
                AntiCheat.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                AntiCheat.Start();
                AntiCheat.WaitForExit();
                Game.StartInfo.FileName = GamePath + "\\ReviveClient-Win64-Shipping.exe";
                ProcessStartInfo startInfo = Game.StartInfo;
                startInfo.Arguments = $" -AUTH_LOGIN={Email} -AUTH_PASSWORD={Password} -AUTH_TYPE=epic -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fltoken=3db3ba5dcbd2e16703f3978d -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiYmU5ZGE1YzJmYmVhNDQwN2IyZjQwZWJhYWQ4NTlhZDQiLCJnZW5lcmF0ZWQiOjE2Mzg3MTcyNzgsImNhbGRlcmFHdWlkIjoiMzgxMGI4NjMtMmE2NS00NDU3LTliNTgtNGRhYjNiNDgyYTg2IiwiYWNQcm92aWRlciI6IkVhc3lBbnRpQ2hlYXQiLCJub3RlcyI6IiIsImZhbGxiYWNrIjpmYWxzZX0.VAWQB67RTxhiWOxx7DBjnzDnXyyEnX7OljJm-j2d88G_WgwQ9wrE6lwMEHZHjBd1ISJdUO1UVUqkfLdU5nofBQ -fromfl=eac";
                Game.StartInfo.RedirectStandardOutput = true;
                Game.StartInfo.UseShellExecute = false;
                Game.Start();
                PlayPage.STATIC_MainLaunchCard.Header = "Close Revive";
                PlayPage.STATIC_MainLaunchCard.Description = "Click here to close Revive. If you're experiencing issues, please restart your computer.";
                PlayPage.STATIC_MainLaunchCard.Content = null;

                FontIcon icon2 = new FontIcon();
                icon2.Glyph = "\uE8BB";
                PlayPage.STATIC_MainLaunchCard.HeaderIcon = icon2;

                Definitions.BindPlayButton = false;
                PlayPage.STATIC_MainLaunchCard.Click += STATIC_MainLaunchCard_CloseClicked;
                /*
                bool isclientrunning = ProcessCheckLoop("ReviveClient-Win64-Shipping");
                while (!isclientrunning)
                {
                    DialogService.ShowSimpleDialog("Failed to continue AntiCheat process was closed!", "Anti-Cheat");
                    break;
                }
                */
            }
            catch(Exception ex)
            {
                KillFnProc();
                DialogService.ShowSimpleDialog(ex, "Error");
                return;
            }
        }

        public static void STATIC_MainLaunchCard_CloseClicked(object sender, RoutedEventArgs e)
        {
            if (!Definitions.BindPlayButton)
            {
                try
                {
                    PlayPage.STATIC_MainLaunchCard.Header = "Launch Season 8";
                    PlayPage.STATIC_MainLaunchCard.Description = "Launch Fortnite Chapter 1 Season 8 powered by Revive";

                    FontIcon icon = new FontIcon();
                    icon.Glyph = "\uE768";
                    PlayPage.STATIC_MainLaunchCard.HeaderIcon = icon;

                    PlayPage.STATIC_MainLaunchCard.Tag = "Launch";
                    KillProcessByName("ReviveClient-Win64-Shipping");
                    KillProcessByName("FortniteClient-Win64-Shipping");
                    KillProcessByName("ReviveClient-Win64-Shipping");
                    KillProcessByName("FortniteClient-Win64-Shipping_EAC");
                    KillProcessByName("FortniteClient-Win64-Shipping_BE");
                    KillProcessByName("FortniteLauncher");
                    ((SettingsCard)sender).Content = null;
                    Definitions.BindPlayButton = true;
                }
                catch { }
            }
        }

        static void KillProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }
        public static void CloseEpicGames()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("EpicGamesLauncher");

                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        process.Kill();
                    }

                }
            }
            catch { }
        }

        public static void KillFnProc()
        {
            KillProcessByName("ReviveClient-Win64-Shipping");
            KillProcessByName("FortniteClient-Win64-Shipping");
            KillProcessByName("ReviveClient-Win64-Shipping");
            KillProcessByName("FortniteClient-Win64-Shipping_EAC");
            KillProcessByName("FortniteClient-Win64-Shipping_BE");
            KillProcessByName("FortniteLauncher");
        }
        static async Task DownloadFile(string URL, string path)
        {
            try
            {
                using (var Webclient = new WebClient())
                {
                    Webclient.DownloadFile(URL, path);
                }
            }
            catch (WebException ex)
            {
                DialogService.ShowSimpleDialog("Error while installing required files: " + ex.Message, "WebClient Error");
            }
        }
    }
}