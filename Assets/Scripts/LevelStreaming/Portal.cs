using MJ.GameState;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    public enum Destination { A, B, C, D, E, F }
    [SerializeField][Tooltip("Scene Name (don't forget to add it to the build Menu)")]
    private string _sceneToLoad;
    public Destination DestinationIdentification;
    public Transform ExitPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        GameManager.Instance.SetState(new LoadingState());
        DontDestroyOnLoad(gameObject);
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        float playerDirection = player.GetFaceDirection();
        yield return new WaitForSeconds(.35f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneToLoad));
        Portal destPortal = GetDestinationPortal();
        UpdatePlayer(destPortal, playerDirection);
        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
        GameManager.Instance.SetState(new GameLoopState());
    }

    private void UpdatePlayer(Portal destPortal, float playerDirection)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Vector3 destination = destPortal.ExitPoint.position;
        destination.z = 0;
        player.transform.position = destination;
        if(playerDirection < 0)
        {
            player.TurnLeft();
        }
        else
        {
            player.TurnRight();
        }
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
