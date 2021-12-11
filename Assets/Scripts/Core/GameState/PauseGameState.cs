﻿using UnityEngine;

namespace MJ.GameState
{
    public class PauseGameState : IState
    {
        public void OnEnter()
        {
            GameManager.Instance.PlayerCanMove = false;
            Time.timeScale = 0f;
        }

        public void OnExit()
        {
            Time.timeScale = 1f;
        }

        public void Tick()
        {

        }
    }
}
