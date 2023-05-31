using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DisruptionManager : MonoBehaviour {

    [SerializeField] private Transform disruptionCanvas;

    private TextMeshProUGUI disruptionText;

    private void Awake() {
        disruptionText = disruptionCanvas.GetComponentInChildren<TextMeshProUGUI>();

        int score = ScoreTracker.GetScore();
        disruptionText.text = GetDisruptionText(score)
                            + "\n\nKeeping your cat inside saves lives!";
    }

    private string GetDisruptionText(int score) {
        if (score < 32) {
            return "Too bad! Cats killed " + (32 - score) + " more birds than you in one minute!";
        } else if (score == 32) {
            return "You killed the same amount of birds as cats managed in one minute!";
        } else {
            return "Congratulations! You killed " + (score - 32) + " more birds than the cats!";
        }
    }
}
