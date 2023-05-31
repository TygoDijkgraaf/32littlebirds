using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {

    private void Awake() {
        ScoreTracker.ResetStaticData();
        SmashableBird.ResetStaticData();
        HintTextUI.ResetStaticData();
        DisruptionAnimation.ResetStaticData();
        PlayspaceBounds.ResetStaticData();
    }
}
