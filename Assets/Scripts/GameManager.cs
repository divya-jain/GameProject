using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public enum ItemType
    {
        Rock,
        Paper,
        Scissor,
        Spock,
        Lizard
    }

    public enum GameState
    {
        Loading,
        Playing,
        Lost,
        TimeOut,
        Restart
    }

    /// <summary>
    /// Handles Player Input & Shows Results 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager _Instance { get; private set; }

        private ItemType _computerSelectedItem;

        private const string rockSpritePath = "Art/Rock";
        private const string paperSpritePath = "Art/Paper";
        private const string scissorSpritePath = "Art/scissors";
        private const string spockSpritePath = "Art/Spock";
        private const string lizardSpritePath = "Art/Lizard";
        private float _playerTimerDuration = 4f;
        private GameState _gameState = GameState.Loading;

        public int _loadingSceneIndex;
        public GameObject playerChooser;
        public GameObject computerChooser;

        public GameObject gameMenu;
        public GameObject gameOver;
        public GameObject NextRound;
        public GameObject timeOut;

        public int Score { get; private set; }

        #region EventFunctions

        private void Start()
        {
            if (_Instance == null)
            {
                _Instance = this;
            }

            ClickableObject.ObjectClicked += OnObjectClicked;
            Timer.TimerEnded += OnTimerFinished;
            StartCoroutine(PlayComputer());
        }

        private void OnDestroy()
        {
            ClickableObject.ObjectClicked -= OnObjectClicked;
            Timer.TimerEnded -= OnTimerFinished;
        }

        void Update()
        {
            if (Timer._Instance.timerIsRunning)
            {
                float timeSpent =
                    Mathf.Clamp01((_playerTimerDuration - Timer._Instance.timeRemaining) / _playerTimerDuration);
                GameMenu._Instance.timer.value = timeSpent;
            }
        }

        #endregion

        #region GameLogic

        public void OnObjectClicked(string tag)
        {
            switch (tag)
            {
                case "Rock":
                    if (_computerSelectedItem == ItemType.Scissor || _computerSelectedItem == ItemType.Lizard)
                    {
                        Score++;
                        StartCoroutine(PlayNextRound());
                    }
                    else
                    {
                        StartCoroutine(ShowLoseScreen(gameOver));
                    }

                    break;
                case "Paper":
                    if (_computerSelectedItem == ItemType.Rock || _computerSelectedItem == ItemType.Spock)
                    {
                        Score++;
                        StartCoroutine(PlayNextRound());
                    }
                    else
                    {
                        StartCoroutine(ShowLoseScreen(gameOver));
                    }

                    break;
                case "Scissor":
                    if (_computerSelectedItem == ItemType.Paper || _computerSelectedItem == ItemType.Lizard)
                    {
                        Score++;
                        StartCoroutine(PlayNextRound());
                    }
                    else
                    {
                        StartCoroutine(ShowLoseScreen(gameOver));
                    }

                    break;
                case "Spock":
                    if (_computerSelectedItem == ItemType.Scissor || _computerSelectedItem == ItemType.Rock)
                    {
                        Score++;
                        StartCoroutine(PlayNextRound());
                    }
                    else
                    {
                        StartCoroutine(ShowLoseScreen(gameOver));
                    }

                    break;
                case "Lizard":
                    if (_computerSelectedItem == ItemType.Paper || _computerSelectedItem == ItemType.Spock)
                    {
                        Score++;
                        StartCoroutine(PlayNextRound());
                    }
                    else
                    {
                        StartCoroutine(ShowLoseScreen(gameOver));
                    }

                    break;
                default:
                    break;
            }

            if (Score > PlayerStats.HighScore)
                PlayerStats.HighScore = Score;
            GameMenu._Instance.score.text = "Score: " + Score;
        }

        public IEnumerator ShowLoseScreen(GameObject uiScreen)
        {
            _gameState = GameState.Lost;
            GameMenu._Instance.highScore.text = PlayerStats.HighScore.ToString();
            StartCoroutine(SwitchScreens(uiScreen));
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync(_loadingSceneIndex);
        }

        public IEnumerator SwitchScreens(GameObject uiScreen)
        {
            Timer._Instance.ResetTimer();
            gameMenu.SetActive(false);
            computerChooser.SetActive(false);
            playerChooser.SetActive(false);
            uiScreen.SetActive(true);
            yield return new WaitForSeconds(1f);
            uiScreen.SetActive(false);
        }

        public IEnumerator PlayNextRound()
        {
            _gameState = GameState.Restart;
            StartCoroutine(SwitchScreens(NextRound));
            yield return new WaitForSeconds(1f);
            StartCoroutine(PlayComputer());
        }

        public IEnumerator PlayComputer()
        {
            _gameState = GameState.Playing;
            gameMenu.SetActive(false);
            computerChooser.SetActive(false);
            playerChooser.SetActive(false);

            yield return new WaitForSeconds(.5f);
            gameMenu.SetActive(true);
            computerChooser.SetActive(true);

            int rand = Random.Range(0, 5);
            switch (rand)
            {
                case 0:
                    _computerSelectedItem = ItemType.Rock;
                    PopulateItemPrefab(rockSpritePath, computerChooser.transform);
                    break;
                case 1:
                    _computerSelectedItem = ItemType.Paper;
                    PopulateItemPrefab(paperSpritePath, computerChooser.transform);
                    break;
                case 2:
                    _computerSelectedItem = ItemType.Scissor;
                    PopulateItemPrefab(scissorSpritePath, computerChooser.transform);
                    break;
                case 3:
                    _computerSelectedItem = ItemType.Spock;
                    PopulateItemPrefab(spockSpritePath, computerChooser.transform);
                    break;
                case 4:
                    _computerSelectedItem = ItemType.Lizard;
                    PopulateItemPrefab(lizardSpritePath, computerChooser.transform);
                    break;
            }

            StartCoroutine(AnimatePlayerChooser());
        }

        public IEnumerator AnimatePlayerChooser()
        {
            yield return new WaitForSeconds(1f);
            playerChooser.SetActive(true);
            List<Animator> animators = GetComponentsInChildren<Animator>().ToList();
            foreach (Animator animator in animators)
            {
                foreach (var currParam in animator.parameters)
                {
                    if (currParam.type == AnimatorControllerParameterType.Trigger && currParam.name == "DoRotate")
                    {
                        animator.SetTrigger("DoRotate");
                    }
                }
            }

            Timer._Instance.StartTimer(_playerTimerDuration);
        }

        private void PopulateItemPrefab(string path, Transform parent)
        {
            Sprite sprite = Resources.Load<Sprite>(path);

            if (sprite == null)
            {
                Debug.Log("Failed to load sprite at path: " + path);
                return;
            }

            parent.GetComponent<SpriteRenderer>().sprite = sprite;
            parent.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        public void OnTimerFinished()
        {
            GameMenu._Instance.OnTimerFinished();
            StartCoroutine(ShowLoseScreen(timeOut));
        }

        #endregion
    }
}