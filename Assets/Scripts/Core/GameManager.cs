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
    private Level _currentLevel;
    public Level CurrentLevel => _currentLevel;
    public List<Level> Levels => _levels;
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

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void SetActiveLevel(Level level)
    {
        if(!_levels.Contains(level))
        {
            _levels.Add(level);
        }

        if (_currentLevel != null)
            _currentLevel.IsActive = false;

        level.IsVisible = true;
        level.IsActive = true;
        level.IsExplored = true;
        _currentLevel = level;
    }
}
