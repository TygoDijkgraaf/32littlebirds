using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

public class LoadSceneButton : MonoBehaviour {

    [SerializeField] private Loader.Scene scene;

    private void Awake() {
        Interactable button = GetComponent<Interactable>();
        button.OnClick.AddListener(() => { Loader.Load(scene); });
    }
}
