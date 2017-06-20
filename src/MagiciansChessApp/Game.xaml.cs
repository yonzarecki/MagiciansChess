using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.System.Display;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MagiciansChessApp
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        private TimeSpan second = TimeSpan.Zero;
        private const int SCORE_LIMIT = 5;
        bool connected = false;
        bool paused = false;
        DisplayRequest request;

        private Server.Command command = Server.Command.EMPTY;

        private Session Params { get; set; }
        private bool Countdown { get { return Params.ByTime; } }
        
        public Game()
        {
            Server.Initiate(UpdateScore);
            request = new DisplayRequest();
        }

        private string showTime()
        {
            return TimeSpaner.ToString().Substring(3); // format is MM:SS
        }

        private void stopTimer()
        {
            if (Timer.IsEnabled && TimeSpaner != TimeSpan.Zero) {Timer.Stop();}
        }

        private void resumeTimer()
        {
            if(!Timer.IsEnabled && TimeSpaner != TimeSpan.Zero) { Timer.Start();}
        }

        private void DispatcherTimer_Tick(object sender, object eo)
        {
            if(command != Server.Command.EMPTY)
            {
                switch (command)
                {
                    case Server.Command.START: //souldn't get here
                        break;
                    case Server.Command.TERMINATE:
                        stopGame(false);
                        break;
                    case Server.Command.GOAL_ROBOT:
                        UpdateScore(false);           
                        break;
                    case Server.Command.GOAL_PLAYER:
                        UpdateScore(true);
                        break;
                    case Server.Command.EMPTY: //souldn't get here
                        break;
                }

                command = Server.Command.EMPTY;
            }
            second += TimeSpan.FromMilliseconds(10);
            if(second < TimeSpan.FromMilliseconds(400))
            {
                return;
            }

            second = TimeSpan.Zero;

            if (Countdown) // by time : 
            {
                TimeSpaner -= TimeSpan.FromSeconds(1);
            }

            if (!Countdown) // by score : 
            {
                TimeSpaner += TimeSpan.FromSeconds(1);
            }

            tb_Timer.Text = showTime();
            if (Countdown && TimeSpaner == TimeSpan.FromSeconds(0))
            {
                stopGame(false);
            }
            
        }

        private void UpdateScore(bool UserScored)
        {
            int userScore = int.Parse(tb_UserScore.Text);
            int robotScore = int.Parse(tb_RobotScore.Text);

            if (UserScored)
            {
                tb_UserScore.Text = (userScore + 1).ToString();
            }

            if (!UserScored)
            {
                tb_RobotScore.Text = (robotScore + 1).ToString();
            }

            if(Params.ByScore && (userScore == SCORE_LIMIT || robotScore == SCORE_LIMIT))
            {
                stopGame(false);
            }
        }

        private void Popup_OK_Invoked(IUICommand command)
        {
            //TODO: Insert new score to the DB
            Server.SendToServer(Server.Command.TERMINATE,Frame);
            Server.Dispose();
            Frame.Navigate(typeof(ScoreBoard), Params);
        }
        private void stopGame(bool navigation)
        {
            Timer.Stop();
            Server.SendToServer(Server.Command.TERMINATE,Frame);

            if (!navigation)
            {
                int playerScore = int.Parse(tb_UserScore.Text);
                int robotScore = int.Parse(tb_RobotScore.Text);
                if (Params.ByTime)
                {
                    Utils.viewModel.AddTimeLimitedGameAsync(Params.PlayerName, DateTime.Now,playerScore,robotScore);
                }

                if (Params.ByScore)
                {
                    Utils.viewModel.AddScoreLimitedGameAsync(Params.PlayerName, DateTime.Now, TimeSpaner);
                }

                MessageDialog msgDialog = new MessageDialog("Game is Over !");
                UICommand OK = new UICommand("OK");
                OK.Invoked += Popup_OK_Invoked;
                Utils.Show(msgDialog, new List<UICommand> { OK });

            }
        }
       

        private TimeSpan TimeSpaner { get; set; }
        public DateTime EndTime { get; set; }
        public DispatcherTimer Timer { get; set; }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                stopTimer();
                Server.Paused = true;
            }

            else
            {
                resumeTimer();
                Server.Paused = false;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e) // 
        {
            Params = e.Parameter as Session;
            this.InitializeComponent();
            request.RequestActive();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += DispatcherTimer_Tick;
            if (Params.ByTime)
            {
                //TimeSpaner = TimeSpan.FromMinutes(3);
                TimeSpaner = TimeSpan.FromSeconds(45);
            }

            if (Params.ByScore)
            {
                TimeSpaner = TimeSpan.Zero;
            }
            tb_Timer.Text = showTime();
            Timer.Start();

            tb_player.Text = Params.PlayerName;

            //reset Score : 
            tb_UserScore.Text = 0.ToString();
            tb_RobotScore.Text = 0.ToString();

            //Connecting to the local Ad-Hock Wifi Server

            await Server.ConnectToServer(Frame);
           
                Server.SendToServer(Server.Command.START, Frame);
                await Server.listenToPackets();
            
            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            stopGame(true);
            Server.Dispose();
            connected = false;
            request.RequestRelease();
        }
    }
}