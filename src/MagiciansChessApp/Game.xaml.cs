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
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MagiciansChessApp
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        private TimeSpan second = TimeSpan.Zero;
        private ChessLibrary.Game currGame;
        
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
            
            second += TimeSpan.FromMilliseconds(10);
            if(second < TimeSpan.FromMilliseconds(400))
            {
                return;
            }

            second = TimeSpan.Zero;

            TimeSpaner += TimeSpan.FromSeconds(1);

            tb_Timer.Text = showTime();
        }

        private void UpdateScore(bool UserScored)
        {
            
        }

        private void Popup_OK_Invoked(IUICommand command)
        {
            //TODO: Insert new score to the DB
            
        }

        private void stopGame(bool navigation)  // TODO: change to our game
        {
            Timer.Stop();

            if (!navigation)
            {
                // TODO: change comments in all file    
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
                PauseButtonImage.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/play.png", UriKind.Absolute));
            }

            else
            {
                resumeTimer();
                PauseButtonImage.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/pause.png", UriKind.Absolute));
            }
        }

        private Boolean checkValid(String str)
        {
            return str.Length == 2 &&
                   char.IsLetter(str[0]) &&
                   char.IsDigit(str[1]);
        }

        private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs e)
        {
            inputButton.IsEnabled = checkValid(fromInput.Text) && checkValid(toInput.Text);
        }

        private void setHumanTurn()
        {
            InputTextBlock.Text = "enter move(letter number):";
            fromInput.Visibility = Visibility.Visible;
            toInput.Visibility = Visibility.Visible;
            inputButton.Visibility = Visibility.Visible;
        }

        private void setComputerTurn()
        {
            InputTextBlock.Text = "Computer is making a move";
            fromInput.Visibility = Visibility.Collapsed;
            toInput.Visibility = Visibility.Collapsed;
            inputButton.Visibility = Visibility.Collapsed;
        }

        private async void DoneClick(object sender, RoutedEventArgs e)
        {
            /*
             * the Do Move dosent work throws null exception nedd to look why */
            if(currGame.DoMove(fromInput.Text,toInput.Text) < 0)
            {
                MessageDialog msg = new MessageDialog("Invalid Move! please enter again");
                Utils.Show(msg, new List<UICommand> { new UICommand("Close") });
                return;
            }
            
            //updated move in board and all is legal
            setComputerTurn();
            string move_str = ChessGameManager.GetBestMove(currGame);
            if (move_str == "bad move")
            {
                MessageDialog msg = new MessageDialog("Bad Computer move! ");
                Utils.Show(msg, new List<UICommand> { new UICommand("Close") });
                return;
            }

            bool pieceCaptured = false;
            if (move_str[move_str.Length - 3] == 'x')
                pieceCaptured = true;

            string from = move_str.Substring(move_str.Length - 5, 2);
            string to = move_str.Substring(move_str.Length - 2, 2);

            //TODO: sendmove to board
            await ChessGameManager.sendMoveToBoard(from + to);

            if (pieceCaptured)
            {
                MessageDialog msg = new MessageDialog("Please remove captured piece " + to);
                Utils.Show(msg, new List<UICommand> { new UICommand("Close") });
            }

            // execute computer move
            if (currGame.DoMove(from,to) == -1)
            {
                MessageDialog msg = new MessageDialog("Computer Error !");
                Utils.Show(msg, new List<UICommand> { new UICommand("Close") });
                return;
            }

            setHumanTurn();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e) // 
        {
            this.InitializeComponent();
            this.currGame = ChessGameManager.initializeGame();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            Timer.Tick += DispatcherTimer_Tick;
            TimeSpaner = TimeSpan.FromMinutes(0);

            tb_Timer.Text = showTime();
            Timer.Start();
            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            stopGame(true);
            Frame.Navigate(typeof(MainMenu));
        }

        private void SurrenderButton_Click(object sender, RoutedEventArgs e)
        {
            stopGame(true);
            Frame.Navigate(typeof(MainMenu));
        }
    }
}