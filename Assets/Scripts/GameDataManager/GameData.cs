using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Player Data
    public Vector3 playerPosition; // Last visited portal exit point
    public PlayerStatsSO PlayerStats;

    // GameManager Data
    public int CurrentLevelIndex;
    public List<LevelData> Levels;
    public List<string> bossesDone;
}

[System.Serializable]
public class LevelData
{
    public string UUID;
    public List<EnemyData> _enemies;
    public List<CollectibleData> _collectibles;
}

[System.Serializable]
public class CollectibleData
{
    public string UUID;
    public Vector3 Position;
}

[System.Serializable]
public class EnemyData
{
    public string UUID; //gameObject name
    public bool isKilled;
    public Vector3 position;
}