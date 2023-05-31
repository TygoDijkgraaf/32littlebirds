using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public static class MyGestures {
    public static bool StickFinger(Handedness handedness)
    {
        float indexTipY;
        MixedRealityPose pose;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, handedness, out pose)) {
            indexTipY = pose.Position.y;
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, handedness, out pose)) {
                if (Mathf.Abs(indexTipY - pose.Position.y) < 0.015f) {
                    if (HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, handedness, out pose)) {
                        if (pose.Position.y < indexTipY - 0.03f) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
