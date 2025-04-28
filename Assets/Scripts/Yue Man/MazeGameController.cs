using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MazeGameController : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public List<string> dialogues;
    private int currentDialogueIndex = 0;
    private bool isDialogueActive = true;
    public List<ColorChangerWithClickAndReset> doors;
    private float interval = 3.0f;
    private float minInterval = 0.5f;
    private float intervalDecrement = 0.1f;

    private int missedClicks = 0;
    private int maxMissedClicks = 5;
    private bool gameOver = false;
    private float gameTimer = 60.0f;
    private bool gameWon = false;

    //relate to UI
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameWinText;

    // relate to te countdown time
    public TextMeshProUGUI countdownText;
    private float countdownTime = 1f;

    // relate to the health value
    public List<Image> healthIcons;
    private int currentHealth = 5;


    public GameObject text;
    public TextMeshProUGUI pressKeyText;

    // low health effect
    public PostEffectsController effectsController;


    void Start()
    {
        dialogues = new List<string>()
        {
            "Father: Don’t hide behind the door!!! I’m not drunk!",
            "Father: I’m not a failure! Come out! Talk to father, I promise I won’t hurt you again this time.",
            "Father: No, you are the bad kids. I’m your father. It’s my duty to punish you!",
            "Emily: Father got drunk again. He behaves like that since I have memory. I tried to understand his difficulties to raise me. However, that should not be the reason to hurt me!",
            "Emily: No! He is coming in! I have to hold the door! HELP!",
            "Game Rules: Click the red door within 3 seconds. You can miss 5 times.",
            "Press A: Turn Right",
            "Press D: Turn Left",
            "Click mouse: to change the door color"
        };
        ShowDialogue(dialogues[currentDialogueIndex]);


        Time.timeScale = 0f;

        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);

        countdownText.color = Color.red;

        text.SetActive(false);
        pressKeyText.gameObject.SetActive(false);

        if (effectsController != null) effectsController.allOff();
    }

    void Update()
    {
        // change dialogue logic
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Q))
        {
            NextDialogue();
        }

        if (text.activeSelf == true) {
            if (!gameWon){
                // game over restart the game
                if (Input.GetKeyDown(KeyCode.P)) {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                }
            }
            if(gameWon){
                //game win quit the game
                if(Input.GetKeyDown(KeyCode.L)){
                    SceneManager.LoadScene("SecondDiary");
                }
            }
        }
    }

    public void ShowDialogue(string message)
    {
        dialogueUI.SetActive(true);
        dialogueText.text = message;
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Count)
        {
            ShowDialogue(dialogues[currentDialogueIndex]);
        }
        else
        {
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        isDialogueActive = false;
        Time.timeScale = 1f;
        StartGame();
    }
    // start the logic of the game
    void StartGame()
    {
        StartCoroutine(RandomlyChangeDoors());
        StartCoroutine(GameTimer());
        UpdateCountdownText();
    }

    IEnumerator RandomlyChangeDoors()
    {
        while (!gameOver)
        {
            int randomIndex = Random.Range(0, doors.Count);
            ColorChangerWithClickAndReset selectedDoor = doors[randomIndex];
            selectedDoor.TriggerColorChange();

            // player didn't click within 5 seconds
            yield return new WaitForSeconds(5.0f);
            if (selectedDoor.isColorChanged)
            {
                missedClicks++;
                LoseHealth();
                if (missedClicks >= maxMissedClicks)
                {
                    // missed too much time
                    GameOver(false);
                }
            }

            yield return new WaitForSeconds(Random.Range(1.0f, interval));

            if (interval > minInterval)
            {
                interval -= intervalDecrement;
            }
        }
    }

    void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        countdownText.color = Color.red;
    }

    IEnumerator GameTimer()
    {
        while (gameTimer > 0 && !gameOver)
        {
            yield return new WaitForSeconds(1.0f);
            gameTimer--;
            UpdateCountdownText();

            if (gameTimer <= 0 && missedClicks < maxMissedClicks)
            {
                GameOver(true);
            }
        }
    }


    void GameOver(bool won)
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameOver = true;
        gameWon = won;

        if (effectsController != null) effectsController.allOff();

        ResetAllDoorsToOriginalColor();

        if (won)
        {
            gameWinUI.SetActive(true);
            text.SetActive(true);
            gameWinText.text = "CONGRATULATION. You have escaped.";

            pressKeyText.gameObject.SetActive(true);
            pressKeyText.text = "Press L to Quit";
        }
        else
        {
            gameOverUI.SetActive(true);
            text.SetActive(true);
            gameOverText.text = "GAME OVER";

            pressKeyText.gameObject.SetActive(true);
            pressKeyText.text = "Press P to Restart";
        }
    }

    void ResetAllDoorsToOriginalColor()
    {
        foreach (ColorChangerWithClickAndReset door in doors)
        {
            door.ResetToOriginalColor();
        }
    }

    void LoseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            healthIcons[currentHealth].enabled = false;
        }

        if (currentHealth < 3 && effectsController != null) {
            effectsController.setVignette(true);
        }

        if (currentHealth <= 0)
        {
            GameOver(false);
        }
    }
}
