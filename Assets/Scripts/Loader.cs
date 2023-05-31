using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        MainMenuScene,
        PlayspaceBoundsScene,
        GameScene,
        LoadingScene,
        DisruptionScene,
        QuestionnaireScene,
        Quit,
    }

    private static Scene targetScene;

    public static void Load(Scene scene) {
        if (scene == Scene.Quit) {
            Application.Quit();
            return;
        }

        Loader.targetScene = scene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
