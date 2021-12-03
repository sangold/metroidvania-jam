using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
                _instance.gameObject.transform.SetParent(null);
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }

        private set { _instance = value; }
    }
    private List<string> _bossesDone;
    private List<Level> _levels;
    private List<string> _mirrorVisibles;

    private StateMachine _stateMachine;
    public bool PlayerCanMove;

    private void Awake()
    {
        
        _bossesDone = new List<string>();
        _levels = new List<Level>();
        _mirrorVisibles = new List<string>();
        _stateMachine = new StateMachine();
    }

    public void SetState(IState targetState)
    {
        _stateMachine.SetState(targetState);
    }
}
