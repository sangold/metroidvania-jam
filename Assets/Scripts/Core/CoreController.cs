using MJ.GameState;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] private Level _level;
    private void Start()
    {
        GameManager.Instance.SetState(new GameLoopState(_level));
    }
}
