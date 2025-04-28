using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class TextWithRed : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float appearDuration = 1.5f;
    public float disappearDuration = 1.5f;
    public float delay;
    private bool isClick = false;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public GameObject gameOverPanel;
    public RectTransform panel;


    public static BookSceneController controller;

    void Awake() {
        if (controller == null) controller = GameObject.FindObjectOfType<BookSceneController>();
    }

    void Start()
    {
        textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 0);
        SetRandomPosition();
        Invoke("BeginTextAppear", delay);

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
                    StopAllCoroutines();
                    textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 1f);
                    GameOver();
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
    }

    void GameOver(){
        if (gameOverPanel != null){
            gameOverPanel.SetActive(true);
            if (controller != null) controller.StopGame();
        }
        /*
        if (panel != null){
            panel.gameObject.SetActive(false);
        }*/
    }

}
