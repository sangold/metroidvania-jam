using System;

namespace MJ.GameState
{
    public class LoadingState : IState
    {

        public static Action OnLoadingScene;
        public static Action OnSceneLoaded;

        public void OnEnter()
        {
            GameManager.Instance.PlayerCanMove = false;
            OnLoadingScene?.Invoke();
        }

        public void OnExit()
        {
            OnSceneLoaded?.Invoke();
        }

        public void Tick(){}
    }
}
