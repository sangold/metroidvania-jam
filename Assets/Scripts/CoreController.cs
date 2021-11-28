using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreController : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetSceneByName("UI").isLoaded == false)
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}
