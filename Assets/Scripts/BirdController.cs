using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    public static BirdController Instance { get; private set; }

    [Tooltip("The minimum number of birds that can be spawned.")]
    [SerializeField] private int minBirds = 5;
    [Tooltip("The maximum number of birds that can be spawned.")]
    [SerializeField] private int maxBirds = 20;
    [Tooltip("The amount of birds spawned per cubic meter.")]
    [SerializeField] private float birdDensity = 1f;
    [Tooltip("The prefabs of the birds that can be spawned.")]
    [SerializeField] private GameObject[] birdPrefabs;


    private GameObject[] birds;

    private static PlayspaceBounds.Playspace playspace;

    private void Awake() {
        Instance = this;

        playspace = PlayspaceBounds.GetPlayspace();
        InstantiateBirds();
    }

    private void InstantiateBirds() {
        int numBirds = Mathf.RoundToInt(birdDensity * playspace.volume);
        numBirds = Mathf.Clamp(numBirds, minBirds, maxBirds);

        birds = new GameObject[numBirds];

        GameObject bird;
        for (int i = 0; i < numBirds; i++) {
            bird = Instantiate(birdPrefabs[Random.Range(0, birdPrefabs.Length)],
                               RandomBirdSpawnPos(), Quaternion.Euler(0, Random.Range(0, 360), 0));
            bird.transform.parent = transform;
            birds[i] = bird;
        }
    }

    public Vector3 GetRandomBirdPosition() {
        return birds[Random.Range(0, birds.Length)].transform.position;
    }

    public static Vector3 RandomBirdSpawnPos() {
        float x = Random.Range(playspace.xMin, playspace.xMax);
        float y = Random.Range(playspace.yMin, playspace.yMax - 1f);
        float z = Random.Range(playspace.zMin, playspace.zMax);

        return new Vector3(x, y, z);

    }
}
