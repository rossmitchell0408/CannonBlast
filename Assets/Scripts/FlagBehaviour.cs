using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBehaviour : MonoBehaviour
{
    //enum FlagState 
    //{
    //    DROPPED,
    //    CARRIED,
    //    SCORED
    //}

    //FlagState state = FlagState.DROPPED;

    PlayerController owningPlayer = new PlayerController();
    Rigidbody2D rBod;
    [SerializeField]
    float dropSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rBod = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DropFlag()
    {
        float x = Random.Range(-1, 1);
        float y = Random.Range(0.5f, 1);
        Vector2 direction = new Vector2(x, y);
        rBod.velocity = direction * dropSpeed;

        gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        owningPlayer = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Prevent player from immediately picking flag back up after dropping
            if (collision.GetComponent<PlayerController>() == owningPlayer)
            {
                return;
            }
            //FlagManager.Instance.SpawnFlag();
            //Destroy(gameObject);

            owningPlayer = collision.GetComponent<PlayerController>();
            owningPlayer.PickupFlag(this);
            gameObject.SetActive(false);
        }
    }
}
