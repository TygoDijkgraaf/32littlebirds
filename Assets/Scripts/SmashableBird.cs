using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMover), typeof(Animator))]
public class SmashableBird : MonoBehaviour {

    private const string FLYING = "flying";

    public static event System.EventHandler<OnAnySmashEventArgs> OnAnySmash;
    public class OnAnySmashEventArgs : System.EventArgs {
        public Vector3 smashPosition;
    }


    private Vector3 handPosition;
    private Renderer birdRenderer;
    private Animator animator;
    private BirdMover birdMover;
    private bool isActive = true;

    private void Awake() {
        birdRenderer = GetComponentInChildren<Renderer>();
        birdMover = GetComponent<BirdMover>();
        animator = GetComponent<Animator>();
        animator.SetBool(FLYING, true);
    }

    private void Update() {
        birdRenderer.enabled = isActive;
        if (isActive) {
            float smashDistance = .15f;
            if (HandTracker.IsHandCloseToPoint(transform.position, smashDistance)) {
                SmashBird();
            }
        }
    }

    // Smash (deactivate) the bird
    private void SmashBird() {
        isActive = false;
        birdMover.enabled = false;

        OnAnySmash?.Invoke(this, new OnAnySmashEventArgs {
            smashPosition = transform.position
        });

        StartCoroutine(RespawnBird());
    }

    // Respawn the bird after a few seconds
    private IEnumerator RespawnBird() {
        float respawnTime = 5f;
        yield return new WaitForSeconds(respawnTime);
        gameObject.transform.position = BirdController.RandomBirdSpawnPos();
        isActive = true;
        birdMover.enabled = true;
    }

    public static void ResetStaticData() {
        OnAnySmash = null;
    }
}
