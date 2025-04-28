using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class IntroTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Image blackScreenImage; 
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;
    [SerializeField] private TextMeshProUGUI textToDisable;

    private bool isKeyEnabled = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        virtualCamera.gameObject.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found.");
        }

        timeline.stopped += OnTimelineStopped;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {

            if (playerMovement != null)
            {
                playerMovement.enableControl(false);
            }

            playerCamera.enabled = false;
            virtualCamera.gameObject.SetActive(true);
            timeline.Play();

            // Disable text and "E" key
            if (textToDisable != null)
            {
                textToDisable.enabled = false;
            }
            isKeyEnabled = false;
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {

        virtualCamera.gameObject.SetActive(false);

        if (textToDisable != null)
        {
            textToDisable.enabled = true;
        }
        isKeyEnabled = true;

        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {

        blackScreenImage.gameObject.SetActive(true);
        blackScreenCanvasGroup.alpha = 0;
        float duration = 1.0f;
        float waitDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackScreenCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        yield return new WaitForSeconds(waitDuration);

        SceneManager.LoadScene("GamePlay"); 
    }

    private void OnDestroy()
    {
        timeline.stopped -= OnTimelineStopped;
    }
}