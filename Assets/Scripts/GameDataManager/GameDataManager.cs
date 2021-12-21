using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameDataManager
{    

    public static void SaveLevel(int saveNumber)
    {
        GameData gameData = new GameData();
        gameData.bossesDone = GameManager.Instance.BossesDone;
        
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameData.playerPosition = player.transform.position;
        gameData.PlayerStats = player.GetStats;

        List<LevelData> ld = new List<LevelData>();
        foreach(Level l in GameManager.Instance.Levels)
        {
            ld.Add(l.Save());
        }
        gameData.Levels = ld;

        string jsonData = JsonUtility.ToJson(gameData);
        using(StreamWriter sw = new StreamWriter($"SaveGame{saveNumber}.json"))
        {
            sw.Write(jsonData);
        }
    }

    public static GameData Load(int saveNumber)
    {
        GameData data = new GameData();
        using(StreamReader sr = new StreamReader($"SaveGame{saveNumber}.json"))
        {
            string json = sr.ReadToEnd();

            data = JsonUtility.FromJson<GameData>(json);

        }

        return data;
    }
    public static bool hasDebugButtons = true;
    public static void saveData(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Vector2 position = player.transform.position;
        PlayerPrefs.SetFloat("Player.x", position.x);
        PlayerPrefs.SetFloat("Player.y", position.y);
        PlayerPrefs.SetInt("Player.HasScythe", player.CheckAbility(PowerUpType.SCYTHE) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasDoubleJump", player.CheckAbility(PowerUpType.DOUBLEJUMP) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasWallJump", player.CheckAbility(PowerUpType.WALLJUMP) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasGhostDash", player.CheckAbility(PowerUpType.GHOSTDASH) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasDash", player.CheckAbility(PowerUpType.DASH) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasMirror", player.CheckAbility(PowerUpType.MIRROR) ? 1 : 0);
        PlayerPrefs.SetInt("Player.HasChargeAttack", player.CheckAbility(PowerUpType.CHARGEATTACK) ? 1 : 0);
        PlayerPrefs.SetInt("Player.health", player.Health);
        PlayerPrefs.SetInt("Player.Maxhealth", player.MaxHealth);
    }
    public static void loadData(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.LoadData(
            PlayerPrefs.GetFloat("Player.x", player.transform.position.x),
            PlayerPrefs.GetFloat("Player.y", player.transform.position.y),
            PlayerPrefs.GetInt("Player.health", 8),
            PlayerPrefs.GetInt("Player.Maxhealth", 4),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasScythe", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasMirror", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasDoubleJump", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasWallJump", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasGhostDash", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasDash", 0)),
            Convert.ToBoolean(PlayerPrefs.GetInt("Player.HasChargeAttack", 0))
            );
    }
}
