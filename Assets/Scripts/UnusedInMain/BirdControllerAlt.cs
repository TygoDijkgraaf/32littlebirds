using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class BirdControllerAlt : MonoBehaviour
{
    [SerializeField] private GameObject[] birdPrefabs;
    [SerializeField] private GameObject featherEmitterPrefab, cageHitBox, fingerMarker;
    private Vector3 cageEdgesMin, cageEdgesMax;
    private GameObject bird, featherEmitter;
    private bool birdIncoming = false, birdOnFinger = false, birdFleeing = false;
    private Vector3 followPos, prevPos = Vector3.zero, birdSpawnPos = new Vector3(0, 2.5f, 3);
    private Quaternion followRot;
    private Handedness handedness = Handedness.Right;
    private float timeSinceLastBird = 0, birdDelta = 4.5f, birdSpeed = 0.005f;
    private MixedRealityPose pose;
    private Animator animator;
    private int flyingBoolHash;


    void Start()
    {
        // Instantiate the feather emitter to be used when a bird is killed (possibly)
        featherEmitter = Instantiate(featherEmitterPrefab, Vector3.zero, Quaternion.identity);
        featherEmitter.SetActive(false);

        // Instantiate the finger marker to indicate to the user when they are holding their finger in the correct position
        fingerMarker = Instantiate(fingerMarker, Vector3.zero, Quaternion.identity);
        fingerMarker.GetComponent<Renderer>().enabled = false;

        // Get the edges of the cage in the shape of a box (does not allow for rotation of the cage)
        Vector3 cageSize = cageHitBox.GetComponent<Renderer>().bounds.size;
        cageEdgesMin = cageHitBox.transform.position - cageSize / 2f;
        cageEdgesMax = cageHitBox.transform.position + cageSize / 2f;

        flyingBoolHash = Animator.StringToHash("flying");
    }

    void Update()
    {
        // Increase time since last bird was spawned
        timeSinceLastBird += Time.deltaTime;

        // Check if the user is holding their finger in the correct position
        bool stickFinger = MyGestures.StickFinger(handedness);

        // Show the marker
        fingerMarker.GetComponent<Renderer>().enabled = stickFinger;
        if (stickFinger) {
            // Get the position of the index tip for the marker
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, handedness, out pose)) {
                fingerMarker.transform.position = pose.Position;
            }

            // Spawn a bird if conditions are met
            if (!birdIncoming && timeSinceLastBird > birdDelta) {
                Debug.Log("Bird incoming!");
                SpawnBird();
            }
        } else if (birdIncoming) {
            // If user stops holding finger in correct position, bird flees
            BirdFlee();
        }

        // Get the position of the index middle joint for the bird to follow
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexMiddleJoint, handedness, out pose) && !birdFleeing) {
            followPos = pose.Position;
            followRot = pose.Rotation;
        }

        if (birdIncoming) {
            // If the hand moves too quickly, the bird flees
            if (Vector3.Distance(prevPos, followPos) > 0.0175f && prevPos != Vector3.zero && prevPos != birdSpawnPos) {
                BirdFlee();
            }

            if (birdOnFinger) {
                // Stop the flying animation
                animator.SetBool(flyingBoolHash, false);

                // If the bird is on the finger, it follows the finger
                bird.transform.position = followPos;
                bird.transform.rotation = Quaternion.Euler(0, followRot.eulerAngles.y - 90, 0);

                // Check if the user has moved the bird into the cage
                CheckCage();
            } else {
                // If the bird is not on the finger, it moves towards the finger
                bird.transform.position = Vector3.MoveTowards(bird.transform.position, followPos, birdSpeed);
                bird.transform.rotation = Quaternion.LookRotation(followPos - bird.transform.position);
            }

            if (birdFleeing && Vector3.Distance(bird.transform.position, followPos) < 0.05f) {
                // If bird is moving away from finger and has reached a location off screen, kill it
                KillBird();
            } else if (!birdOnFinger && Vector3.Distance(bird.transform.position, followPos) < 0.05f) {
                // If bird has reached the finger, set it on the finger
                birdOnFinger = true;
            }
        }

        prevPos = followPos;
    }

    // Spawn a random bird on the bird spawn location
    void SpawnBird()
    {
        bird = Instantiate(birdPrefabs[Random.Range(0, birdPrefabs.Length)], new Vector3(0, 2.5f, 3), Quaternion.identity);
        birdIncoming = true;

        // Immediately start the flying animation
        animator = bird.GetComponent<Animator>();
        animator.SetBool(flyingBoolHash, true);
    }

    // Kill the bird and reset the bird variables (emitting feathers or not)
    void KillBird() {
        if (birdIncoming) {
            // EmitFeathers(bird.transform.position);
            bird.transform.position = birdSpawnPos;
            Destroy(bird);
            birdIncoming = false;
            birdOnFinger = false;
            timeSinceLastBird = 0;
        }
    }

    // Make bird flee back to its spawn location
    void BirdFlee() {
        if (birdIncoming) {
            birdOnFinger = false;
            followPos = birdSpawnPos;
            birdFleeing = true;
            birdSpeed = 0.01f;
            animator.SetBool(flyingBoolHash, true);
            StartCoroutine("StopBirdFleeing");
        }
    }

    // Stop the bird from fleeing after a short time
    IEnumerator StopBirdFleeing() {
        yield return new WaitForSeconds(2f);
        birdFleeing = false;
        birdSpeed = 0.005f;
    }

    // Emit feathers at the given position
    void EmitFeathers(Vector3 pos) {
        featherEmitter.transform.position = pos;
        featherEmitter.SetActive(true);
        StartCoroutine("DeactivateFeathers");
    }

    // Deactivate the feather emitter after a short time
    IEnumerator DeactivateFeathers() {
        yield return new WaitForSeconds(birdDelta);
        featherEmitter.SetActive(false);
    }

    // If the bird is in the cage, add a point to the score and reset the bird variables
    void BirdInCage() {
        Debug.Log("Congratulations! You caught a bird!");
        // ScoreTracker.score++;
        bird.transform.position = birdSpawnPos;
        Destroy(bird);
        birdIncoming = false;
        birdOnFinger = false;
        timeSinceLastBird = 2f;
    }

    // Check if the bird is in the cage by checking if its position is within the cage edges
    void CheckCage() {
        Vector3 birdPos = bird.transform.position;
        bool inCage = true;
        for (int i = 0; i < 3; i++) {
            if (birdPos[i] < cageEdgesMin[i] || birdPos[i] > cageEdgesMax[i]) {
                inCage = false;
            }
        }

        if (inCage) {
            BirdInCage();
        }
    }
}
