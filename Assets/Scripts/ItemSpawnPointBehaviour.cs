using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPointBehaviour : MonoBehaviour
{
    public bool occupied = false;
    [SerializeField]
    ItemPickup itemPickupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem()
    {
        ItemPickup item = Instantiate(itemPickupPrefab);
        item.transform.parent = transform;
        item.transform.position = transform.position;
        ItemSpawnManager.Instance.AddItem(item);
        occupied = true;
    }

    public void DespawnItem(ItemPickup item)
    {
        occupied = false;
        ItemSpawnManager.Instance.RemoveItem(item);
    }
}
