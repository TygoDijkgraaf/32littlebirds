using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileWriter : MonoBehaviour {

    private static StreamWriter writer;
    private static string filePath;

    private void Awake() {
        string fileName = "answers.csv";
        filePath = Application.persistentDataPath + "\\" + fileName;
    }

    public static void WriteFile(int[] answers) {
        if (!File.Exists(filePath)) {
            writer = new StreamWriter(filePath, true);
        } else {
            writer = File.AppendText(filePath);
        }

        if (new FileInfo(filePath).Length == 0) {
            WriteHeader(answers.Length);
        }

        WriteAnswers(answers);

        writer.Flush();
        writer.Close();
    }

    private static void WriteHeader(int answers) {
        writer.Write("Score,");

        for (int i = 0; i < answers; i++) {
            writer.Write("Q" + (i + 1));
            if (i < answers - 1) {
                writer.Write(",");
            }
        }

        writer.Write("\n");
    }

    private static void WriteAnswers(int[] answers) {
        writer.Write(ScoreTracker.GetScore() + ",");

        for (int i = 0; i < answers.Length; i++) {
            writer.Write(answers[i]);
            if (i < answers.Length - 1) {
                writer.Write(",");
            }
        }

        writer.Write("\n");
    }
}
