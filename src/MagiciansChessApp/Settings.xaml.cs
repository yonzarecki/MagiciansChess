using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking;
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        StreamSocket socket;
        StreamSocketListener listener;
        DataReader reader;
        string port = "8001";
        HostName host;

        bool ConnectedToServer { get; set; }
        public Settings()
        {
            socket = new StreamSocket();
            ConnectToServer();
            host = new HostName("192.168.12.171");
            this.InitializeComponent();
        }
        private void btn_BTConnect_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btn_shutMusicDown_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            vc = e.Parameter as VolumeControl;
        }

        private async void ConnectToServer()
        {
            await socket.ConnectAsync(new Windows.Networking.HostName("192.168.12.171"), "8001");
            
            ConnectedToServer = true;
            btn_Send.IsEnabled = true;
            
        }

        private async void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            if (!ConnectedToServer)
            {
                ConnectToServer();
            }

            DataWriter writer = new DataWriter(socket.OutputStream);
            writer.WriteString(tb_Msg.Text);
            await writer.StoreAsync();

            listenToPackets();
            
        }

        private async void listenToPackets()
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
                    Utils.Show(new Windows.UI.Popups.MessageDialog(s), new List<UICommand> { new UICommand("Close") });
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

        private /*async*/ void sendStringTCP()
        {
            //    try
            //    {


            //        TcpClient tcpclnt = new TcpClient();
            //        Console.WriteLine("Connecting.....");

            //        tcpclnt.Connect("172.21.5.99", 8001);
            //        // use the ipaddress as in the server program

            //        Console.WriteLine("Connected");
            //        Console.Write("Enter the string to be transmitted : ");

            //        String str = Console.ReadLine();
            //        Stream stm = tcpclnt.GetStream();

            //        ASCIIEncoding asen = new ASCIIEncoding();
            //        byte[] ba = asen.GetBytes(str);
            //        Console.WriteLine("Transmitting.....");

            //        stm.Write(ba, 0, ba.Length);

            //        byte[] bb = new byte[100];
            //        int k = stm.Read(bb, 0, 100);

            //        for (int i = 0; i < k; i++)
            //            Console.Write(Convert.ToChar(bb[i]));

            //        tcpclnt.Close();
            //    }

            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error..... " + e.StackTrace);
            //    }
            //}
        }

       
    }
}
