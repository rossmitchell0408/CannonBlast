using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    /****************Singleton*****************/
    public static FlagManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    List<FlagSpawnBehaviour> spawnPoints;
    [SerializeField]
    List<FlagBehaviour> flags = new List<FlagBehaviour>();
    [SerializeField]
    int minimumFlagCount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        //BeginGame();
    }

    public void BeginGame()
    {
        FindSpawnPoints();
        CheckFlagCount();
    }

    public void EndGame()
    {
        foreach (FlagBehaviour flag in flags)
        {
            Destroy(flag.gameObject);
        }

        flags.Clear();
    }

    void FindSpawnPoints()
    {
        spawnPoints = new List<FlagSpawnBehaviour>(FindObjectsOfType<FlagSpawnBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFlag()
    {
        if (spawnPoints == null)
        {
            FindSpawnPoints();
        }

        int rand = Random.Range(0, spawnPoints.Count);

        spawnPoints[rand].SpawnFlag();
    }

    public void AddFlag(FlagBehaviour flag)
    {
        flags.Add(flag);
    }

    public void RemoveFlag(FlagBehaviour flag)
    {
        if (flags.Contains(flag))
        {
            flags.Remove(flag);
        }

        CheckFlagCount();
    }

    void CheckFlagCount()
    {
        if (flags.Count < minimumFlagCount)
        {
            SpawnFlag();
        }
    }
}
