using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireManager : MonoBehaviour {

    [Serializable]
    public struct Question {
        public string question;

        [Tooltip("Questions with less than 2 answers will not be asked. Only the first 5 answers will be considered.")]
        public string[] answers;
    }


    public static QuestionnaireManager Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private Transform pressableButtonBarPrefab2Answers;
    [SerializeField] private Transform pressableButtonBarPrefab3Answers;
    [SerializeField] private Transform pressableButtonBarPrefab4Answers;
    [SerializeField] private Transform pressableButtonBarPrefab5Answers;
    [SerializeField] private Transform thankYouPrefab;

    [Header("Questions & Answers")]
    [SerializeField] private Question[] questions;

    private Transform pressableButtonBar;
    private ButtonBar buttonBar;
    private int questionIndex = 0;
    private int[] userAnswers;

    private void Awake() {
        Instance = this;

        if (questions.Length == 0) {
            Debug.LogWarning("No questions");
            return;
        }

        userAnswers = new int[questions.Length];

        InstantiateButtonBar();
    }

    private void InstantiateButtonBar() {
        Transform pressableButtonBarPrefab = null;
        int answers = Mathf.Clamp(questions[questionIndex].answers.Length, 2, 5);

        switch (answers) {
            case 2:
                pressableButtonBarPrefab = pressableButtonBarPrefab2Answers;
                break;
            case 3:
                pressableButtonBarPrefab = pressableButtonBarPrefab3Answers;
                break;
            case 4:
                pressableButtonBarPrefab = pressableButtonBarPrefab4Answers;
                break;
            case 5:
                pressableButtonBarPrefab = pressableButtonBarPrefab5Answers;
                break;
        }

        pressableButtonBar = Instantiate(pressableButtonBarPrefab, new Vector3(0f, 0f, .45f), Quaternion.Euler(12f, 0f, 0f));

        buttonBar = pressableButtonBar.GetComponent<ButtonBar>();
        buttonBar.ConfigureButtonBar(questions[questionIndex]);
    }

    public void NextQuestion() {
        Destroy(pressableButtonBar.gameObject);
        questionIndex++;

        if (questionIndex >= questions.Length) {
            Debug.Log("End of the questions");

            Instantiate(thankYouPrefab, new Vector3(0f, 0f, .45f), Quaternion.Euler(12f, 0f, 0f));

            FileWriter.WriteFile(userAnswers);
            return;
        }

        InstantiateButtonBar();
    }

    public void ButtonPress(int index) {
        userAnswers[questionIndex] = index;
        NextQuestion();
    }
}
