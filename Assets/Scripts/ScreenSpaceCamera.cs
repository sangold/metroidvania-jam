using UnityEngine;

public class ScreenSpaceCamera : MonoBehaviour
{
    public void Init()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GameObject MiniMapCamera = GameObject.Find("MiniMapCamera");
        if(MiniMapCamera == null)
            return;
        MiniMapCamera.transform.position = Camera.main.transform.position;
        MiniMapCamera.transform.parent = Camera.main.transform;
    }
}
