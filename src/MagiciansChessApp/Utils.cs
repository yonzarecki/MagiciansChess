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

    /*
     * Server for accessing the board through the WiFi module.
     */ 
    public class Server
    {
        public enum Command
        {
            START = 'S',
            TERMINATE = 'T',
            GOAL_ROBOT = 'R',
            GOAL_PLAYER = 'P',
            EMPTY = 'E',
            ALIVE = 'A'
        }

        static StreamSocket socket;
        static DataReader reader;
        static DataWriter writer;
        static string port = "8001";
        static string ip = "192.168.8.250";
        static HostName host;
        public static bool Paused { get; set; }
        public delegate void ScoreUpdator(bool UserScored);
        static ScoreUpdator UpdateScore;
        public static bool Connected { get; set; }

        static Server()
        {
            Connected = false;
        }
        public static void Initiate(ScoreUpdator updatorFunc)
        {
            if (Connected)
            {
                Dispose();
            }
            socket = new StreamSocket();
            host = new HostName(ip);
            UpdateScore = updatorFunc;
            Connected = false;
        }
        public static async Task listenToPackets()
        {
            //await listener.BindServiceNameAsync(port);

            try
            {
                reader = new DataReader(socket.InputStream);
                while (true) //TODO: while we didn't end game
                {
                    reader.InputStreamOptions = InputStreamOptions.Partial;
                    uint income = await reader.LoadAsync(sizeof(uint));
                    string s = reader.ReadString(income);
                    if (Paused)
                    {
                        continue;
                    }

                    switch (s)
                    {
                        case "R":
                            UpdateScore(false);
                            break;
                        case "P":
                            UpdateScore(true);
                            break;
                        default:
                            break;
                    }
                    if (!Connected)
                    {
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
        }
        public static async Task<bool> ConnectToServer(Frame current)
        {

            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();

            //if (connectionProfile == null || !connectionProfile.IsWlanConnectionProfile)
            //{
            //    MessageDialog msg = new MessageDialog("Please Enable WiFi", "Wifi Connection");
            //    UICommand OK = new UICommand("OK");
            //    Utils.Show(msg, new List<UICommand>() { OK });
            //    current.Navigate(typeof(MainMenu), null);
            //    return false;
            //}
            await socket.ConnectAsync(host, port);
            Connected = true;
            return true;
        }
        public static async void SendToServer(Command c,Frame current)
        {
            //while (!connected) { }
            try {
                writer = new DataWriter(socket.OutputStream);
                writer.WriteByte(Convert.ToByte((char)c));
                await writer.StoreAsync();
            } catch(Exception ex)
            {
                //MessageDialog msgDialog = new MessageDialog("The Connection Has Been Closed.");
                //UICommand OK = new UICommand("OK");
                //OK.Invoked += (IUICommand command) =>
                //{
                //    Server.Dispose(); current.Navigate(typeof(MainMenu), null);
                //};

                //Utils.Show(msgDialog, new List<UICommand> { OK });
            }
            
        }
        public static void Dispose()
        {
            socket.Dispose();
            Connected = false;
        }

    }
    
    public class Utils
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

    }
}