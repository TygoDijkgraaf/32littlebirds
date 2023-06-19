using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3? maybeRightHandPosition, maybeLeftHandPosition;
    private Vector3 rightHandPosition, leftHandPosition, maxHandPosition, minHandPosition;
    private static float xMin = -1f, xMax = 1f, zMin = -1f, zMax = 1f;
    private static float yMin = -.5f;

    private void Update() {
        maybeRightHandPosition = HandTracker.GetRightHandPosition();
        maybeLeftHandPosition = HandTracker.GetLeftHandPosition();

        // Check if hands are available. If not, set to zero as to not interfere
        if (maybeRightHandPosition != null) {
            rightHandPosition = (Vector3)maybeRightHandPosition;
        } else {
            rightHandPosition = new Vector3(0, 0, 0);
        }

        if (maybeLeftHandPosition != null) {
            leftHandPosition = (Vector3)maybeLeftHandPosition;
        } else {
            leftHandPosition = new Vector3(0, 0, 0);
        }

        // Get the min and max hand positions
        minHandPosition = Vector3.Min(rightHandPosition, leftHandPosition);
        maxHandPosition = Vector3.Max(rightHandPosition, leftHandPosition);

        // Adjust the bounds if necessary
        if (minHandPosition.x < xMin) {
            xMin = minHandPosition.x;
            xPlaneMin.position = new Vector3(xMin, 0, 0);
        }

        if (maxHandPosition.x > xMax) {
            xMax = maxHandPosition.x;
            xPlaneMax.position = new Vector3(xMax, 0, 0);
        }

        if (minHandPosition.z < zMin) {
            zMin = minHandPosition.z;
            zPlaneMin.position = new Vector3(0, 0, zMin);
        }

        if (maxHandPosition.z > zMax) {
            zMax = maxHandPosition.z;
            zPlaneMax.position = new Vector3(0, 0, zMax);
        }

        if (minHandPosition.y < yMin) {
            yMin = minHandPosition.y;
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
