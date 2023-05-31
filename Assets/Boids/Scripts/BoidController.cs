using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour {

    [SerializeField] private int boidCount = 5;
    [SerializeField] private GameObject boidPrefab;

    private Boid[] boids;

    private void Awake() {
        SpawnBoids();
    }

    private void SpawnBoids() {
        boids = new Boid[boidCount];
        for (int i = 0; i < boidCount; i++) {
            boids[i] = Instantiate(boidPrefab, new Vector3(Random.Range(-1f, 1f), 0f, 0f), Quaternion.identity).GetComponent<Boid>();
            boids[i].SetController(this);
        }
    }

    public Boid[] GetBoids() {
        return boids;
    }
}
