using System;
using System.Collections.Generic;
using UnityEngine;

public enum World
{
    PRIDE,
    GLUTTONY,
    WRATH,
    ENVY,
    LUST,
    GREED,
    SLOTH
}

[Serializable]
public class Level
{
    [Header("Identification")]
    public string UUID;
    public World World;
    [Header("Position in the world")]
    public int PosX;
    public int PosY;
    [HideInInspector]
    public bool IsMirrorActivated;
    [HideInInspector]
    public bool IsExplored;
    [HideInInspector]
    public bool IsVisible;
    [HideInInspector]
    public bool IsActive;

    public void LoadFromData()
    {

    }

    public LevelData Save()
    {
        LevelData data = new LevelData();
        data.UUID = UUID;
        data._enemies = new List<Enemy.EnemyData>();
        foreach(Enemy enemy in GameObject.FindObjectsOfType<Enemy>(true))
        {
            data._enemies.Add((Enemy.EnemyData)enemy.CaptureState());
        }

        return data;
    }

    public static Color GetMainColor(World worldType)
    {
        switch(worldType)
        {
            case World.PRIDE:
                return new Color32(64, 78, 145, 255);
            case World.ENVY:
                return new Color32(64, 145, 108, 255);
            case World.GREED:
                return new Color32(202, 103, 2, 255);
            case World.GLUTTONY:
                return new Color32(252, 142, 30, 255);
            case World.LUST:
                return new Color32(63, 24, 125, 255);
            case World.WRATH:
                return new Color32(185, 61, 49, 255);
            case World.SLOTH:
                return new Color32(82, 82, 82, 255);
            default:
                return new Color32(82, 82, 82, 255);
        }
    }

    public static Color GetDarkestColor(World worldType)
    {
        switch (worldType)
        {
            case World.PRIDE:
                return new Color32(29, 27, 66, 255);
            case World.ENVY:
                return new Color32(27, 67, 50, 255);
            case World.GREED:
                return new Color32(187, 62, 3, 255);
            case World.GLUTTONY:
                return new Color32(249, 119, 0, 255);
            case World.LUST:
                return new Color32(39, 27, 66, 255);
            case World.WRATH:
                return new Color32(66, 27, 28, 255);
            case World.SLOTH:
                return new Color32(31, 31, 31, 255);
            default:
                return new Color32(31, 31, 31, 255);
        }
    }
    public static Color GetLightColor(World worldType)
    {
        switch (worldType)
        {
            case World.PRIDE:
                return new Color32(83, 129, 184, 255);
            case World.ENVY:
                return new Color32(82, 183, 136, 255);
            case World.GREED:
                return new Color32(238, 180, 71, 255);
            case World.GLUTTONY:
                return new Color32(252, 167, 81, 255);
            case World.LUST:
                return new Color32(129, 83, 184, 255);
            case World.WRATH:
                return new Color32(216, 118, 109, 255);
            case World.SLOTH:
                return new Color32(133, 133, 133, 255);
            default:
                return new Color32(133, 133, 133, 255);
        }
    }
    public static Color GetLightestColor(World worldType)
    {
        switch (worldType)
        {
            case World.PRIDE:
                return new Color32(182, 216, 227, 255);
            case World.ENVY:
                return new Color32(183, 228, 199, 255);
            case World.GREED:
                return new Color32(233, 216, 166, 255);
            case World.GLUTTONY:
                return new Color32(255, 207, 158, 255);
            case World.LUST:
                return new Color32(206, 182, 227, 255);
            case World.WRATH:
                return new Color32(227, 186, 182, 255);
            case World.SLOTH:
                return new Color32(209, 209, 209, 255);
            default:
                return new Color32(209, 209, 209, 255);
        }
    }
}
