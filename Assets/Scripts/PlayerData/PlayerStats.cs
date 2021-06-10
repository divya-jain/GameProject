/// <summary>
/// Player Data that remains constant throughout the game session
/// </summary>

namespace Game.Utils
{
    public static class PlayerStats
    {
        private static int highScore;

        public static int HighScore
        {
            get { return highScore; }
            set { highScore = value; }
        }
    }
}