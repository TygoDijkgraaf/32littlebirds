using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ExperienceReset : MonoBehaviour {

    public void ResetExperience() {
        // Todo: figure out why resetting the experience causes the app to crash (textmesh destroyed?)
        // Destroy(ScoreTracker.Instance.gameObject);
        Loader.Load(Loader.Scene.MainMenuScene);
    }
}
