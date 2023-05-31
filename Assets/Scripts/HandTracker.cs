using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class HandTracker : MonoBehaviour {

    // [SerializeField] Transform handMarker;
    // [SerializeField] Handedness handedness = Handedness.Right;

    // private static Vector3 handPosition = Vector3.zero;
    // private static Vector3 lastPosition = Vector3.zero;
    // private static float handVelocity;

    // private Transform marker;

    // private void Awake() {
    //     marker = Instantiate(handMarker);
    // }

    // private void Update() {

    //     // Continuously update the hand position and velocity
    //     if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, handedness, out MixedRealityPose pose)) {
    //         handPosition = pose.Position;
    //         marker.position = handPosition;
    //     } else {
    //         handPosition = Vector3.zero;
    //     }

    //     handVelocity = Vector3.Distance(handPosition, lastPosition) / Time.deltaTime;

    //     lastPosition = handPosition;
    // }

    // public static bool IsHandCloseToPoint(Vector3 point, float distance) {
    //     return (handPosition - point).sqrMagnitude < distance * distance;
    // }

    // // Getter methods for hand position and velocity
    // public static Vector3 GetHandPosition() {
    //     return handPosition;
    // }

    // public static float GetHandVelocity() {
    //     return handVelocity;
    // }


    [SerializeField] Transform handMarker;
    [SerializeField] TrackedHandJoint joint = TrackedHandJoint.Palm;

    private static Vector3? rightHandPosition = null;
    private static Vector3? leftHandPosition = null;

    private Transform markerR, markerL; // only for testing purposes

    private void Awake() {
        // markerR = Instantiate(handMarker); // only for testing purposes
        // markerL = Instantiate(handMarker); // only for testing purposes
    }

    private void Update() {

        // Continuously update the hand position and velocity
        if (HandJointUtils.TryGetJointPose(joint, Handedness.Right, out MixedRealityPose pose)) {
            // markerR.gameObject.SetActive(true); // only for testing purposes
            rightHandPosition = pose.Position;
            // markerR.position = (Vector3)rightHandPosition; // only for testing purposes
        } else {
            rightHandPosition = null;
            // markerR.gameObject.SetActive(false); // only for testing purposes
        }

        if (HandJointUtils.TryGetJointPose(joint, Handedness.Left, out pose)) {
            // markerL.gameObject.SetActive(true); // only for testing purposes
            leftHandPosition = pose.Position;
            // markerL.position = (Vector3)leftHandPosition; // only for testing purposes
        } else {
            leftHandPosition = null;
            // markerL.gameObject.SetActive(false); // only for testing purposes
        }

    }

    public static bool IsHandCloseToPoint(Vector3 point, float distance) {
        if (rightHandPosition != null && ((Vector3)rightHandPosition - point).sqrMagnitude < distance * distance) {
            return true;
        }

        if (leftHandPosition != null && ((Vector3)leftHandPosition - point).sqrMagnitude < distance * distance) {
            return true;
        }

        return false;
    }

    public static Vector3? GetRightHandPosition() {
        if (rightHandPosition != null) {
            return (Vector3)rightHandPosition;
        }

        return null;
    }

    // // Getter methods for hand position and velocity
    // public static Vector3 GetHandPosition() {
    //     return handPosition;
    // }

    // public static float GetHandVelocity() {
    //     return handVelocity;
    // }
}
