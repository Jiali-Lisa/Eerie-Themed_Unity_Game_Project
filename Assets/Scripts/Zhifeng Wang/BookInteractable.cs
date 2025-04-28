using UnityEngine;
using UnityEngine.SceneManagement;

public class BookInteractable : MonoBehaviour, IInteractable
{
    public SceneController sceneController;

    private bool hasInteracted = false;

    public void Interact(GameObject player)
    {
        if (hasInteracted) return;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        sceneController.SavePlayerStateBeforeSceneChange();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        hasInteracted = true;
    }

    public void InteractionOff() {}

    public string GetInteractText()
    {
        return "Press E to Read";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
