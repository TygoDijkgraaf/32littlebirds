using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class GestureController : MonoBehaviour
{

    [SerializeField] private GameObject birdPerch;
    [SerializeField] private lb_BirdController birdController;

    private GameObject perchObject;
    private GameObject perchedBird;
    private float indexTipY;
    private MixedRealityPose pose;
    private Vector3 prevPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        perchObject = Instantiate(birdPerch, this.transform);
        perchObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        perchObject.SetActive(false);

        // if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose)) {
        //     indexTipY = pose.Position.y;
        //     if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Right, out pose)) {
        //         if (Mathf.Abs(indexTipY - pose.Position.y) < 0.01f) {
        //             if (HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, Handedness.Right, out pose)) {
        //                 if (pose.Position.y < indexTipY - 0.03f) {
        //                     Debug.Log("Bird incoming!");
        //                     if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexMiddleJoint, Handedness.Right, out pose)) {
        //                         perchObject.SetActive(true);
        //                         perchObject.transform.position = pose.Position;
        //                         perchObject.transform.rotation = pose.Rotation;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }

        if (MyGestures.StickFinger(Handedness.Right)) {
            Debug.Log("Bird incoming!");
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexMiddleJoint, Handedness.Right, out pose)) {
                perchObject.SetActive(true);
                perchObject.transform.position = pose.Position;
                perchObject.transform.rotation = pose.Rotation;
            }
        }

        // TODO: figure out how to detect bird has landed
    }
}
