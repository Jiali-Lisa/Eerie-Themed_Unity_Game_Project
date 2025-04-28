using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    private bool dialogue = false;
    [SerializeField] private GameObject PauseMenuCanavas;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private AudioSource[] Audio;

    private PlayerMovement playerMovement;
    public static CursorLockMode currGameLockMode = CursorLockMode.Locked;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Play();
            }
            else{
                Stop();
            }
        }

    }

    // Method related to Menu
    public void Play()
    {
        Cursor.lockState = currGameLockMode;
        Debug.Log(Cursor.lockState);

        PauseMenuCanavas.SetActive(false);
        if (crosshair != null) {
            crosshair.SetActive(true);
        }

        Time.timeScale = 1f;
        Paused = false;

        if (!dialogue){
            if (playerMovement != null) playerMovement.enableControl(true);
        }


        foreach (AudioSource audio in Audio) {
            if (audio) {
                audio.UnPause();
            }
        }
    }

    void Stop()
    {
        Cursor.lockState = CursorLockMode.None;

        PauseMenuCanavas.SetActive(true);
        if (crosshair != null) {
            crosshair.SetActive(false);
        }

        Time.timeScale = 0f;
        Paused = true;

        if (playerMovement != null) playerMovement.enableControl(false);

        foreach (AudioSource audio in Audio) {
            if (audio) {
                audio.Pause();
            }
        }
    }

    public bool isPaused() {
        return Paused;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void setdialogue(bool set){
        dialogue = set;
    }
}
