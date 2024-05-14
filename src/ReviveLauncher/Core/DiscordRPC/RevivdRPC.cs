using DiscordRPC;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ReviveRPC
{
    private static DiscordRpcClient client;

    public static async void Start()
    {
        if (client == null)
        {
            client = new DiscordRpcClient("1143952474672201858");

            // Connect and start the bot
            client.Initialize();
        }

        if (client.IsInitialized)
        {
            if (Definitions.DiscordPRC == true)
            {
                if (Definitions.UserName == null)
                {
                    while (Definitions.UserName == null)
                    {
                        await Task.Delay(1000); // avoid freezing
                    }
                    UpdatePresence("Logged in as " + Definitions.UserName, "");
                }
            }
        }

    }

    public static void Stop()
    {
        if (client != null && client.IsInitialized)
        {
            client.ClearPresence();
            client.Deinitialize();
        }
    }

    public static void UpdatePresence(string state, string details)
    {
        if (client.IsInitialized)
        {
            client.SetPresence(new RichPresence()
            {
                State = state,
                Details = details,
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow // Use UtcNow to ensure consistent timestamps across time zones
                },
                Assets = new Assets()
                {
                    LargeImageKey = "revive",
                    LargeImageText = "Numbani",
                    SmallImageKey = "small-image",
                    SmallImageText = "Rogue - Level 100"
                },
                Buttons = new Button[]
                {
                new Button() { Label = "Join Revive Discord", Url = "https://discord.gg/Revivefn" }
                }
            });
        }
    }

}