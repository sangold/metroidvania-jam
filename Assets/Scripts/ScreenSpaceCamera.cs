using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GameObject MiniMapCamera = GameObject.Find("MiniMapCamera");
        MiniMapCamera.transform.position = Camera.main.transform.position;
        MiniMapCamera.transform.parent = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
