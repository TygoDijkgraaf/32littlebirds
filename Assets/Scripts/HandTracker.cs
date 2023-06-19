using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class HandTracker : MonoBehaviour {

    [SerializeField] Transform handMarker;
    [SerializeField] TrackedHandJoint joint = TrackedHandJoint.Palm;

    private static Vector3? rightHandPosition = null;
    private static Vector3? leftHandPosition = null;

    private void Update() {
        // Continuously update the hand position and velocity
        if (HandJointUtils.TryGetJointPose(joint, Handedness.Right, out MixedRealityPose pose)) {
            rightHandPosition = pose.Position;
        } else {
            rightHandPosition = null;
        }

        if (HandJointUtils.TryGetJointPose(joint, Handedness.Left, out pose)) {
            leftHandPosition = pose.Position;
        } else {
            leftHandPosition = null;
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

    public static Vector3? GetLeftHandPosition() {
        if (leftHandPosition != null) {
            return (Vector3)leftHandPosition;
        }

        return null;
    }
}
