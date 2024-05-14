using MongoDB.Bson.IO;
using ReviveLauncher;
using System;

internal class Definitions
{
    // main
    public static string CurrentVersion = "1.0.0";
    public static string APIurl = "http://34.154.97.248:3001/v1/";
    public static string RootDirectory = Environment.GetEnvironmentVariable("LocalAppData") + "\\ReviveLauncher";
    public static string BELauncherurl = "https://cdn.discordapp.com/attachments/1146197920169857044/1146204255028510791/FortniteClient-Win64-Shipping_BE.exe";
    public static string Launcherurl = "https://cdn.discordapp.com/attachments/1146197920169857044/1146204254592311456/FortniteLauncher.exe";
    public static string GFSDKUrl = "https://drive.google.com/u/0/uc?id=1YAdGQzhWG5OBCjMX7bDn_1JjCD0oZipL&export=download";
    public static string RedirectUrl = "https://drive.google.com/uc?export=download&id=1YAdGQzhWG5OBCjMX7bDn_1JjCD0oZipL";
    public static string AntiCheatUrl = "https://drive.google.com/u/0/uc?id=1ALkyQn4qg8Gc6g_w4pv5ISfkYWnnhxXy&export=download";
    public static string ReviveSplashScreen = "https://cdn.discordapp.com/attachments/1146197920169857044/1146206353078091856/SplashScreen.png";
    public static string ReviveSettingsJson = "https://cdn.discordapp.com/attachments/1146197920169857044/1146206534674677760/Settings.json";
    public static string APIversion = null;
    public static string UserName = null;
    public static string FortnitePath = null;
    public static string Email = null;
    public static string Password = null;
    public static bool EOR = false;
    public static bool DiscordPRC = true;
    public static bool LoggedOut = false;
    public static bool RefreshToken = false;
    public static bool ForceClose = false;
    public static bool outdated = false;
    public static bool FinishedAPIRequest = false;
    public static bool BindPlayButton = true;
    public static bool ConnectionFailedToAPI = false;

    // options
    public static bool limitguestfeatures = true; // set to false to enable all features for guests.
    public static bool AllowLaunchingAnyVersion = false; // false is limited to 8.51 if set to true it will allow launching any version 12.41 is the highest version tested.

    //secrets
    public static string accessToken = "31811a3e1afdb125efa4151438f29928a5dcf6b403c13f0a2dde440bca545654";
}
