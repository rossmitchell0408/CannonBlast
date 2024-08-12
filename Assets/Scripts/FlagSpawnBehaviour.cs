using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawnBehaviour : MonoBehaviour
{
    [SerializeField]
    FlagBehaviour flagPrefab;

    public void SpawnFlag()
    {
        FlagBehaviour flag = Instantiate(flagPrefab);
        flag.transform.position = transform.position;
        FlagManager.Instance.AddFlag(flag);
    }
}
