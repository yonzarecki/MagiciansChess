using MagiciansChessApp;
using MagiciansChessApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MagiciansChessApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
            

        }
   
        private void BtnGame_OnClick(object sender, RoutedEventArgs e)
        {
           
            Frame.Navigate(typeof (StartGamePage), vc);
        }

        private void BtnScoreBoard_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScoreBoard), vc);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Utils.EnableNavigateButton();
        }

        private static MagiciansChessAPI NewAPIClient()
        {
            var client = new MagiciansChessAPI(new Uri("https://magicianschessapi.azurewebsites.net"));
            // Uncomment following line and entire ServicePrincipal.cs file for service principal authentication of calls to ToDoListDataAPI
            //client.HttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", ServicePrincipal.GetS2SAccessTokenForProdMSA().AccessToken);
            return client;
        }
        // TODO - here we define all function for our controller, using the DataAPI client we defined above

    }
}
