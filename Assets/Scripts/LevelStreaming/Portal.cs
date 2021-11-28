using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public enum Destination { A, B, C, D, E, F }
    [SerializeField][Tooltip("Scene Number in build menu")]
    private int _sceneToLoad = -1;
    public Destination DestinationIdentification;
    public Transform ExitPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(_sceneToLoad);
        Portal destPortal = GetDestinationPortal();
        UpdatePlayer(destPortal);
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal destPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 destination = destPortal.ExitPoint.position;
        destination.z = 0;
        player.transform.position = destination;
    }

    private Portal GetDestinationPortal()
    {
        foreach(Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.DestinationIdentification != DestinationIdentification) continue;

            return portal;
        }

        return null;
    }
}
