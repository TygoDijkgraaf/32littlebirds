using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour {

    private const string SPAWN = "Spawn";

    [SerializeField] private Transform button;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float delay;

    private float timeUntillSpawn;

    private void Awake() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        yield return new WaitForSeconds(delay);
        Instantiate(button, spawnPoint);
    }
}
