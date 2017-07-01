using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Networking;
using Windows.Networking.Connectivity;
using System.Collections.ObjectModel;

namespace MagiciansChessApp
{
    public class Session
    {
        public Session(bool byScore, string playerName)
        {
            ByScore = byScore;
            PlayerName = playerName;
        }

        public Session(Session other)
        {
            ByScore = other.ByScore;
            PlayerName = other.PlayerName;
        }

        public bool ByScore { get; set; }

        public bool ByTime
        {
            get { return !ByScore; }
            set { ByScore = !value; }
        }

        public string PlayerName { get; set; }
    }
    
    public static class Utils
    {
        private static Windows.UI.Popups.IUICommand msgResponseAsyncOperation { get; set; }
        public static async void Show(MessageDialog msgDialog, List<UICommand> commands, uint defaultCommandIndex = 0,
            uint cancelCommandIndex = 0)
        {
            var message = msgDialog;
            foreach (UICommand command in commands)
            {
                message.Commands.Add(command);
            }
            message.DefaultCommandIndex = defaultCommandIndex;
            message.CancelCommandIndex = cancelCommandIndex;
            msgResponseAsyncOperation = await message.ShowAsync();
        }

        public static Windows.UI.Popups.IUICommand getMsgResponse()
        {
            return msgResponseAsyncOperation;
        }
        public static void EnableNavigateButton()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            string myPages = "";
            foreach (PageStackEntry page in rootFrame.BackStack)
            {
                myPages += page.SourcePageType.ToString() + "\n";
            }
            //stackCount.Text = myPages;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public static async Task<int> AddTimeLimitedGameAsync(string name, DateTime date, int playerScore, int robotScore)
        {
            // TODO change
            var a = 5*5;
            return a;
        }

        public static MagiciansChessAPI NewAPIClient()
        {
            var client = new MagiciansChessAPI(new Uri(App.apiUrl));
            // Uncomment following line and entire ServicePrincipal.cs file for service principal authentication of calls to ToDoListDataAPI
            //client.HttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", ServicePrincipal.GetS2SAccessTokenForProdMSA().AccessToken);
            return client;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var col = new ObservableCollection<T>();
            foreach (var cur in enumerable)
            {
                col.Add(cur);
            }
            return col;
        }

    }

    public class ChessGameManager
    {
        public static ChessLibrary.Game initializeGame()
        {
            return new ChessLibrary.Game();
        }

        public static async Task<int> GetBestMove(ChessLibrary.Game g)
        {
            using (var client = Utils.NewAPIClient())
            {
                //ChessAIExtensions.GetBestMoveByGamexmlAndPlayercolorAndTimelimitinsecs(new ChessAI(client), 
                //    g.XMl);
            }
            return 55;
        }

        public static async void sendMoveToBoard(String move)
        {
            await App.bluetoothManager.Send(move);
        }
    }
}