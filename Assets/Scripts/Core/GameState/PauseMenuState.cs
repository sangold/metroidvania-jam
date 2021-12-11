using UnityEngine;
using UnityEngine.SceneManagement;

namespace MJ.GameState
{
    public class PauseMenuState : IState
    {
        public void OnEnter()
        {
            GameManager.Instance.PlayerCanMove = false;
            Time.timeScale = 0f;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }

        public void OnExit()
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
            Time.timeScale = 1f;
        }

        public void Tick()
        {
            if(Input.GetButtonUp("Cancel"))
            {
                GameManager.Instance.GoToGameLoopState();
            }
        }
    }
}
