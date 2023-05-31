using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptionAnimation : MonoBehaviour {

    public static event EventHandler<OnCatLeapEventArgs> OnCatLeap;
    public class OnCatLeapEventArgs : EventArgs {
        public Vector3 catPosition;
    }

    public static event EventHandler<OnBirdKilledEventArgs> OnBirdKilled;
    public class OnBirdKilledEventArgs : EventArgs {
        public Vector3 position;
    }


    private enum AnimationState {
        Flying,
        WaitingForCat,
        CatLeaping,
        Finished
    }

    private const string FLYING = "flying";
    private const string LEAPING = "Leaping";

    [SerializeField] private Transform birdPrefab;
    [SerializeField] private Transform birdSpawnPoint;
    [SerializeField] private Transform birdLandPosition;
    [SerializeField] private Transform cat;
    [SerializeField] private Transform catTarget;

    [SerializeField] private AnimationCurve catCurve;

    private Vector3 catStartPosition, catStartRotation;
    private Transform bird;
    private Animator birdAnimator;

    private AnimationState animationState;
    private float waitingForCatTimer = 3f;
    private float timeSinceCatLeaped = 0f;
    private float timeToLeap = 2f;
    private bool birdKilled = false;

    private void Awake() {
        cat.gameObject.SetActive(false);
        catStartPosition = cat.position;
        catStartRotation = cat.localEulerAngles;
        animationState = AnimationState.Flying;
        SpawnBird();
    }

    private void Update() {
        switch (animationState) {
            case AnimationState.Flying:
                bird.position = Vector3.MoveTowards(bird.position, birdLandPosition.position, .5f * Time.deltaTime);
                bird.LookAt(birdLandPosition);

                // Check if the bird has landed
                // Use sqrMagnitude for performance
                if ((bird.position - birdLandPosition.position).sqrMagnitude < .01f) {
                    bird.position = birdLandPosition.position;
                    bird.rotation = birdLandPosition.rotation;
                    birdAnimator.SetBool(FLYING, false);
                    animationState = AnimationState.WaitingForCat;
                }
                break;

            case AnimationState.WaitingForCat:
                waitingForCatTimer -= Time.deltaTime;
                if (waitingForCatTimer < 0f) {
                    animationState = AnimationState.CatLeaping;
                    cat.gameObject.SetActive(true);
                    OnCatLeap?.Invoke(this, new OnCatLeapEventArgs {
                        catPosition = cat.position
                    });
                }
                break;

            case AnimationState.CatLeaping:
                PlayCatAnimation();
                break;

            case AnimationState.Finished:
                // Do nothing
                break;
        }
    }

    private void SpawnBird() {
        bird = Instantiate(birdPrefab, birdSpawnPoint.position, Quaternion.identity);
        birdAnimator = bird.GetComponent<Animator>();
        birdAnimator.SetBool(FLYING, true);
    }

    private void PlayCatAnimation() {
        timeSinceCatLeaped += Time.deltaTime;

        Vector3 position = Vector3.Lerp(catStartPosition, catTarget.position, timeSinceCatLeaped / timeToLeap);
        position.y = catStartPosition.y + catCurve.Evaluate(timeSinceCatLeaped / timeToLeap);
        cat.position = position;

        if (timeSinceCatLeaped >= timeToLeap) {
            Destroy(cat.gameObject);
            animationState = AnimationState.Finished;
            return;
        }

        cat.localEulerAngles = new Vector3(Mathf.Lerp(catStartRotation.x, catStartRotation.x + 60f, timeSinceCatLeaped / timeToLeap),
                                           catStartRotation.y, catStartRotation.z);

        // Check if the cat is close to the bird and kill it
        // Use sqrMagnitude for performance
        if (!birdKilled && (cat.position - birdLandPosition.position).sqrMagnitude < .01f) {
            Destroy(bird.gameObject);
            birdKilled = true;
            OnBirdKilled?.Invoke(this, new OnBirdKilledEventArgs {
                position = birdLandPosition.position
            });
        }
    }

    public static void ResetStaticData() {
        OnCatLeap = null;
        OnBirdKilled = null;
    }
}
