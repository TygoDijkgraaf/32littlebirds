using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAfterDelay : MonoBehaviour {

    private enum SpawnOption {
        Spawn,
        Despawn
    }

    [SerializeField] private Transform objectToSpawn;
    [SerializeField] private float delay;
    [SerializeField] private SpawnOption spawnOption;

    private bool spawned;

    private void Awake() {
        switch (spawnOption) {
            case SpawnOption.Spawn:
                spawned = false;
                break;
            case SpawnOption.Despawn:
                spawned = true;
                break;
        }

        objectToSpawn.gameObject.SetActive(spawned);
        StartCoroutine(SpawnDespawn());
    }

    private IEnumerator SpawnDespawn() {
        yield return new WaitForSeconds(delay);
        objectToSpawn.gameObject.SetActive(!spawned);
    }
}
