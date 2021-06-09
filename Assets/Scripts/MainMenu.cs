using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles Game Screen Transition on actions TapToStart,GameOver. Also displays Players scores
/// </summary>
///
/// 
public enum ItemType
{
    Rock,
    Paper,
    Scissor,
    Spock,
    Lizard
}

public class MainMenu : MonoBehaviour
{
    private ItemType _computerSelectedItem;

    private const string rockPrefabPath = "Prefabs/Rock";
    private const string paperPrefabPath = "Prefabs/Paper";
    private const string scissorPrefabPath = "Prefabs/Scissor";
    private const string spockPrefabPath = "Prefabs/Spock";
    private const string lizardPrefabPath = "Prefabs/Lizard";


    public GameObject GameMenu;
    public GameObject StartMenu;
    public GameObject GameScreen;
    public GameObject playerChooser;
    public GameObject computerChooser;
    public Text score;
    public Text highScore;

    public GameObject gameOver;
    public GameObject NextRound;

    #region Score

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    #endregion

    public void OnPlayButtonClicked()
    {
        StartGame();
    }

    public void OnItemSelect(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                if (_computerSelectedItem == ItemType.Scissor || _computerSelectedItem == ItemType.Lizard)
                {
                    Score++;
                    StartCoroutine(PlayNextRound());
                }
                else
                {
                    StartCoroutine(ResetGameOnLose());
                }
                break;
            case 1:
                if (_computerSelectedItem == ItemType.Rock || _computerSelectedItem == ItemType.Spock)
                {
                    Score++;
                    StartCoroutine(PlayNextRound());
                }
                else
                {
                    StartCoroutine(ResetGameOnLose());
                }
                break;
            case 2:
                if (_computerSelectedItem == ItemType.Paper || _computerSelectedItem == ItemType.Lizard)
                {
                    Score++;
                    StartCoroutine(PlayNextRound());
                }
                else
                {
                    StartCoroutine(ResetGameOnLose());
                }

                break;
            case 3:
                if (_computerSelectedItem == ItemType.Scissor || _computerSelectedItem == ItemType.Rock)
                {
                    Score++;
                    StartCoroutine(PlayNextRound());
                }
                else
                {
                    StartCoroutine(ResetGameOnLose());
                }

                break;
            case 4:
                if (_computerSelectedItem == ItemType.Paper || _computerSelectedItem == ItemType.Spock)
                {
                    Score++;
                    StartCoroutine(PlayNextRound());
                }
                else
                {
                    StartCoroutine(ResetGameOnLose());
                }

                break;
            default:
                break;
        }

        if (Score > HighScore)
            HighScore = Score;
        score.text = "Score: " + Score;
    }

    public IEnumerator ResetGameOnLose()
    {
        gameOver.SetActive(true);
        yield return new WaitForSeconds(2f);
        highScore.text = HighScore.ToString();
        Score = 0;
        gameOver.SetActive(false);
        StartMenu.SetActive(true);
        GameMenu.SetActive(false);
        GameScreen.SetActive(false);
        StartCoroutine(StartMenu.GetComponentInChildren<SequentialAnimator>().Play());
        yield return null;

    }

    public IEnumerator PlayNextRound()
    {
        NextRound.SetActive(true);
        yield return new WaitForSeconds(2f);
        NextRound.SetActive(false);
        PlayComputer();
    }

    private void StartGame()
    {
        StartMenu.SetActive(false);
        GameMenu.SetActive(true);
        GameScreen.SetActive(true);
        PlayComputer();
    }

    void PlayComputer()
    {
        int rand = Random.Range(0, 5);

        switch (rand)
        {
            case 0:
                _computerSelectedItem = ItemType.Rock;
                PopulateItemPrefab(rockPrefabPath, computerChooser.transform);
                break;
            case 1:
                _computerSelectedItem = ItemType.Paper;
                PopulateItemPrefab(paperPrefabPath, computerChooser.transform);
                break;
            case 2:
                _computerSelectedItem = ItemType.Scissor;
                PopulateItemPrefab(scissorPrefabPath, computerChooser.transform);
                break;
            case 3:
                _computerSelectedItem = ItemType.Spock;
                PopulateItemPrefab(spockPrefabPath, computerChooser.transform);
                break;
            case 4:
                _computerSelectedItem = ItemType.Lizard;
                PopulateItemPrefab(lizardPrefabPath, computerChooser.transform);
                break;
        }

        playerChooser.SetActive(true);
    }

    private void PopulateItemPrefab(string path, Transform parent)
    {
        Object prefab = Resources.Load(path);

        if (prefab == null)
        {
            Debug.Log("Failed to load prefab at path: " + path);
            return;
        }

        Instantiate(prefab, parent);
    }
}