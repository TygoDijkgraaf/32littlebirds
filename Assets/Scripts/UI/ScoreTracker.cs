using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour {

    public static ScoreTracker Instance { get; private set; }

    private const string POPUP = "Popup";

    [SerializeField] private Transform scoreCanvas;

    private TextMesh scoreText;
    private Animator animator;
    private static int score = 0;

    private void Awake() {
        Instance = this;
        // DontDestroyOnLoad(this);

        scoreText = scoreCanvas.GetComponentInChildren<TextMesh>();
        animator = scoreCanvas.GetComponentInChildren<Animator>();
        UpdateScoreText();
    }

    private void Start() {
        SmashableBird.OnAnySmash += SmashableBird_OnAnySmash;
    }

    private void SmashableBird_OnAnySmash(object sender, SmashableBird.OnAnySmashEventArgs e) {
        score++;
        animator.SetTrigger(POPUP);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = "Score: " + score;
    }

    public static int GetScore() {
        return ScoreTracker.score;
    }

    public static void ResetStaticData() {
        score = 0;
    }
}
