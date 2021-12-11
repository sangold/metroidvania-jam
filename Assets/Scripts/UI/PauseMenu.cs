using MJ.GameState;
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

    public void ShowMap()
    {
        PauseMenuChoices.SetActive(false);
        MapView.Open();
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
