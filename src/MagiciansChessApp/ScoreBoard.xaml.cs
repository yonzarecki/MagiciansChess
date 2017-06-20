using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        public ScoreBoard()
        {
            this.InitializeComponent();
        }



        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            Params = args.Parameter as Session;

            if (Params != null)
            {
                using (var client = Utils.NewAPIClient())
                {
                    var t = MagiciansChessExtensions.GetAsync(new MagiciansChess(client));
                    t.ContinueWith(tsk => this.ShowTable(tsk.Result));
                }
            }
        }

        private void ShowTable(IList<Models.LeaderboardEntry> table_entries)
        {
            ByScoreListView.Visibility = Visibility.Visible;
        }
    }
}