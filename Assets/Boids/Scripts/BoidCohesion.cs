using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidCohesion : MonoBehaviour {

    [SerializeField] private float cohesionRadius = 5f;
    private Boid boid;

    private void Start() {
        boid = GetComponent<Boid>();
    }

    private void Update() {
        Boid[] boids = boid.GetController().GetBoids();
        Vector3 averagePosition = Vector3.zero;
        int found = 0;
        foreach(Boid boid in boids.Where(b => b != boid)) {
            if (Vector3.Distance(transform.position, boid.transform.position) < cohesionRadius) {
                averagePosition += boid.transform.position;
                found++;
            }
        }

        if (found > 0) {
            averagePosition /= found;
            boid.velocity = Vector3.Lerp(boid.velocity, (averagePosition - transform.position).normalized, Time.deltaTime * 2f);
        }

    }
}
