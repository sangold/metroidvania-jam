using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private GameEvent _skipCutScene;
    private void Start()
    {
        GameManager.Instance.SkipCutSceneEvent = _skipCutScene;
        if(!GameManager.Instance.GameHasStarted)
        {
            GameManager.Instance.GameHasStarted = true;
            GameManager.Instance.GoToGameLoopState(_level);
        }
    }
}
