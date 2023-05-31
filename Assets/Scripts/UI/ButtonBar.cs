using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class ButtonBar : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI questionText;


    public void ConfigureButtonBar(QuestionnaireManager.Question question) {

        int answers = question.answers.Length;

        if (answers < 2) {
            // If there are less than 2 answers, there is no point in asking the question
            Debug.LogWarning("Less than 2 answers, skipping question");
            QuestionnaireManager.Instance.NextQuestion();
        } else if (answers > 5) {
            // If there are more than 5 answers, only the first 5 will be considered
            Debug.LogWarning("More than 5 answers, only the first 5 will be considered");
            answers = 5;
        }

        questionText.text = question.question;

        Interactable[] buttons = GetComponentsInChildren<Interactable>();

        for (int i = 0; i < answers; i++) {
            int index = i;
            buttons[i].OnClick.AddListener(() => { QuestionnaireManager.Instance.ButtonPress(index); });
            buttons[i].GetComponentInChildren<TextMeshPro>().text = question.answers[i];
        }
    }
}
