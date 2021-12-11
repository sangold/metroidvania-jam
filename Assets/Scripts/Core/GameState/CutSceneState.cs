using UnityEngine;

namespace MJ.GameState
{
    public class CutSceneState : IState
    {
        public void OnEnter()
        {
            GameManager.Instance.PlayerCanMove = false;
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
            if (Input.GetButtonUp("Cancel"))
            {
                GameManager.Instance.SkipCutSceneEvent.Raise();
            }
        }
    }
}
