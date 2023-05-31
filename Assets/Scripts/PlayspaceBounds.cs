using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Currently playspacebounds object is destroyed, but visuals stay for testing purposes.
//! This causes problems with reset. Destroy all visuals in final version!

public class PlayspaceBounds : MonoBehaviour {

    public struct Playspace {
        public float xMin, xMax;
        public float zMin, zMax;
        public float yMin, yMax;
        public Vector3 center;
        public float volume;

        public Playspace(float xMin, float xMax, float zMin, float zMax, float yMin) {
            this.xMin = xMin;
            this.xMax = xMax;
            this.zMin = zMin;
            this.zMax = zMax;
            this.yMin = yMin;
            this.yMax = 1.5f;

            this.center = new Vector3((xMin + xMax) / 2, (yMin + .5f) / 2, (zMin + zMax) / 2);
            this.volume = (xMax - xMin) * (yMax - yMin) * (zMax - zMin);
        }
    }

    [SerializeField] private Transform xPlaneMin;
    [SerializeField] private Transform xPlaneMax;
    [SerializeField] private Transform zPlaneMin;
    [SerializeField] private Transform zPlaneMax;
    [SerializeField] private Transform floorPlane;

    private Vector3? maybeRightHandPosition;
    private Vector3 rightHandPosition;
    private static float xMin = -1f, xMax = 1f, zMin = -1f, zMax = 1f;
    private static float yMin = -.5f;

    private void Update() {
        maybeRightHandPosition = HandTracker.GetRightHandPosition();
        if (maybeRightHandPosition == null) {
            return;
        }

        rightHandPosition = (Vector3)maybeRightHandPosition;

        if (rightHandPosition.x < xMin) {
            xMin = rightHandPosition.x;
            xPlaneMin.position = new Vector3(xMin, 0, 0);
        }

        if (rightHandPosition.x > xMax) {
            xMax = rightHandPosition.x;
            xPlaneMax.position = new Vector3(xMax, 0, 0);
        }

        if (rightHandPosition.z < zMin) {
            zMin = rightHandPosition.z;
            zPlaneMin.position = new Vector3(0, 0, zMin);
        }

        if (rightHandPosition.z > zMax) {
            zMax = rightHandPosition.z;
            zPlaneMax.position = new Vector3(0, 0, zMax);
        }

        if (rightHandPosition.y < yMin) {
            yMin = rightHandPosition.y;
            floorPlane.position = new Vector3(0, yMin, 0);
        }
    }

    public static Playspace GetPlayspace() {
        return new Playspace(xMin, xMax, zMin, zMax, yMin);
    }

    public static void ResetStaticData() {
        xMin = -1f;
        xMax = 1f;
        zMin = -1f;
        zMax = 1f;
        yMin = -.5f;
    }
}
