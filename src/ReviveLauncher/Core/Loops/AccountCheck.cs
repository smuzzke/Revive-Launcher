using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using ReviveLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml;
using ReviveLauncher.Services;
using System.Runtime.CompilerServices;

namespace ReviveLauncher.Core.Loops
{
    public class AccountCheck
    {
        public static async Task CheckAccount()
        {
            try
            {
                while (true)
                {
                    MongoDB.User user = new MongoDB.User();
                    if (user != null)
                    {
                        if (user.banned)
                        {
                            DialogService.ShowSimpleDialog("You have been banned.", "Error");
                            ReviveLauncher.Fortnite.KillFnProc();

                            await Task.Delay(250);

                            MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                        }
                    } else
                    {
                        DialogService.ShowSimpleDialog("Error user is null", "Error");
                        ReviveLauncher.Fortnite.KillFnProc();
                        await Task.Delay(250);

                        MainWindow.ShellFrame.Navigate(typeof(LoginPage));
                    }
                    await Task.Delay(1000); 
                }
            } catch { }
        }
    }
}
