using HockeyApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class ScoreBoard : Page
    {
        private Session Params { get; set; }
        ScoreboardViewModel viewModel = new ScoreboardViewModel(App.MobileService);
        public enum ButtonState
        {
            BY_TIME,
            BY_SCORE,
            NONE
        }
        private ButtonState BtnState { get; set; }
        private bool ByTimeIsReady = false;
        private bool ByScoreIsReady = false;

        public ScoreBoard()
        {
            BtnState = ButtonState.NONE;
            viewModel.PropertyChanged += ShowTable;
            viewModel.PropertyChanged += MakeButtonsVisible;
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            Params = args.Parameter as Session;

            if (Params != null)
            {
                BtnState = Params.ByScore ? ButtonState.BY_SCORE : ButtonState.BY_TIME;
                if (!viewModel.IsPending)
                {
                    if (Params.ByScore)
                    {
                        viewModel.GetAllScoreLimitedGamesAsync();                    }

                    if (Params.ByTime)
                    {
                        viewModel.GetAllTimeLimitedGamesAsync();
                    }
                }
            }

            if (viewModel.IsPending)
            {
                btn_ByScore.Visibility = Visibility.Collapsed;
                btn_ByTime.Visibility = Visibility.Collapsed;
            }
        }

        
        private void btn_changeBy_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn?.Name)
            {
                case "btn_ByTime":
                    BtnState = ButtonState.BY_TIME;
                    ByScoreListView.Visibility = Visibility.Collapsed;
                    ByTimeListView.Visibility = Visibility.Collapsed;
                    viewModel.GetAllTimeLimitedGamesAsync();
                    break;

                case "btn_ByScore":
                    BtnState = ButtonState.BY_SCORE;
                    ByTimeListView.Visibility = Visibility.Collapsed;
                    ByScoreListView.Visibility = Visibility.Collapsed;
                    viewModel.GetAllScoreLimitedGamesAsync();
                    break;
                default:
                    break;
            }
        }

        private void ShowTable(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ScoreLimitedGames":
                    ByScoreIsReady = true;
                    if (BtnState == ButtonState.BY_SCORE)
                    {
                        ByScoreListView.Visibility = Visibility.Visible;
                    }
                    break;
                case "TimeLimitedGames":
                    ByTimeIsReady = true;
                    if (BtnState == ButtonState.BY_TIME)
                    {
                        ByTimeListView.Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    break;
            }
        }

        private void MakeButtonsVisible(object sender, PropertyChangedEventArgs e)
        {
            btn_ByTime.Visibility = Visibility.Visible;
            btn_ByScore.Visibility = Visibility.Visible;
        }
    }
}