using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherEmitter : MonoBehaviour {

    [SerializeField] private Transform featherEmitterPrefab;
    [SerializeField] private int numberOfFeatherEmitters = 5;

    private Transform[] featherEmitters;

    private void Awake() {
        featherEmitters = new Transform[numberOfFeatherEmitters];
        InstantiateFeatherEmitters();
    }

    private void Start() {
        SmashableBird.OnAnySmash += SmashableBird_OnAnySmash;
        DisruptionAnimation.OnBirdKilled += DisruptionAnimation_OnBirdKilled;
    }

    private void SmashableBird_OnAnySmash(object sender, SmashableBird.OnAnySmashEventArgs e) {
        EmitFeathers(e.smashPosition);
    }

    private void DisruptionAnimation_OnBirdKilled(object sender, DisruptionAnimation.OnBirdKilledEventArgs e) {
        EmitFeathers(e.position);
    }

    // Instantiate all the feather emitters
    private void InstantiateFeatherEmitters() {
        Transform featherEmitterTransform;
        for(int i = 0; i < numberOfFeatherEmitters; i++) {
            featherEmitterTransform = Instantiate(featherEmitterPrefab, Vector3.zero, Quaternion.identity);
            featherEmitterTransform.parent = transform;
            featherEmitterTransform.gameObject.SetActive(false);
            featherEmitters[i] = featherEmitterTransform;
        }
    }

    // Emit feathers from a given position
    public void EmitFeathers(Vector3 position) {
        // Iterate over all feather emitters and activate the first one that is not active
        foreach (Transform featherEmitterTransform in featherEmitters) {
            if (featherEmitterTransform != null && !featherEmitterTransform.gameObject.activeSelf) {
                featherEmitterTransform.transform.position = position;
                featherEmitterTransform.gameObject.SetActive(true);
                StartCoroutine(DeactivateFeathers(featherEmitterTransform));
                return;
            }
        }
    }

    // Deactivate the feather emitter after a short time
    private IEnumerator DeactivateFeathers(Transform featherEmitterTransform) {
        yield return new WaitForSeconds(4.5f);
        featherEmitterTransform.gameObject.SetActive(false);
    }
}
