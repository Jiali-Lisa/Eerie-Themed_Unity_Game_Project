using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class ChasingTipsManager : MonoBehaviour
{
    [SerializeField] private PlayerInteract player;
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject tipsContainer;
    [SerializeField] private GameObject T;
    [SerializeField] private AudioSource Audio;
    private Animator animator;
    private bool allowTips = false;
    private bool hidden = true;
    private bool animatingPaper = false;

    private void Start()
    {
        paper.SetActive(false);
        foreach (Transform child in tipsContainer.transform) {
            child.gameObject.SetActive(false);
        }
        T.SetActive(true);
        allowTips = false;
        animator = paper.GetComponent<Animator>();
        paper.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !animatingPaper)
        {
            if (!hidden) {
                HideTips();
                return;
            }
            ShowTips();
        }
    }

    public void HideTips()
    {
        if (!allowTips || hidden) return;
        animatingPaper = true;
        StartCoroutine(Roll());
        hidden = true;
    }

    public void ShowTips()
    {
        if (!allowTips || player.IsUsingItem()) return;
        paper.SetActive(true);
        animatingPaper = true;
        StartCoroutine(Unroll());
        hidden = false;
    }

    private IEnumerator Unroll() {
        if (animator != null) animator.SetTrigger("Unroll");
        if (Audio != null && !Audio.isPlaying) Audio.Play();
        yield return new WaitForSeconds(1);
        animatingPaper = false;
    }

    private IEnumerator Roll() {
        if (animator != null) animator.SetTrigger("Roll");
        if (Audio != null && !Audio.isPlaying) Audio.Play();
        yield return new WaitForSeconds(1);
        paper.SetActive(false);
        animatingPaper = false;
    }

    public void setTips(GameObject[] tips, bool on) {
        foreach (GameObject tip in tips) {
            tip.gameObject.SetActive(on);
        }
    }

    public void setAllowTips(bool value) {
        allowTips = value;
    }
}
