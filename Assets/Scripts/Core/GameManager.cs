using MJ.GameState;
using System;
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

    public void GoToMainMenuState()
    {
        _stateMachine.SetState(new MainMenuState());
    }

    private List<string> _bossesDone;
    public List<string> BossesDone => _bossesDone;
    private List<Level> _levels;

    

    private Level _currentLevel;
    public Level CurrentLevel => _currentLevel;
    public List<Level> Levels => _levels;

    public bool GameHasStarted;

    private List<string> _mirrorVisibles;

    private StateMachine _stateMachine;
    public bool PlayerCanMove;
    public GameEvent SkipCutSceneEvent;

    private void Awake()
    {
        _bossesDone = new List<string>();
        _levels = new List<Level>();
        _mirrorVisibles = new List<string>();
        _stateMachine = new StateMachine();
    }

    #region State changes function
    public void GoToLoadingState()
    {
        _stateMachine.SetState(new LoadingState());
    }

    public void GoToCutSceneState()
    {
        _stateMachine.SetState(new CutSceneState());
    }

    public void GoToGameLoopState(Level level = null)
    {
        _stateMachine.SetState(new GameLoopState(level));
    }
    public void GoToPauseState()
    {
        _stateMachine.SetState(new PauseGameState());
    }
    public void GoToPauseMenuState()
    {
        _stateMachine.SetState(new PauseMenuState());
    }
    #endregion

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void SetSkipCutScene(GameEvent skip)
    {
        if (SkipCutSceneEvent == null)
            SkipCutSceneEvent = skip;
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

    public void AddBossKill(string bossName)
    {
        if (!_bossesDone.Contains(bossName))
            _bossesDone.Add(bossName);
    }
}
