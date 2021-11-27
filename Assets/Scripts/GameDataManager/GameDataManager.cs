using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataManager
{
    public static bool hasDebugButtons = true;
    public static void saveData(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Vector2 position = player.transform.position;
        PlayerPrefs.SetFloat("Player.x", position.x);
        PlayerPrefs.SetFloat("Player.y", position.y);
        PlayerPrefs.SetInt("Player.health", player.Health);
    }
    public static void loadData(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("Player.x", player.transform.position.x), PlayerPrefs.GetFloat("Player.y", player.transform.position.y));
        player.Health = PlayerPrefs.GetInt("Player.health", 100);
    }
}
