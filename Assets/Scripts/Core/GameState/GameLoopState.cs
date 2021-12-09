using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MJ.GameState
{
    public class GameLoopState : IState
    {
        private Level _level;
        public GameLoopState(Level level = null)
        {
            if(level == null)
            {
                _level = GameManager.Instance.CurrentLevel;
                return;
            }

            _level = level;
        }
        public void OnEnter()
        {
            GameManager.Instance.SetActiveLevel(_level);
            GameManager.Instance.PlayerCanMove = true;
            if (SceneManager.GetSceneByName("UI").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive).completed += (AsyncOperation o) => SetCameraForUI();
            }
            else
            {
                SetCameraForUI();
            }
        }

        private void SetCameraForUI()
        {
            //GameObject.FindObjectOfType<ScreenSpaceCamera>().Init();
        }

        public void OnExit()
        {
            GameManager.Instance.PlayerCanMove = false;
        }

        public void Tick()
        {
            if(Input.GetButtonUp("Cancel"))
            {
                GameManager.Instance.SetState(new PauseMenuState());
            }
        }
    }
}
