using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{
    public bool occupied = false;
    float spawnDelay = 3f;

    public IEnumerator Spawn(PlayerController player)
    {
        occupied = true;
        player.Respawn();
        player.transform.position = transform.position;
        yield return new WaitForSeconds(spawnDelay);
        occupied = false;
    }
}
