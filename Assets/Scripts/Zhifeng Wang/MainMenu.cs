using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("Intro");
        PauseMenu.currGameLockMode = CursorLockMode.Locked;
    }

    public void quit()
    {
        Application.Quit();
        // Debug.Log("111");
    }
}
