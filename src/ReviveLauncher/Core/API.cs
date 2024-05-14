using Microsoft.UI.Xaml;
using ReviveLauncher.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class API
{
    public static string apiUrl = Definitions.APIurl;
    private static string accessToken = Definitions.accessToken;

    private static HttpClient CreateHttpClient()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return httpClient;
    }
    public static async Task<string> Connect()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "Test");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        return "Success";
                    }
                    else
                    {
                        DialogService.ShowSimpleDialog("Gotten invalid data from the API please try again later.", "API Error");
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("API connection failed please check your internet connection and try again.", "API Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> Version()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "Version"); // the rest of the API URL goes in ""

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    if (content != null)
                    {
                        if (content == "Invalid authorization token")
                        {
                            return null;
                        }
                        return content;
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to get latest version please try again later.", "API Error");
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
        return "Error";
    }

    public static async Task<string> BackendStatus()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "BackendStatus");
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // log the repsonce and log the content
                    if (content == "Success")
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("Failed to check Backend status please try again later.", "API Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> GameServerStatus()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "GameServerStatus");// not working
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    if (content == "Success")
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    DialogService.ShowSimpleDialog("GameServer Status is unavailable please try again later.", "API Error");
                    return "Error";
                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.ConnectionFailedToAPI = true;
            Definitions.FinishedAPIRequest = true;
            return "Error";
        }
    }

    public static async Task<string> MongoDBConntectionString()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetConnectionString?Key={await GetKey()}");
                string content = await response.Content.ReadAsStringAsync();
                // Check if the request was successful
                if (content == "key not found." || content == "Invalid authorization token")
                {
                    DialogService.ShowSimpleDialog("Revive Launcher is outdated please update to continue using Revive Launcher.", "Authentication failed");
                } else
                {
                return content;
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
    }
    public static async Task<string> GetKey()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl + $"GetKey");
                string content = await response.Content.ReadAsStringAsync();
                if (content == "Invalid authorization token")
                {
                    return null;
                }
                return content;
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
    }
    // get in app notifications from server
    /*
    public static void StartClient()
    {
        TcpClient client = null;
        try
        {
            string serverAddress = "127.0.0.1"; // Replace with the server IP address
            int port = 3001; // Match the port used in the server

            client = new TcpClient(serverAddress, port);

            byte[] data = new byte[256];
            StringBuilder response = new StringBuilder();
            NetworkStream stream = client.GetStream();

            int bytesRead;
            while ((bytesRead = stream.Read(data, 0, data.Length)) > 0)
            {
                response.Append(Encoding.ASCII.GetString(data, 0, bytesRead));
            }

            DialogService.ShowSimpleDialog("Server response: " + response.ToString(), "Success");
        } catch { }
    }
    */
    public static async Task<string> GetSkin()
    {
        try
        {
            using (HttpClient httpClient = CreateHttpClient())
            {
                // Send the GET request and receive the response
                while (Definitions.Email == null)
                {
                    await Task.Delay(1000); // avoid freezing
                }

                if (Definitions.Password != null)
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "GetSkin?username=" + Definitions.UserName);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();
                        if (content == "No username provided.")
                        {
                            DialogService.ShowSimpleDialog("Failed to get current skin username is null.", "API Error");
                        } else if (content == "User not found." || content == "Invalid authorization token")
                        {
                            DialogService.ShowSimpleDialog("Authentication failed: cannot authenticate client.", "API Error");
                        }
                        else 
                        {
                            if (!string.IsNullOrEmpty(content) && content.StartsWith("AthenaCharacter:"))
                            {
                                string trimmedValue = content.Substring("AthenaCharacter:".Length);
                                string url = "http://fortnite-api.com/images/cosmetics/br/" + trimmedValue + "/smallicon.png";

                                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri validUri))
                                {
                                    url = "http://fortnite-api.com/images/cosmetics/br/CID_001_Athena_Commando_F_Default/smallicon.png";
                                }
                                DialogService.ShowSimpleDialog(url, "URL");
                                return url;
                            }

                            //return "Error";
                        }
                    }
                    else
                    {
                        // this is annoying
                        // DialogService.ShowSimpleDialog("Failed to get current skin.", "API Error");
                    }

                }
            }
        }
        catch (Exception ex)
        {
            DialogService.ShowSimpleDialog("Connection to Revive API unsuccessful. Please review your firewall and anti-virus settings, then attempt the connection again.", "API Error");
            //DialogService.ShowSimpleDialog($"An error occurred: {ex.Message}", "API Error");
            Definitions.FinishedAPIRequest = true;
            Definitions.ConnectionFailedToAPI = true;
            return "Error";
        }
        return "Error";
    }
}
