using CommunityToolkit.Labs.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ReviveLauncher.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static ReviveLauncher.Services.NavigationService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ReviveLauncher.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminPanel : Page
    {
        public static bool BypassEac = false;
        public static bool ArCheat = false;
        public static bool FixMemoryLeak = false;
        public AdminPanel()
        {
            this.InitializeComponent();
        }
        private void BypassEAC(object sender, RoutedEventArgs e)
        {
            BypassEac = true;
        }
        private void ARCheat(object sender, RoutedEventArgs e)
        {
            ArCheat = true;
        }
        private void memoryleak(object sender, RoutedEventArgs e)
        {
            FixMemoryLeak = true;
        }
    }
}
