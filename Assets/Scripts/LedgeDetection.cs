using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    public bool stepDetected = false;
    public bool ledgeDetected = false;
    public bool wallDetected = false;
    [SerializeField]
    LayerMask grabableLayers;

    [SerializeField]
    int numberOfRaycasts;

    PlayerController player;
    float playerHeight;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
        playerHeight = player.GetComponent<Collider2D>().bounds.size.y;
    }

    void Update()
    {
        CheckForLedge();
    }

    public Vector2 CheckForLedge()
    {
        if (player == null)
        {
            player = transform.parent.GetComponent<PlayerController>();
            playerHeight = player.GetComponent<Collider2D>().bounds.size.y;
        }

        Vector2 contactPoint = new Vector2();
        float upperOffset = playerHeight / 4;
        Vector2 startingPos = new Vector2(transform.position.x, transform.position.y + playerHeight / 2 + upperOffset);
        float offset = (playerHeight + upperOffset) / numberOfRaycasts;
        Color color = new Color();
        Vector2 direction = new Vector2(player.GetLookDirection().x, 0f);
        bool topRay = true;
        stepDetected = false;
        ledgeDetected = false;
        wallDetected = false;

        if (direction.x == 0)
        {
            direction = new Vector2(1 * player.transform.localScale.x, 0f);
        }

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            float distance = 1f;
            bool hit = Physics2D.Raycast(startingPos, direction, distance, grabableLayers);

            if (hit)
            {
                distance = Physics2D.Raycast(startingPos, direction, distance, grabableLayers).distance;
                
                if (i < numberOfRaycasts / 5)
                {
                    wallDetected = true;
                }
                else if (!wallDetected && i < numberOfRaycasts / 2)
                {
                    ledgeDetected = true;
                }
                else if (!wallDetected && !ledgeDetected && i >= numberOfRaycasts / 2)
                {
                    stepDetected = true;
                }
                

                if (topRay)
                {
                    topRay = false;
                    color = Color.blue;
                    contactPoint = startingPos + direction * distance;
                }
                else
                {
                    color = Color.green;
                }
            }
            else
            {
                color = Color.red;
            }

            Debug.DrawRay(startingPos, direction * distance, color);
            startingPos = new Vector2(startingPos.x, startingPos.y - offset);
        }

        return contactPoint;
    }
}
