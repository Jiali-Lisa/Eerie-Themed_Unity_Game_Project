using UnityEngine;
using UnityEngine.UI;

public class DiaryInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject bookImageUI;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Button quitButton;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;


    private bool hasInteracted = false;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

    }

    public void Interact(GameObject player)
    {
        if (hasInteracted) return;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        ShowBookImage();
        uiManager.ShowDialogue();
    }

    public void InteractionOff() {}

    public string GetInteractText()
    {
        return "Press E to Check";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void ShowBookImage()
    {
        if (bookImageUI != null)
        {
            bookImageUI.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        if (playerMovement != null) playerMovement.enabled = false;
        hasInteracted = true;
    }

}
