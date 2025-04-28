using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGamePlay : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("GamePlay");
        PauseMenu.currGameLockMode = CursorLockMode.Locked;
    }
}
