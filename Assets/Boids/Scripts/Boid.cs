using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    public Vector3 velocity;
    private BoidController controller;

    private void Update() {
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }

    public void SetController(BoidController controller) {
        this.controller = controller;
    }

    public BoidController GetController() {
        return controller;
    }
}
