using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private const string COUNTDOWN = "Countdown";

    [SerializeField] private Transform timeCanvas;
    [SerializeField] private int secondsLeft = 60;

    private TextMesh timeText;

    private void Awake() {
        timeText = timeCanvas.GetComponentInChildren<TextMesh>();
        UpdateTimeText();
    }

    private void Start() {
        SmashableBird.OnAnySmash += SmashableBird_OnAnySmash;
    }

    private void SmashableBird_OnAnySmash(object sender, SmashableBird.OnAnySmashEventArgs e) {
        InvokeRepeating(COUNTDOWN, 1f, 1f);
        SmashableBird.OnAnySmash -= SmashableBird_OnAnySmash;
    }

    private void UpdateTimeText() {
        timeText.text = TimeSpan.FromSeconds(secondsLeft).ToString(@"mm\:ss");
    }

    private void Countdown() {
        secondsLeft--;
        UpdateTimeText();

        if (secondsLeft <= 0) {
            CancelInvoke(COUNTDOWN);
            Loader.Load(Loader.Scene.DisruptionScene);
        }
    }
}