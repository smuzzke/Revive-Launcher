using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Newtonsoft.Json.Linq;
using ReviveLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ReviveLauncher.Pages;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System.Threading;

public class FortniteFiles
{
    private static CancellationTokenSource progressCancellationTokenSource;
    public static async Task<string> CheckForMods(string FNPath)
    {
        if (Directory.Exists(FNPath))
        {
            if (Directory.Exists(Path.Combine(FNPath, "FortniteGame", "Content", "Paks")))
            {
                string directoryPath = Path.Combine(FNPath, "FortniteGame", "Content", "Paks");

                string[] expectedFiles = new string[]
                {
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk0-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk2-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk5-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk7-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk8-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk9-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s1-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s2-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s3-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s4-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s5-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s6-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s7-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10_s8-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk10-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk11_s1-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk11-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1000-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1001-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1002-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1003-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1004-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1005-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1006-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1007-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1008-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1008-WindowsClient.sig"),
                    Path.Combine(directoryPath, "pakchunk1009-WindowsClient.pak"),
                    Path.Combine(directoryPath, "pakchunk1009-WindowsClient.sig"),
                };
                List<string> missingFiles = expectedFiles
                .Where(fileName => !File.Exists(Path.Combine(directoryPath, fileName)))
                .ToList();
                long totalExpectedFileSize = expectedFiles
                .Where(fileName => File.Exists(Path.Combine(directoryPath, fileName)))
                .Sum(fileName => new FileInfo(Path.Combine(directoryPath, fileName)).Length);
                const long desiredTotalSizeInBytes = 49129162723L; // idk if its different for everyone but this is the size of my 8.51 paks. if you modify the value make sure to leave the 'L' at the end!!

                if (totalExpectedFileSize != desiredTotalSizeInBytes)
                {
                    DialogService.ShowSimpleDialog($"Please verify your Fortnite files and ensure you have selected your 8.51 build.", "Incorrect Files");
                    return "Error";
                }
                else if (missingFiles.Count > 7)
                {
                    DialogService.ShowSimpleDialog($"Many files are missing please make sure you have selected a 8.51 build.", "Missing Files");
                    return "Error";
                }
                else if (missingFiles.Count > 0)
                {
                    DialogService.ShowSimpleDialog($"Some files are missing: {string.Join(", ", missingFiles)}. Please make sure you have selected a 8.51 build and have verified your Fortnite files then attempt relaunching.", "Missing Files");
                    return "Error";
                }


                string[] files = Directory.GetFiles(directoryPath);

                List<string> unexpectedFiles = new List<string>();

                foreach (string file in files)
                {
                    if (!Array.Exists(expectedFiles, expectedFile => expectedFile.Equals(file)))
                    {
                        unexpectedFiles.Add(file);
                    }
                }

                if (unexpectedFiles.Count > 0)
                {
                    bool result = await DialogService.YesOrNoDialog($"Revive does not support playing with modded paks. Would you like to remove the listed files? {Environment.NewLine} {string.Join(", ", unexpectedFiles)}", "Modified Client");
                    if (result == true)
                    {
                        try
                        {
                            foreach (string unexpectedFile in unexpectedFiles)
                            {
                                File.Delete(unexpectedFile);
                            }
                        }
                        catch { }

                    }
                    return "Error";
                }
                else
                {
                    return "Success";
                }
            }
            else
            {
                DialogService.ShowSimpleDialog("Failed to launch Fortnite. Paks directory not found.", "Error");
                return "Error";
            }
        }
        else
        {
            DialogService.ShowSimpleDialog("Fortnite path is empty. Please check your settings and try again.", "Error");
            return "Error";
        }
    }
    public static async Task<bool> VerifyClient(string FNPath)
    {
        try
        {
            string[] requiredPaths = new string[]
            {
                Path.Combine(FNPath, "ReviveClient-Win64-Shipping.exe"),
                Path.Combine(FNPath, "EasyAntiCheat", "Certificates"),
                Path.Combine(FNPath, "EasyAntiCheat", "Licenses"),
                Path.Combine(FNPath, "EasyAntiCheat", "Localization"),
                Path.Combine(FNPath, "EasyAntiCheat", "EasyAntiCheat_EOS_Setup.exe"),
                Path.Combine(FNPath, "EasyAntiCheat", "Settings.json"),
                Path.Combine(FNPath, "EasyAntiCheat", "SplashScreen.png")
            };

            foreach (string path in requiredPaths)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    await PatchClient(FNPath);
                    EndPatchingProgress();
                    return true;
                }
            }

            File.Delete(FNPath + "\\EasyAntiCheat\\SplashScreen.png");
            File.Delete(FNPath + "\\EasyAntiCheat\\Settings.json");

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Definitions.ReviveSplashScreen, FNPath + "\\EasyAntiCheat\\SplashScreen.png");
                client.DownloadFile(Definitions.ReviveSettingsJson, FNPath + "\\EasyAntiCheat\\Settings.json");
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Error while verifying files: " + ex.Message, "Error");
            return false;
        }

        return true;
    }

    static async Task DownloadEACPatch(string FNPath)
    {
        try
        {
            string url = Definitions.AntiCheatUrl;
            string downloadPath = Definitions.RootDirectory + "\\EAC";

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            string extractionPath = FNPath;

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, Path.Combine(downloadPath, "EasyAntiCheat.zip"));
            }

            ZipFile.ExtractToDirectory(Path.Combine(downloadPath, "EasyAntiCheat.zip"), extractionPath);
        }
        catch (WebException ex)
        {
            DialogService.ShowSimpleDialog("Failed to download required files: " + ex.Message, "Webclient Error");
            return;
        }
    }
    static async Task StartPatchingProgressAsync()
    {
        progressCancellationTokenSource = new CancellationTokenSource();
        await UpdateDescriptionWithPatchingProgressAsync(progressCancellationTokenSource.Token);
    }

    static void EndPatchingProgress()
    {
        // Cancel the progress update if it's still running
        progressCancellationTokenSource?.Cancel();
    }

    static async Task UpdateDescriptionWithPatchingProgressAsync(CancellationToken cancellationToken)
    {
        string[] progressIndicators = { ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "...", ".", "..", "..." };

        for (int i = 0; i < progressIndicators.Length; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // If cancellation is requested, exit the loop
                break;
            }

            PlayPage.STATIC_MainLaunchCard.Description = "Downloading required files" + progressIndicators[i];
            await Task.Delay(1000);
        }

        // Optionally, you can set the description to something else or clear it when the loop ends.
        // PlayPage.STATIC_MainLaunchCard.Description = "Patching complete"; // Set a final message
        // PlayPage.STATIC_MainLaunchCard.Description = string.Empty; // Clear the description
    }
    public static async Task<string> PatchClient(string FNPath)
    {
        try
        {
            ProgressRing loading = new ProgressRing();
            loading.IsIndeterminate = true;
            loading.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
            loading.HorizontalAlignment = HorizontalAlignment.Center;

            StartPatchingProgressAsync();

            PlayPage.STATIC_MainLaunchCard.Content = loading;
            await Task.Delay(1000);

            KillProcessByName("FortniteClient-Win64-Shipping");
            KillProcessByName("FortniteClient-Win64-Shipping_EAC");
            KillProcessByName("ReviveClient-Win64-Shipping");
            KillProcessByName("FortniteClient-Win64-Shipping_BE");
            KillProcessByName("FortniteLauncher");

            if (Directory.Exists(FNPath + "\\EasyAntiCheat"))
            {
                Directory.Delete(FNPath + "\\EasyAntiCheat", true);
            }

            if (File.Exists(FNPath + "\\ReviveClient-Win64-Shipping.exe"))
            {
                File.Delete(FNPath + "\\ReviveClient-Win64-Shipping.exe");
            }

            await DownloadEACPatch(FNPath);
            string downloadPath = Definitions.RootDirectory + "\\EAC";

            if (Directory.Exists(downloadPath))
            {
                Directory.Delete(downloadPath, true);
            }

            if (File.Exists(downloadPath + "\\EasyAntiCheat.zip"))
            {
                File.Delete(downloadPath + "\\EasyAntiCheat.zip");
            }
            return "Success";
        }
        catch (WebException ex)
        {
            DialogService.ShowSimpleDialog("An error occurred while installing zip: " + ex.Message, "Error");
            return "Error";
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
}