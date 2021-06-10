using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles score and timer text on game screen
/// </summary>

namespace Game
{
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu _Instance { get; private set; }

        public Text score;
        public Text highScore;
        public Slider timer;

        private void Start()
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
        }

        public void OnTimerFinished()
        {
            timer.enabled = false;
        }
    }
}