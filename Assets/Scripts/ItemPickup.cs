using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType GetRandomItem()
    {
        int rand = Random.Range(0, (int)ItemType.NUMBER_OF_ITEM_TYPES);
        ItemType item = (ItemType)rand;

        return ItemType.MISSILE;
        //return item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().GetNewItem(GetRandomItem());
            transform.parent.GetComponent<ItemSpawnPointBehaviour>().DespawnItem(this);
            Destroy(gameObject);
        }
    }
}
