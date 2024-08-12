using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static ItemSpawnManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    List<ItemSpawnPointBehaviour> spawnPoints;
    List<ItemPickup> items;

    [SerializeField]
    int maxItems = 3;

    float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<ItemPickup>();
        //StartCoroutine(RunItemSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGame()
    {
        StartCoroutine(RunItemSpawner());
    }

    public void EndGame()
    {
        StopCoroutine(RunItemSpawner());
    }

    void FindSpawnPoints()
    {
        spawnPoints = new List<ItemSpawnPointBehaviour>(FindObjectsOfType<ItemSpawnPointBehaviour>());
    }

    public void AddItem(ItemPickup item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemPickup item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    IEnumerator RunItemSpawner()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            FindSpawnPoints();
        }

        spawnTime = Random.Range(3f, 10f);

        yield return new WaitForSeconds(spawnTime);

        if (items.Count >= maxItems)
        {
            yield return StartCoroutine(RunItemSpawner());
            yield return null;
        }

        ItemSpawnPointBehaviour spawnPoint = SelecteSpawnPoint();

        spawnPoint.SpawnItem();

        yield return StartCoroutine(RunItemSpawner());
    }

    private ItemSpawnPointBehaviour SelecteSpawnPoint()
    {
        List<ItemSpawnPointBehaviour> availablePoints = new List<ItemSpawnPointBehaviour>();
        foreach (ItemSpawnPointBehaviour spawnPoint in spawnPoints)
        {
            if (!spawnPoint.occupied)
            {
                availablePoints.Add(spawnPoint);
            }
        }

        // None available do not spawn item
        if (availablePoints.Count == 0)
        {
            return null;
        }

        int rand = Random.Range(0, availablePoints.Count);

        return availablePoints[rand];
    }
}
