using System;

namespace HockeyApp.Models
{
    public class Game
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Rank { get; set; }
    }
}