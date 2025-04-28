using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSource Audio;
    [SerializeField] private bool repeat;
    private bool AudioPlayed = false;

    public void Play() {
        if (Audio.loop) {
            return;
        }
        if (repeat) {
            Audio.Play();
            return;
        }
        if (!AudioPlayed) {
            Audio.Play();
            AudioPlayed = true;
        }
    }

    public void Pause() {
        Audio.Pause();
    }
}
