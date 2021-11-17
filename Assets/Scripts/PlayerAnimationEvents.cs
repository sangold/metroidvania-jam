using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public Player main;
    public void ReturnToStand(){
        main.state = Player._finiteState.stand;
    }
}
