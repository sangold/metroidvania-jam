using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.GoToMainMenuState();
    }
    public void StartGame()
    {
        StartCoroutine(LoadLevel());

    }

    private IEnumerator LoadLevel()
    {
        string _sceneToLoad = "Level_P_00";
        GameManager.Instance.GoToLoadingState();
        yield return SceneManager.LoadSceneAsync(_sceneToLoad);
        GameManager.Instance.GoToGameLoopState();
    }
}
