using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HockeyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnlyBT : Page
    {
        private TimeSpan current = TimeSpan.Zero;
        private DispatcherTimer timer;
        private bool btnConnectPressed { get; set; }
        public OnlyBT()
        {
            this.InitializeComponent();
           
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            current += TimeSpan.FromMilliseconds(500);
            tb_Msg.Text = "Time : " + current.TotalSeconds;
            //if(Bluetooth.strings != null && Bluetooth.strings.Count > 0)
            //{
                //tb_Msg.Text = Bluetooth.strings[0];
                //Bluetooth.strings.RemoveAt(0);
                timer.Stop();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += Timer_Tick;
                timer.Start();
            //}
        }

        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (!btnConnectPressed)
            {
                Bluetooth.ConnectToBoard();
                btnConnectPressed = true;
            }
        }
    }
}
