using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField]
    PlayerController originalPlayer;
    [SerializeField]
    PlayerController targetPlayer;
    [SerializeField]
    Rigidbody2D rBod;

    [SerializeField]
    Vector2 launchVector;
    [SerializeField]
    float launchSpeed;

    [SerializeField]
    float travelSpeed;
    [SerializeField]
    float turnSpeed;

    [SerializeField]
    float knockback;
    [SerializeField]
    float damage;

    //bool launchState = true;

    // Start is called before the first frame update
    void Start()
    {
        rBod = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Launch(PlayerController player, Vector2 direction)
    {
        if (rBod == null)
        {
            rBod = GetComponent<Rigidbody2D>();
        }

        if (direction == Vector2.zero)
        {
            direction = new Vector2(player.transform.localScale.x, 0f);
        }
        transform.up = direction;

        originalPlayer = player;
        targetPlayer = null;
        gameObject.SetActive(true);
        transform.position = player.transform.position + (new Vector3(direction.x, direction.y, 0f) * 2f);
        launchVector = direction;
        rBod.velocity = launchVector * launchSpeed;
    }

    private void Move()
    {
        if (targetPlayer == null)
        {
            return;
        }

        Vector2 targetDirection = targetPlayer.transform.position - transform.position;
        targetDirection.Normalize();

        float rotation = Vector3.Cross(transform.up, targetDirection).z;

        rBod.angularVelocity = rotation * turnSpeed;
        rBod.velocity = transform.up * launchSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().GetHit(transform.position, knockback, damage);
        }
        ItemManager.Instance.ReturnMissile(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        if (targetPlayer != null)
        {
            return;
        }

        if (collision.GetComponent<PlayerController>() == originalPlayer)
        {
            return;
        }
        
        targetPlayer = collision.GetComponent<PlayerController>();
       
    }
}
