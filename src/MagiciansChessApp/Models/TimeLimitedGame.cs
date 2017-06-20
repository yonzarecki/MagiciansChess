namespace MagiciansChessApp.Models
{
    public class TimeLimitedGame : Game
    {
        public int PlayerScore { get; set; }
        public int RobotScore { get; set; }
    }
}