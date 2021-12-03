using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMapView: MonoBehaviour
{
    private List<Level> _levels;
    private Dictionary<Level, GameObject> _levelsGOs;
    [SerializeField] private GameObject _levelSquareGO;
    [SerializeField] private float horizontalMargin = 0f;
    [SerializeField] private float verticalMargin = 0f;


    private void Awake()
    {
        _levelsGOs = new Dictionary<Level, GameObject>();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        InitGrid();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void InitGrid()
    {
        
        _levels = GameManager.Instance.Levels;

        Debug.Log(_levels.Count);
        foreach(var level in _levels)
        {
            if (!level.IsVisible) continue;
            if(!_levelsGOs.ContainsKey(level))
            {
                var levelGO = Instantiate(_levelSquareGO);
                levelGO.GetComponent<UILevelSquare>().Init(level, transform, new Vector2(horizontalMargin, verticalMargin));
                _levelsGOs.Add(level, levelGO);
            }
            var uiSquare = _levelsGOs[level].GetComponent<UILevelSquare>();
            uiSquare.SetActive(level.IsActive);
            uiSquare.SetExplored(level.IsExplored);
        }
    }
}

