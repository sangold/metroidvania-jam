using UnityEngine;

namespace MJ.GameState
{
    public class PauseGameState : IState
    {
        public void OnEnter()
        {
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
