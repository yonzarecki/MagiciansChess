using System;

namespace MagiciansChessDataAPI.Models
{
    public class LeaderboardEntry
    {
        public int ID { get; set; }
        public string username { get; set; }
        public bool humanWon { get; set; }
        public TimeSpan gameTime { get; set; }
    }
}
