using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static SpawnManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    List<SpawnPointBehaviour> spawnPoints;
    //List<PlayerController> players;
    
    // Start is called before the first frame update
    void Start()
    {
        FindSpawnPoints();
    }

    void FindSpawnPoints()
    {
        spawnPoints = new List<SpawnPointBehaviour>(FindObjectsOfType<SpawnPointBehaviour>());
    }

    public void RespawnPlayer(PlayerController player)
    {
        if (spawnPoints == null)
        {
            FindSpawnPoints();
        }

        List<SpawnPointBehaviour> points = new List<SpawnPointBehaviour>();

        foreach (SpawnPointBehaviour point in spawnPoints)
        {
            points.Add(point);
        }

        bool findingUnoccupiedSpawnPoint = true;

        while (findingUnoccupiedSpawnPoint)
        {
            int rand = Random.Range(0, points.Count);

            if (!points[rand].occupied)
            {
                StartCoroutine(points[rand].Spawn(player));
                findingUnoccupiedSpawnPoint = false;
            }
            else
            {
                points.RemoveAt(rand);
            }

            // if all spawn points are occupied just pick one 
            if (points.Count <= 0)
            {
                rand = Random.Range(0, spawnPoints.Count);

                spawnPoints[rand].Spawn(player);

                findingUnoccupiedSpawnPoint = false;
            }
        }

    }
}
