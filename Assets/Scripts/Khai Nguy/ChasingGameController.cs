using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChasingGameController : MonoBehaviour
{
    private bool gameOver = false;
    private bool gameWon = false;

    [SerializeField] private Image blackScreenImage;
    [SerializeField] private CanvasGroup blackScreenCanvasGroup;

    [SerializeField] private GameObject player;
    [SerializeField] private PhoneInteractable phone;
    [SerializeField] private GameObject phoneLocker;

    private bool gameTriggered = false;
    [SerializeField] private ChasingDoorInteractable firstDoor;
    [SerializeField] private TriggerInteractable firstDoorUnlock;
    [SerializeField] private ChasingMovingWall lastWall;

    // Goal
    [SerializeField] private ChasingGameGoal goal;
    [SerializeField] private TriggerInteractable[] tasks;
    private bool showIndicator = false;

    [SerializeField] private GameObject menuManager;
    [SerializeField] private AudioSource Ambience;
    [SerializeField] private AudioSource Breaths;
    [SerializeField] private AudioSource Distort;
    private int distortionIntensity;
    [SerializeField] private int distortionCountDownDelay;
    private bool allowDistortion;
    [Range(0, 30)][SerializeField] private int numberOfDistortions;

    [SerializeField] private PostEffectsController effectsController;
    [SerializeField] private float effectTimeScale = 1;
    private bool effectRunning = false;

    // Dialogues
    private ChasingDialogueManager dialogueManager;
    private Dictionary<DIALOGUES, bool> doShowDialogues = new Dictionary<DIALOGUES, bool>();

    public GameObject interactUI;
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameWinText;

    void Start()
    {
        distortionIntensity = 1;
        allowDistortion = false;

        firstDoor.LockDoor();
        HideIndicators();

        doShowDialogues.Add(DIALOGUES.DISTORT1, true);
        doShowDialogues.Add(DIALOGUES.DISTORT2, true);
        doShowDialogues.Add(DIALOGUES.FLASH, true);
        doShowDialogues.Add(DIALOGUES.LOCKER, true);
        doShowDialogues.Add(DIALOGUES.EXIT, true);


        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);

        dialogueManager = GetComponent<ChasingDialogueManager>();

        StartCoroutine(Disorientation());
        effectsController.setVignette(true);
        effectsController.changeVignetteTint(Color.black);
        effectsController.changeVignetteRadius(1);
        effectsController.changeVignetteFrequency(0);
    }

    void Update() {
        if (gameOver) {
            return;
        }

        if (menuManager.GetComponent<PauseMenu>().isPaused()) {
            return;
        }

        if (!gameTriggered) {
            if (!firstDoorUnlock.IsInteracted()) {
                if (firstDoor.IsInteracted()) {
                    dialogueManager.changeDialoguesSection(DIALOGUES.DOOR);
                    dialogueManager.StartDialogue();
                    firstDoor.setInteracted(false);
                }

                return;
            }

            firstDoor.UnlockDoor();

            if (firstDoor.IsOpen()) {
                gameTriggered = true;
                StartCoroutine(DisorientationCountDown());
            }
        }

        if (gameTriggered) {
            if (allowDistortion && numberOfDistortions >= distortionIntensity) {
                allowDistortion = false;

                if (doShowDialogues[DIALOGUES.DISTORT1] && distortionIntensity == 1) {
                    dialogueManager.changeDialoguesSection(DIALOGUES.DISTORT1);
                    dialogueManager.setAllowInteract(true);
                    dialogueManager.StartDialogue();
                    doShowDialogues[DIALOGUES.DISTORT1] = false;
                    Distort.volume = 0.5f;
                }

                if (doShowDialogues[DIALOGUES.DISTORT2] && distortionIntensity == 2) {
                    dialogueManager.changeDialoguesSection(DIALOGUES.DISTORT2);
                    dialogueManager.setAllowInteract(true);
                    dialogueManager.StartDialogue();
                    doShowDialogues[DIALOGUES.DISTORT2] = false;
                    phone.updateTips();
                    player.GetComponent<PlayerInteract>().showItemUI();
                    phone.setFlashed(false);
                    Distort.volume = 1;
                }

                if (!Distort.isPlaying) {
                    Distort.Stop();
                }

                Distort.Play();
                StartCoroutine(Disorientation(distortionIntensity, distortionIntensity));
                distortionIntensity++;
            }
        }

        if (showIndicator) {
            ShowIndicators();
        }

        if (goal.isInteracted()) {
            GameOver(true);
        }

        if (distortionIntensity > numberOfDistortions && allowDistortion) {
            StartCoroutine(Disorientation(30, 30));
            GameOver(false);
        }

        if (!lastWall.IsOpen()) {
            foreach (TriggerInteractable task in tasks) {
                if (!task.IsInteracted()) return;
            }
            lastWall.Open();
        }
    }

    void GameOver(bool won)
    {
        gameOver = true;
        gameWon = won;

        Ambience.Stop();
        Breaths.Stop();
        Distort.Stop();

        interactUI.SetActive(false);
        player.GetComponent<PlayerInteract>().setUI(false);
        player.GetComponent<PlayerMovement>().enableControl(false);

        if (won)
        {
            gameWinUI.SetActive(true);
            gameWinText.text = "YOU HAVE ESCAPED!!!";
            StartCoroutine(FadeToBlackAndSwitchScene());
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            gameOverUI.SetActive(true);
            gameOverText.text = "GAME OVER";
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        SceneManager.LoadScene("StartScene");
    }

    public void stopDisorientation() {
        effectRunning = false;
    }

    private IEnumerator DisorientationCountDown() {
        yield return new WaitForSeconds(distortionCountDownDelay);
        allowDistortion = true;
    }

    private IEnumerator Disorientation() {
        effectRunning = true;
        effectsController.setDistortTimeScale(effectTimeScale);
        effectsController.setDistort(true);
        while (effectRunning) {
            yield return new WaitForSeconds(Mathf.PI / effectTimeScale);
        }
        effectsController.setDistort(false);
        StartCoroutine(PhonePickUpTracker());
    }

    private IEnumerator Disorientation(float time) {
        effectsController.decreaseVignetteRadius(0.1f);
        effectRunning = true;
        effectsController.setDistortTimeScale(effectTimeScale);
        effectsController.setDistort(true);
        yield return new WaitForSeconds(time);
        effectsController.setDistort(false);
        StartCoroutine(DisorientationCountDown());
    }

    private IEnumerator Disorientation(float time, float newTimeScale) {
        effectsController.decreaseVignetteRadius(0.1f);
        effectRunning = true;
        effectsController.setDistortTimeScale(newTimeScale);
        effectsController.setDistort(true);
        yield return new WaitForSeconds(time);
        effectsController.setDistort(false);
        StartCoroutine(DisorientationCountDown());
    }

    private IEnumerator FadeToBlackAndSwitchScene()
    {
        float duration = 5.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (RenderSettings.fogDensity < 0.25f) {
                RenderSettings.fogDensity += 0.001f;
            }
            yield return null;
        }

        SceneManager.LoadScene("ThirdDiary");
    }

    private IEnumerator PhonePickUpTracker() {
        yield return new WaitForSeconds(15);

        if (!phone.IsPickedUp()) {
            dialogueManager.changeDialoguesSection(DIALOGUES.LOCKER);
            dialogueManager.setAllowInteract(true);
            dialogueManager.StartDialogue();
            doShowDialogues[DIALOGUES.LOCKER] = false;

            StartCoroutine(LockerHintOn());

            yield return new WaitUntil(() => phone.IsPickedUp());

            phoneLocker.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }

    private IEnumerator LockerHintOn() {
        phoneLocker.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        phoneLocker.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));

        float step = 0.1f;
        float curr = 0f;

        while (curr < 1) {
            curr += step;
            phoneLocker.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0, curr));
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void GoalHint() {
        player.GetComponent<PlayerInteract>().PutAwayItem();
        dialogueManager.setAllowInteract(false, true);
        dialogueManager.changeDialoguesSection(DIALOGUES.EXIT);
        dialogueManager.StartDialogue();
        doShowDialogues[DIALOGUES.FLASH] = false;
        StartCoroutine(AddTasks());
    }

    private IEnumerator AddTasks() {
        yield return new WaitUntil(() => dialogueManager.isHidden());
        foreach (TriggerInteractable task in tasks) {
            if (!task.IsInteracted()) task.AddTask(TaskType.MAIN);
        }
        StartCoroutine(EnableIndicators());
    }

    private Vector2 GetVectorToTarget(Vector3 targetPos) {
        Vector3 toTarget = Quaternion.Euler(0, -player.transform.rotation.eulerAngles.y, 0) * (targetPos - player.transform.position).normalized;

        return new Vector2(toTarget.x, toTarget.z).normalized;
    }

    private void ShowIndicators() {
        foreach (TriggerInteractable task in tasks) {
            if (!task.IsInteracted()) {
                Vector2 v = GetVectorToTarget(task.transform.position);
                float offset = 10f;
                float top = Screen.height/2 - offset;
                float right = Screen.width/2 - offset;

                Vector2 indicatorPos = new Vector2();
                Vector2 vert;
                Vector2 horz;
                if (v.x != 0) {
                    vert = new Vector2(right, v.y * right / v.x);
                } else {
                    vert = new Vector2(right, 0f);
                }
                if (v.y != 0) {
                    horz = new Vector2(v.x * top / v.y, top);
                } else {
                    horz = new Vector2(0f, top);
                }
                if (v.x < 0f) {
                    vert *= -1;
                }
                if (v.y < 0f) {
                    horz *= -1;
                }
                indicatorPos = vert.sqrMagnitude > horz.sqrMagnitude ? horz : vert;
                float angle = Vector2.SignedAngle(new Vector2(0, 1), v);

                Transform canvas = task.transform.GetChild(0);
                if (canvas != null) {
                    canvas.gameObject.SetActive(true);
                    Transform indicator = canvas.GetChild(0);
                    if (indicator != null) {
                        indicator.GetComponent<RectTransform>().anchoredPosition = indicatorPos;
                        indicator.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle);
                    }
                }
            }
        }
    }

    private void HideIndicators() {
        foreach (TriggerInteractable task in tasks) {
            Transform canvas = task.transform.GetChild(0);
            if (canvas != null) {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator EnableIndicators() {
        showIndicator = true;
        yield return new WaitForSeconds(10);
        showIndicator = false;
        HideIndicators();
    }
}
