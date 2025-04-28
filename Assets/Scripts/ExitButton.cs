using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField]private Button quitButton;
    void Start()
    {
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
}
