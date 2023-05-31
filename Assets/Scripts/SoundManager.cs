using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;


    private void Start() {
        SmashableBird.OnAnySmash += SmashableBird_OnAnySmash;
        DisruptionAnimation.OnBirdKilled += DisruptionAnimation_OnBirdKilled;
        DisruptionAnimation.OnCatLeap += DisruptionAnimation_OnCatLeap;
        HintTextUI.OnHintTextShown += HintTextUI_OnHintTextShown;
    }

    private void SmashableBird_OnAnySmash(object sender, SmashableBird.OnAnySmashEventArgs e) {
        PlaySound(audioClipsRefsSO.birdHit, e.smashPosition);
    }

    private void DisruptionAnimation_OnBirdKilled(object sender, DisruptionAnimation.OnBirdKilledEventArgs e) {
        PlaySound(audioClipsRefsSO.birdHit, e.position);
    }

    private void DisruptionAnimation_OnCatLeap(object sender, DisruptionAnimation.OnCatLeapEventArgs e) {
        PlaySound(audioClipsRefsSO.angryCat, e.catPosition);
    }

    private void HintTextUI_OnHintTextShown(object sender, HintTextUI.OnHintTextShownEventArgs e) {
        PlaySound(audioClipsRefsSO.hint, e.hintPosition);
    }

    public static void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public static void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f) {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }
}
