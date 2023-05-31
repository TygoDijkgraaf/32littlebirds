using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAudio : MonoBehaviour {

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private float birdSongDelay;
    private float minBirdSongDelay = 1f;
    private float maxBirdSongDelay = 10f;

    private void Awake() {
        birdSongDelay = Random.Range(minBirdSongDelay, maxBirdSongDelay);
    }

    private void Update() {
        birdSongDelay -= Time.deltaTime;
        if (birdSongDelay <= 0f) {
            SoundManager.PlaySound(audioClipsRefsSO.birdSongs, BirdController.Instance.GetRandomBirdPosition(), .5f);
            birdSongDelay = Random.Range(minBirdSongDelay, maxBirdSongDelay);
        }
    }
}
