using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isEndDoor = false;
    private GameObject endDoor;
    private int keysCollected = 0;

    void Start()
    {
        endDoor = GameObject.FindGameObjectWithTag("EndDoor");
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.IsPlaying())
            return;
        if (isEndDoor && Input.GetKeyDown(KeyCode.G) && keysCollected >= 3)
        {
            endDoor.GetComponent<EndDoor>().OpenDoor();
        }
        else if (isEndDoor && Input.GetKeyDown(KeyCode.G) && keysCollected < 3)
        {
            UIManager.instance.ShowNoKeyText();

        }
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     Cursor.lockState = CursorLockMode.None;
        //     Cursor.visible = true;
        //     GameManager.instance.SetPlaying(false);
        //     Timer.instance.StopTimer();
        //     UIManager.instance.ShowClearPanel();
        //     StartCoroutine(WaitAndSave());
        // }
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.instance.ShowCaughtPanel();
            StartCoroutine(WaitAndReloadStage());
        }
    }

    IEnumerator WaitAndReloadStage()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    IEnumerator WaitAndSave()
    {
        yield return new WaitForSeconds(5);
        UIManager.instance.ShowSavePanel();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndDoor"))
        {
            isEndDoor = true;
        }
        else if (other.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            keysCollected++;
            UIManager.instance.AddKeyText();
        }
        else if (other.CompareTag("EndPoint"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.instance.SetPlaying(false);
            Timer.instance.StopTimer();
            UIManager.instance.ShowClearPanel();
            StartCoroutine(WaitAndSave());
        }
        else if (other.CompareTag("Ghost"))
        {
            UIManager.instance.ShowCaughtPanel();
            StartCoroutine(WaitAndReloadStage());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EndDoor"))
        {
            isEndDoor = false;
        }
    }
}
