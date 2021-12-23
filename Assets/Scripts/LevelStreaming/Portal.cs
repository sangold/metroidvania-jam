using MJ.GameState;
using Reapling.SaveLoad;
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
    public bool isManual;
    private bool _hasPlayerInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;
        _hasPlayerInRange = true;
        if (isManual) return;
        if (collision.tag == "Player")
            StartCoroutine(LoadLevel());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;
        _hasPlayerInRange = false;
    }

    public void ManualLoading()
    {
        if(_hasPlayerInRange)
            StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        GameManager.Instance.GoToLoadingState();
        DontDestroyOnLoad(gameObject);
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        bool PlayerIsTurnToTheLeft = player.IsTurnToTheLeft();
        SaveLoadManager.Instance.Save(2);
        yield return new WaitForSeconds(.35f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Time.timeScale = 0f;
        yield return SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneToLoad));
        Portal destPortal = GetDestinationPortal();
        UpdatePlayer(destPortal, PlayerIsTurnToTheLeft);
        Time.timeScale = 1f;
        SaveLoadManager.Instance.Load(2);
        GameManager.Instance.GoToGameLoopState();
        yield return new WaitForSeconds(.25f);
        FindObjectOfType<HealthHUD>().ReattachTarget();
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal destPortal, bool playerIsTurnToTheLeft)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Vector3 destination = destPortal.ExitPoint.position;
        destination.z = 0;
        player.transform.position = destination;
        if(playerIsTurnToTheLeft)
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
