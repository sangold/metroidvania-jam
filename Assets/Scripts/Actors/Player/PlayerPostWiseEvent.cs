using UnityEngine;
using Event = AK.Wwise.Event;

[CreateAssetMenu(menuName = "SO/Audio/Player", fileName = "New PlayerAudioChannelSO")]
public class PlayerPostWiseEvent : ScriptableObject
{
    [Header("Movement")]
    public Event Player_Footstep_Event;
    public Event Player_Jump_Event;
    public Event Player_Double_Jump_Event;
    public Event Player_Landed_Event;

    [Header("Attack")]
    public Event Player_Slash_Event;
    public Event Player_Slide_Event;

    [Header("Capacity")]
    public Event Player_Ghost_Dash_Event;
    public Event Player_Wall_Sliding;
}
