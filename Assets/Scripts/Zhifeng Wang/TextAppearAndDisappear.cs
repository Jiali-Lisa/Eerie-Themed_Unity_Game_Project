using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TextAppearAndDisappear : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float appearDuration = 1.5f;
    public float disappearDuration = 1.5f;
    public float delay;
    private bool isClick = false;
    public bool last;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public RectTransform panel;
    public GameObject gameOverPanel;
    public GameObject gamePassPanel;
    public static AudioSource scribbleSFX;


    private static int unclickedCount = 0;
    public int maxUnclickedAllowed = 3;
    public Image[] hearts;

    public static BookSceneController controller;

    void Awake() {
        if (controller == null) controller = GameObject.FindObjectOfType<BookSceneController>();
    }

    void Start()
    {
        PauseMenu.currGameLockMode = CursorLockMode.None;
        textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 0);
        SetRandomPosition();
        Invoke("BeginTextAppear", delay);
        unclickedCount = 0;
        if (scribbleSFX == null) {
            scribbleSFX = GetComponentInParent<AudioSource>();
        }
    }

    void BeginTextAppear(){
        StartCoroutine(TextAppear());
    }

    void SetRandomPosition(){
        if (panel != null){
            float width = panel.rect.width;
            float height = panel.rect.height;

            Vector2 randomPosition = new Vector2(Random.Range(-width/2 + 200, width/2 - 200), Random.Range(-height/2 + 100, height/2 - 100));
            textElement.rectTransform.anchoredPosition = randomPosition;
        }
    }

    void Update()
    {
        if (controller.stopped) return;
        if (Input.GetMouseButtonDown(0) && !isClick)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == textElement.gameObject && textElement.color.a > 0.01)
                {
                    isClick = true;
                    if (scribbleSFX != null) scribbleSFX.Play();
                    StopAllCoroutines();
                    textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 1f);
                    if (last && !gameOverPanel.activeInHierarchy){
                        gamePassPanel.SetActive(true);
                        if (controller != null) controller.StopGame();
                        //SceneManager.LoadScene("GamePlay");
                    }
                }
            }
        }

    }

    public IEnumerator TextAppear()
    {
        while (textElement.color.a < 1.0f && !isClick)
        {
            textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, textElement.color.a + (Time.deltaTime / appearDuration));
            yield return null;
        }

        if (!isClick)
        {
            StartCoroutine(TextDisappear());
        }
    }

    public IEnumerator TextDisappear()
    {
        while (textElement.color.a > 0 && !isClick)
        {
            textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, textElement.color.a - (Time.deltaTime / disappearDuration));
            yield return null;
        }

        if (!isClick)
        {
            unclickedCount++;
            UpdateHearts(unclickedCount);


            if (unclickedCount > maxUnclickedAllowed && !gameOverPanel.activeInHierarchy)
            {
                //Debug.Log("1111111");
                gameOverPanel.SetActive(true);
                if (controller != null) controller.StopGame();
            }
        }


        if (last && !gameOverPanel.activeInHierarchy){
            gamePassPanel.SetActive(true);
            if (controller != null) controller.StopGame();
            //SceneManager.LoadScene("GamePlay");
        }
    }

    void UpdateHearts(int unclickedCount)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < unclickedCount)
            {
                hearts[i].enabled = false;
            }
        }
        if (controller != null && unclickedCount > 1) controller.lowHealth();
    }
}
