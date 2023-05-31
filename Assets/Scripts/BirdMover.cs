using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : MonoBehaviour {

    private enum BirdState {
        Flying,
        Turning,
    }

    [SerializeField] private float moveSpeed = .3f;
    [SerializeField] private float turningSpeed = 4f;

    private BirdState state = BirdState.Flying;
    private float changeDirectionTimer, checkBoundsTimer = 1f;
    private Vector3 target;
    private PlayspaceBounds.Playspace playspace;
    private bool checkBounds = true;


    private void Awake() {
        changeDirectionTimer = Random.Range(1f, 3f);

        playspace = PlayspaceBounds.GetPlayspace();

        // Apply a buffer to the playspace bounds so the bird does not leave it when turning
        float turningRadius = moveSpeed / turningSpeed;
        float buffer = turningRadius + .1f;

        playspace.xMin += buffer;
        playspace.xMax -= buffer;
        playspace.zMin += buffer;
        playspace.zMax -= buffer;
        playspace.yMin += buffer;
    }

    private void Update() {
        switch (state) {
            case BirdState.Flying:
                BirdFly();
                break;
            case BirdState.Turning:
                BirdTurn();
                break;
        }
    }

    private void BirdFly() {
        MoveBird();
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0) {
            state = BirdState.Turning;
            target = transform.forward + Random.insideUnitSphere;
            return;
        }

        if (checkBoundsTimer > 0) {
            checkBoundsTimer -= Time.deltaTime;
        } else {
            checkBounds = true;
        }

        if (checkBounds) {
            CheckBounds();
        }
    }

    private void CheckBounds() {
        if (transform.position.x < playspace.xMin || transform.position.x > playspace.xMax ||
            transform.position.z < playspace.zMin || transform.position.z > playspace.zMax ||
            transform.position.y < playspace.yMin || transform.position.y > playspace.yMax) {

            // Set the birds 'target' back into the bounds of the scene
            state = BirdState.Turning;
            checkBounds = false;
            checkBoundsTimer = 1f;
            target = playspace.center - transform.position.normalized;
        }
    }

    private void BirdTurn() {
        MoveBird();
        RotateBird();
    }

    private void MoveBird() {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void RotateBird() {
        Quaternion lookRotation = Quaternion.LookRotation(target, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turningSpeed);

        if (transform.rotation == lookRotation) {
            state = BirdState.Flying;
            changeDirectionTimer = Random.Range(1f, 3f);
        }
    }
}
