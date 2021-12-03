using System;
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

    public void LoadFromData()
    {

    }
}
