using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dontdestroymyboundspls : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
