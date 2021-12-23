using MJ.GameState;
using Reapling.SaveLoad;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenuChoices;
    [SerializeField]
    private UIMapView MapView;
    private void OnEnable()
    {
        MapView.gameObject.SetActive(false);
    }

    public void Save()
    {
        SaveLoadManager.Instance.Save(2);
    }

    public void ShowMap()
    {
        PauseMenuChoices.SetActive(false);
        MapView.Open();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowChoices()
    {
        MapView.Close();
        PauseMenuChoices.SetActive(true);
    }
    public void Unpause()
    {
        GameManager.Instance.GoToGameLoopState();
    }
}
