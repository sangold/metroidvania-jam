using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Player Data
    public Vector3 playerPosition; // Last visited portal exit point
    public int CurrentLevelIndex;
    public PlayerStatsSO PlayerStats;

    public List<Level> Levels;

}