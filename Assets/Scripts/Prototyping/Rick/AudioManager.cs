using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] backgroundTracks;  // Array to hold background music tracks
    private AudioSource audioSource;
    private int currentTrackIndex = 0;    // Index to track current song

    public float fadeTime = 1;
    private bool musicStopped = false;


/*
    // Awake Is Only used for Testing
    public void Awake()
    {
        PlayMusic();
    }
*/

    // Start Playing music
    public void PlayMusic()
    {
        audioSource = GetComponent<AudioSource>(); // Audiosource on AudioManager GameObject

        if (backgroundTracks.Length > 0)
        {
            ShuffleTracks();
            PlayNextTrack();
        }
    }
    public void StopMusic()         // Call this function to stop the music from playing.
    {
        musicStopped = true;
        audioSource.Stop();        // Stop current track
        audioSource.clip = null;   // Optional: Clear the clip
    }


    public void Update()
    {
        if (!musicStopped && !audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        if (backgroundTracks.Length == 0) return;

        StartCoroutine(FadeAndPlay(backgroundTracks[currentTrackIndex]));
        currentTrackIndex = (currentTrackIndex + 1) % backgroundTracks.Length;
    }

    private void ShuffleTracks()
    {
        for (int i = 0; i < backgroundTracks.Length; i++)
        {
            int random = Random.Range(i, backgroundTracks.Length);
            AudioClip temp = backgroundTracks[i];
            backgroundTracks[i] = backgroundTracks[random];
            backgroundTracks[random] = temp;
        }
    }

    private IEnumerator FadeAndPlay(AudioClip newTrack)
    {
        audioSource.volume = 0f;
        audioSource.clip = newTrack;
        audioSource.Play();

        // Simple fade in over 1 second
        float time = 0f;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            audioSource.volume = time; // t goes from 0 to 1
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
