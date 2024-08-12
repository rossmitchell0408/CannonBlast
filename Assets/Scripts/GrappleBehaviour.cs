using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBehaviour : MonoBehaviour
{
    Rigidbody2D rBod;
    [SerializeField]
    float launchSpeed = 20f;
    [SerializeField]
    float returnSpeed = 30f;
    [SerializeField]
    Vector3 launchAngle;
    [SerializeField]
    float launchOffset;

    //bool canGrapple = true;
    //bool isGrappling = false;
    bool hasContact = false;
    //float grappleTime = 3f;

    [SerializeField]
    float grappleTimer = 1.5f;

    //bool launching = false;
    //bool returning = false;
    //bool holding = false;
    //bool resting = true;

    float grappleDistance;
    [SerializeField]
    LaunchState launchState = LaunchState.RESTING;

    PlayerController player;

    [SerializeField]
    float tugPower;

    enum LaunchState
    {
        RESTING,
        LAUNCHING,
        HOLDING,
        RETURNING
    }

    // Start is called before the first frame update
    void Awake()
    {
        //player = transform.parent.GetComponent<PlayerController>();
        rBod = GetComponent<Rigidbody2D>();
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public bool GrappleReady()
    {
        if (launchState == LaunchState.RESTING)
        {
            return true;
        }
        return false;
    }

    public bool PullReady()
    {
        if (launchState == LaunchState.HOLDING)
        {
            //Debug.Log("Holding");
            return true;
        }
        return false;
    }

    private void Move()
    {
        switch (launchState)
        {
            case LaunchState.RESTING:
                break;
            case LaunchState.LAUNCHING:
                //transform.position += launchAngle.normalized * launchSpeed * Time.deltaTime;
                break;
            case LaunchState.HOLDING:
                rBod.velocity = Vector2.zero;
                player.FollowGrapple();
                break;
            case LaunchState.RETURNING:
                Vector3 direction = player.transform.position - transform.position;
                //transform.position += direction.normalized * launchSpeed * Time.deltaTime;
                rBod.velocity = direction * returnSpeed;
                break;
        }
    }

    public void LaunchGrapple()
    {
        gameObject.SetActive(true);
        transform.position = player.transform.position;
        launchAngle.x = player.transform.localScale.x;
        launchState = LaunchState.LAUNCHING;
    }

    public void LaunchGrapple(Vector2 direction)
    {
        gameObject.SetActive(true);
        //launchAngle.x = player.transform.localScale.x;
        if (direction == Vector2.zero)
        {
            direction = new Vector2(player.transform.localScale.x, 0f);
        }
        launchAngle = direction.normalized;
        transform.position = player.transform.position + launchAngle * launchOffset;
        launchState = LaunchState.LAUNCHING;
        rBod.velocity = direction * launchSpeed;
        StartCoroutine(LaunchGrappleTimer());
    }


    public void RetractGrapple()
    {
        if (launchState == LaunchState.RESTING)
        {
            return;
        }
        launchState = LaunchState.RETURNING;
        hasContact = false;
        StartCoroutine(ReturnGrappleTimer());
        //Debug.Log("Returning");
    }

    IEnumerator LaunchGrappleTimer()
    {
        yield return new WaitForSeconds(grappleTimer);
        if (launchState == LaunchState.LAUNCHING)
        {
            RetractGrapple();
        }
    }

    IEnumerator ReturnGrappleTimer()
    {
        yield return new WaitForSeconds(grappleTimer);
        if (launchState == LaunchState.RETURNING)
        {
            ReturnGrapple();
        }
    }

    void ReturnGrapple()
    {
        launchState = LaunchState.RESTING;
        gameObject.SetActive(false);
    }

    void TugPlayer(PlayerController target)
    {
        Vector2 tugDirection = (player.transform.position - target.transform.position).normalized;
        target.gameObject.GetComponent<PlayerController>().Stun();
        target.GetComponent<Rigidbody2D>().AddForce(tugDirection * tugPower, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit");
        if (launchState == LaunchState.RETURNING && collision.gameObject == player.gameObject)
        {
            //launchState = LaunchState.RESTING;
            //gameObject.SetActive(false);
            ReturnGrapple();
        }

        if (launchState != LaunchState.LAUNCHING)
        {
            return;
        }

        if (collision.gameObject.tag == "Ground")
        {
            launchState = LaunchState.HOLDING;
            hasContact = true;
            grappleDistance = (transform.position - player.transform.position).magnitude;
        }

        if (collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
        {
            TugPlayer(collision.gameObject.GetComponent<PlayerController>());
            RetractGrapple();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //Debug.Log("Hit");
    //    if (launchState == LaunchState.RETURNING && collision.gameObject == player.gameObject)
    //    {
    //        //launchState = LaunchState.RESTING;
    //        //gameObject.SetActive(false);
    //        ReturnGrapple();
    //    }

    //    if (launchState != LaunchState.LAUNCHING)
    //    {
    //        return;
    //    }

    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        launchState = LaunchState.HOLDING;
    //        hasContact = true;
    //        grappleDistance = (transform.position - player.transform.position).magnitude;
    //    }

    //    if (collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
    //    {
    //        TugPlayer(collision.gameObject.GetComponent<PlayerController>());
    //        RetractGrapple();
    //    }
    //}
}
