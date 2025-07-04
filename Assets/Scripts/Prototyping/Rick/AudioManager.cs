using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] CurrentMusicTracks;  // Array to hold background music tracks
    public AudioClip[] startScreenTracks; // Array to hold the music tracks for start screen
    public AudioClip[] backgroundTracks;   // Array to hold the music tracks for the background music

    public bool isGameplayMusic = false;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;    // Index to track current song

    public float fadeTime = 1;
    private bool musicStopped = false;



    // Awake Is Only used for Testing
    public void Awake()
    {
        PlayMusic();
    }


    // Start Playing music
    public void PlayMusic()
    {
        musicStopped = false;
        audioSource = GetComponent<AudioSource>(); // Audiosource on AudioManager GameObject

        if (isGameplayMusic)
        {
            CurrentMusicTracks = backgroundTracks;
        }
        else {
            CurrentMusicTracks = startScreenTracks; 
        }

        if (CurrentMusicTracks.Length > 0)
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
        if (CurrentMusicTracks.Length == 0) return;

        StartCoroutine(FadeAndPlay(CurrentMusicTracks[currentTrackIndex]));
        currentTrackIndex = (currentTrackIndex + 1) % CurrentMusicTracks.Length;
    }

    private void ShuffleTracks()
    {
        for (int i = 0; i < CurrentMusicTracks.Length; i++)
        {
            int random = Random.Range(i, CurrentMusicTracks.Length);
            AudioClip temp = CurrentMusicTracks[i];
            CurrentMusicTracks[i] = CurrentMusicTracks[random];
            CurrentMusicTracks[random] = temp;
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

    public void ChangeToBackgroundTracks()
    {
        StopMusic();        // stop current Track
        isGameplayMusic = true;
        PlayMusic();        // start new Tracks
    }
}
