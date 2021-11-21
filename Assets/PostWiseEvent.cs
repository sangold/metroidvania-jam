using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWiseEvent : MonoBehaviour
{
    public AK.Wwise.Event Player_Slash_Event;
    public AK.Wwise.Event Player_Slide_Event;
    public AK.Wwise.Event Player_Landed_Event;
    public AK.Wwise.Event Player_Jump_Event;
    public AK.Wwise.Event Player_Double_Jump_Event;
    public AK.Wwise.Event Player_Ghost_Dash_Event;
    public AK.Wwise.Event Player_Wall_Sliding;
    // Start is called before the first frame update
    void Start()
    {
        //Player_Slash_Event.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
