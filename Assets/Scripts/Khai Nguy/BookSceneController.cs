using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BookSceneController : MonoBehaviour
{
    [SerializeField] private PostEffectsController effectsController;
    [SerializeField] private AudioSource music;
    public bool stopped;

    void Start() {
        effectsController.allOff();
        stopped = false;
    }

    void Update() {
    }

    public void StopGame() {
        if (music != null) music.Pause();
        stopped = true;
        effectsController.allOff();
        TextAppearAndDisappear[] texts = GameObject.FindObjectsByType<TextAppearAndDisappear>(FindObjectsSortMode.None);
        foreach (TextAppearAndDisappear text in texts) {
            text.StopAllCoroutines();
        }
        TextWithRed[] redTexts = GameObject.FindObjectsByType<TextWithRed>(FindObjectsSortMode.None);
        foreach (TextWithRed text in redTexts) {
            text.StopAllCoroutines();
        }
    }

    public void lowHealth() {
        if (!stopped) effectsController.setVignette(true);
    }
}
