using MagiciansChessApp.ViewModel;
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

namespace MagiciansChessApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScoreBoard : Page
    {
        private Session Params { get; set; }
        ScoreboardViewModel viewModel = new ScoreboardViewModel(App.MobileService);

        public ScoreBoard()
        {
            viewModel.PropertyChanged += ShowTable;
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            Params = args.Parameter as Session;

            if (Params != null && !viewModel.IsPending)
            {
                viewModel.GetAllTimeLimitedGamesAsync();
            }
        }

        private void ShowTable(object sender, PropertyChangedEventArgs e)
        {
            ByScoreListView.Visibility = Visibility.Visible;
        }
    }
}