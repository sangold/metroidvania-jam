using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTilePostWiseEvent : MonoBehaviour
{
    public AK.Wwise.Event Fire_Tile_On_Event;
    // Start is called before the first frame update
    void Start()
    {
        Fire_Tile_On_Event.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
