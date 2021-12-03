using MJ.GameState;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SetState(new GameLoopState());
    }
}
