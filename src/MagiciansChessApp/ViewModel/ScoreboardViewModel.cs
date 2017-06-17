using HockeyApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace HockeyApp.ViewModel
{
    public class ScoreboardViewModel : INotifyPropertyChanged
    {
        MobileServiceClient _client;

        public ScoreboardViewModel(MobileServiceClient client)
        {
            _client = client;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        private MobileServiceCollection<ScoreLimitedGame, ScoreLimitedGame> _ScoreLimitedGames;
        public MobileServiceCollection<ScoreLimitedGame, ScoreLimitedGame> ScoreLimitedGames
        {
            get { return _ScoreLimitedGames; }
            set
            {
                _ScoreLimitedGames = value;
                NotifyPropertyChanged("ScoreLimitedGames");
            }
        }

        private MobileServiceCollection<TimeLimitedGame, TimeLimitedGame> _TimeLimitedGames;
        public MobileServiceCollection<TimeLimitedGame, TimeLimitedGame> TimeLimitedGames
        {
            get { return _TimeLimitedGames; }
            set
            {
                _TimeLimitedGames = value;
                NotifyPropertyChanged("TimeLimitedGames");
            }
        }

        private bool _IsPending;
        public bool IsPending
        {
            get { return _IsPending; }
            set
            {
                _IsPending = value;
                NotifyPropertyChanged("IsPending");
            }
        }

        private string _ErrorMessage = null;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        // Service operations
        public async Task GetAllScoreLimitedGamesAsync()
        {
            IsPending = true;
            ErrorMessage = null;

            try
            {
                IMobileServiceTable<ScoreLimitedGame> table = _client.GetTable<ScoreLimitedGame>();
                ScoreLimitedGames = await table.OrderBy(x => x.Rank).ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
            finally
            {
                IsPending = false;
            }
        }

        public async Task AddScoreLimitedGameAsync(string name, DateTime date, TimeSpan duration)
        {
            IsPending = true;
            ErrorMessage = null;

            var scoreLimitedGame = new ScoreLimitedGame()
            {
                Name = name,
                Date = date,
                Duration = duration
            };

            try
            {
                IMobileServiceTable<ScoreLimitedGame> table = _client.GetTable<ScoreLimitedGame>();
                await table.InsertAsync(scoreLimitedGame);
                ScoreLimitedGames.Add(scoreLimitedGame);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
            finally
            {
                IsPending = false;
            }
        }

        public async Task GetAllTimeLimitedGamesAsync()
        {
            IsPending = true;
            ErrorMessage = null;

            try
            {
                IMobileServiceTable<TimeLimitedGame> table = _client.GetTable<TimeLimitedGame>();
                TimeLimitedGames = await table.OrderBy(x => x.Rank).ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
            finally
            {
                IsPending = false;
            }
        }

        public async Task AddTimeLimitedGameAsync(string name, DateTime date, int playerScore, int robotScore)
        {
            IsPending = true;
            ErrorMessage = null;

            var timeLimitedGame = new TimeLimitedGame()
            {
                Name = name,
                Date = date,
                PlayerScore = playerScore,
                RobotScore = robotScore
            };

            try
            {
                IMobileServiceTable<TimeLimitedGame> table = _client.GetTable<TimeLimitedGame>();
                await table.InsertAsync(timeLimitedGame);
                TimeLimitedGames.Add(timeLimitedGame);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
            finally
            {
                IsPending = false;
            }
        }
    }
}