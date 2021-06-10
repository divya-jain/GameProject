using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class Timer : MonoBehaviour
    {
        public static System.Action TimerEnded;

        public static Timer _Instance { get; private set; }

        public float timeRemaining;
        public bool timerIsRunning = false;

        private void Start()
        {
            if (_Instance == null)
            {
                _Instance = this;
                DontDestroyOnLoad(_Instance);
            }
        }

        public void StartTimer(float seconds)
        {
            // Starts the timer automatically
            this.timeRemaining = seconds;
            this.timerIsRunning = true;
        }

        public void ResetTimer()
        {
            // resets the timer
            this.timerIsRunning = false;
        }

        void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    timeRemaining = 0;
                    this.timerIsRunning = false;
                    TimerEnded?.Invoke();
                }
            }
        }
    }
}