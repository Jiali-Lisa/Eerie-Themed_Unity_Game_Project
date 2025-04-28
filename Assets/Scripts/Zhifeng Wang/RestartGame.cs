using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void ReloadGame()
    {
        SceneManager.LoadScene("BookScene");
    }
}

