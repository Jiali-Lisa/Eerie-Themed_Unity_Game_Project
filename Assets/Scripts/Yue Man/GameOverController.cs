using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;        
using UnityEngine.SceneManagement; 

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverUI; 
    public TextMeshProUGUI gameOverText; 
    private bool isGameOver = false;

    void Start()
    {
        gameOverUI.SetActive(false);
      
    }

    public void TriggerGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            gameOverUI.SetActive(true);

            gameOverText.text = "GAME OVER";
            Debug.Log("Game Over UI");

        
            Time.timeScale = 0f;  
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
