using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider _loadBar;
    public Text _loadText;
    public int _loadSceneAtIndex;

    private void Start()
    {
        LoadLevel(_loadSceneAtIndex);
    }

    private void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            _loadBar.value = progress;
            _loadText.text = progress * 100 + "%";
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}