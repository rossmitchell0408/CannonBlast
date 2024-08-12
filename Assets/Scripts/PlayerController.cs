using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rBod;
    public Transform groundPoint;
    public LayerMask groundLayer;

    Vector2 lookDirection;
    float moveSpeed = 8f;
    bool isFacingRight = true;
    
    float jumpPower = 10f;
    bool canDoubleJump = true;

    bool canDash = true;
    bool isDashing = false;
    float dashPower = 20f;
    float dashTime = 0.3f;
    float dashCoolDown = 2f;

    bool canGrapple = true;
    bool isGrappling = false;
    float grappleSpeed = 20f;
    float grappleTime = 2f;
    [SerializeField]
    float grappleClimbSpeed = 10f;

    [SerializeField]
    GrappleBehaviour grapplePrefab;
    [SerializeField]
    GrappleBehaviour grapple;

    bool isStunned = false;
    [SerializeField]
    float stunTimer = 1f;

    [SerializeField]
    List<AttackBehaviour> attacks;
    int attackIndex = 0;
    bool canAttack = true;
    bool isAttacking = false;
    bool attackLunging = false;
    float attackCooldown = 0.5f;
    float attackInputDelay = 0.3f;
    float attackInputCounter = 0f;
    float comboDelay = 1f;
    float comboCounter = 0f;
    //float comboTimer = 1.5f;

    List<FlagBehaviour> flags = new List<FlagBehaviour>();
    [SerializeField]
    GameObject flagSymbol;

    Animator animator;

    float coyoteTime = 0.3f;
    float coyoteTimeCounter = 0f;
    float landingBuffer = 0.3f;
    float landingBufferCounter = 0f;

    bool grabbingLedge = false;

    [SerializeField]
    ItemType item = ItemType.NONE;

    public float damage;
    public int score;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        rBod = GetComponent<Rigidbody2D>();
        if(grapple == null)
        {
            grapple = Instantiate(grapplePrefab);
            grapple.SetPlayer(this);
            grapple.gameObject.SetActive(false);
        }

        foreach (AttackBehaviour attack in attacks)
        {
            attack.SetPlayer(this);
        }

        animator = GetComponent<Animator>();
        item = ItemType.NONE;
        //transform.localScale = new Vector3(-1f, 1f, 1f);

        PlayerUIManager puim = FindObjectOfType<PlayerUIManager>();
        puim.SetupUI(this);

        GameManager.Instance.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbingLedge)
        {
            rBod.gravityScale = 0f;
        }
        else
        {
            rBod.gravityScale = 1f;
        }

        if (!isDashing && !isStunned && !attackLunging)
        {
            CalculateMoveVelocity();
        }
        if(!isFacingRight && lookDirection.x > 0f)
        {
            TurnAround();
            //Debug.Log("Test1");
        }
        else if(isFacingRight && lookDirection.x < 0f)
        {
            TurnAround();
            //Debug.Log("Test2");
        }
        //Test();
        //Debug.Log(transform.localScale.x);

        if (IsGrounded() || grabbingLedge)
        {
            canDoubleJump = true;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (comboCounter > 0f)
        {
            comboCounter -= Time.deltaTime;
        }

        if (attackInputCounter > 0f)
        {
            if (canAttack)
            {
                attackInputCounter = 0f;
                StartCoroutine(Punching());
            }
            else
            {
                attackInputCounter -= Time.deltaTime;
            }
        }

        Animate();
    }


    void Animate()
    {
        if (isStunned)
        {
            animator.SetInteger("State", (int)AnimationState.HURT);
        }
        else if (isAttacking)
        {
            animator.SetInteger("State", (int)AnimationState.PUNCH);
        }
        else if (IsGrounded())
        {
            if (rBod.velocity.x <= 0.1f && rBod.velocity.x >= -0.1f)
            {
                animator.SetInteger("State", (int)AnimationState.IDLE);
            }
            else
            {
                animator.SetInteger("State", (int)AnimationState.RUN);
            }
        }
        else if (IsJumping())
        {
            animator.SetInteger("State", (int)AnimationState.JUMP);
        }
        else if (IsFalling())
        {
            animator.SetInteger("State", (int)AnimationState.FALL);
        }
        //Test();
    }

    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }

    bool IsGrounded()
    {
        return Physics2D.CircleCast(groundPoint.position, .1f, Vector2.down, .1f, groundLayer);
    }

    bool IsJumping()
    {
        if (!IsGrounded() && rBod.velocity.y > 0)
        {
            return true;
        }
        return false;
    }

    bool IsFalling()
    {
        if (!IsGrounded() && rBod.velocity.y < 0)
        {
            return true;
        }
        return false;
    }

    void TurnAround()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        //GetComponent<SpriteRenderer>().flipX = !isFacingRight;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isStunned || attackLunging)
        {
            return;
        }

        lookDirection = context.ReadValue<Vector2>();
    }

    void CalculateMoveVelocity()
    {
        float yVelocity = rBod.velocity.y;
        float xVelocity = lookDirection.x * moveSpeed;
        LedgeDetection ledgeDetection = GetComponentInChildren<LedgeDetection>();
        Vector2 ledgePoint = ledgeDetection.CheckForLedge();

        // Set player to appropreate ledge grabbing position if ledge detected
        if (ledgeDetection.ledgeDetected)
        {
            if (!grabbingLedge)
            {
                transform.position = new Vector2(ledgePoint.x - ((GetComponent<Collider2D>().bounds.size.x / 2) * lookDirection.x), ledgePoint.y - GetComponent<Collider2D>().bounds.size.y / 2);
                xVelocity = 0f;
                yVelocity = 0f;

                grabbingLedge = true;
            }
        }
        else
        {
            grabbingLedge = false;
        }

        // Set y velocity to move up to top of step if the step is low enough
        if (ledgeDetection.stepDetected && lookDirection.x != 0f)
        {
            yVelocity = (ledgePoint.y - (transform.position.y - GetComponent<Collider2D>().bounds.size.y / 2)) * moveSpeed;
        }

        rBod.velocity = new Vector2(xVelocity, yVelocity);
    }

    public void Jump(InputAction.CallbackContext context) 
    { 
        if (isStunned)
        {
            return;
        }

        if (context.performed && coyoteTimeCounter > 0f/*IsGrounded()*/)
        {
            rBod.velocity = new Vector2(rBod.velocity.x, jumpPower);
            //animator.SetInteger("State", (int)AnimationState.JUMP);
        }
        else if (context.performed && coyoteTimeCounter <= 0f/*!IsGrounded()*/ && canDoubleJump)
        {
            canDoubleJump = false;
            rBod.velocity = new Vector2(rBod.velocity.x, jumpPower * 0.75f);
            //animator.SetInteger("State", (int)AnimationState.JUMP);
        }

        if (context.canceled && rBod.velocity.y > 0f)
        {
            rBod.velocity = new Vector2(rBod.velocity.x, rBod.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canDash)
            {
                StartCoroutine(Dashing());
            }
        }
    }

    private IEnumerator Dashing()
    {
        canDash = false;
        isDashing = true;
        float grav = rBod.gravityScale;
        rBod.gravityScale = 0f;
        rBod.velocity = new Vector2(transform.localScale.x * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);
        rBod.gravityScale = grav;
        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    public void Grapple(InputAction.CallbackContext context)
    {
        if (context.performed && !isStunned)
        {
            if (grapple.GrappleReady())
            {
                grapple.LaunchGrapple(lookDirection);
            }
        }

        if(context.canceled)
        {
            grapple.RetractGrapple();
            rBod.gravityScale = 1f;
        }
    }

    public void FollowGrapple()
    {
        if (!grapple.PullReady())
        {
            return;
        }

        rBod.gravityScale = 0f;
        Vector3 direction = grapple.transform.position - transform.position;
        rBod.velocity = direction.normalized * grappleClimbSpeed;
        //transform.position += direction.normalized * grappleClimbSpeed * Time.deltaTime;
    }

    public void Punch(InputAction.CallbackContext context)
    {
        if (context.performed && !isStunned)
        {
            attackInputCounter = attackInputDelay;
        }
    }

    IEnumerator Punching()
    {
        canAttack = false;
        isAttacking = true;

        if (comboCounter > 0f)
        {
            comboCounter = 0f;
            attackIndex++;
        }
        else
        {
            attackIndex = 0;
        }

        if (attackIndex >= attacks.Count)
        {
            attackIndex = 0;
        }

        animator.SetInteger("AttackIndex", attackIndex);
        attacks[attackIndex].Attack();

        StartCoroutine(Lunge());

        yield return new WaitForSeconds(attacks[attackIndex].GetAttackTime());

        comboCounter = comboDelay;
        isAttacking = false;
        canAttack = true;
    }

    IEnumerator Lunge()
    {
        attackLunging = true;
        rBod.AddForce(new Vector2(transform.localScale.x * 3f, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        attackLunging = false;
    }

    public void GetHit(Vector3 contactPoint, float knockbackPower, float damage)
    {
        this.damage += damage;
        Vector2 knockbackDirection = (transform.position - contactPoint).normalized;
        //Debug.Log(knockbackDirection.y);
        if (knockbackDirection.y <= 0.01f)
        {
            knockbackDirection.y = 0.5f;
        }

        Stun(CalculateStunTime(knockbackPower));
        rBod.AddForce(knockbackDirection * CalculateKnockback(knockbackPower), ForceMode2D.Impulse);
        DropAllFlags();
    }

    float CalculateKnockback(float knockbackPower)
    {
        return (damage / 100) * knockbackPower;
    }

    float CalculateStunTime(float power)
    {
        
        if (power < 10f)
        {
            CameraController.Instance.Shake();
            return 0.5f;
        }
        else if (power < 15f)
        {
            HitStop.Instance.Stop();
            CameraController.Instance.Shake(0.1f, 0.3f);
            return 1f;
        }
        else if (power < 20f)
        {
            HitStop.Instance.Stop(0.3f);
            CameraController.Instance.Shake(0.2f, 0.4f);
            return 1.5f;
        }
        else
        {
            HitStop.Instance.Stop(0.5f);
            CameraController.Instance.Shake(0.3f, 0.5f);
            return 2f;
        }
    }

    public void Stun(float time = 1f)
    {
        StartCoroutine(StunTimer(time));
    }

    IEnumerator StunTimer(float time)
    {
        isStunned = true;
        if (time == 0f)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time);

        
        isStunned = false;
    }

    public void PickupFlag(FlagBehaviour flag)
    {
        flags.Add(flag);
        //flag.gameObject.SetActive(false);
        flagSymbol.SetActive(true);
    }

    public void DropAllFlags()
    {
        if (flags == null || flags.Count == 0)
        {
            return;
        }

        foreach(FlagBehaviour flag in flags)
        {
            flag.transform.position = transform.position;
            StartCoroutine(flag.DropFlag());
        }

        flags.Clear();
        flagSymbol.SetActive(false);
    }

    public void ScoreFlags()
    {
        if (flags == null || flags.Count == 0)
        {
            return;
        }

        score += flags.Count;

        foreach (FlagBehaviour flag in flags)
        {
            //Debug.Log("Goal");
            FlagManager.Instance.RemoveFlag(flag);
            Destroy(flag.gameObject);
        }

        flags.Clear();
        flagSymbol.SetActive(false);
    }

    public void GetNewItem(ItemType item)
    {
        this.item = item;
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        switch (item)
        {
            case ItemType.NONE:
                Debug.Log("No item to use");
                break;
            case ItemType.MISSILE:
                Debug.Log("Using Missile");
                MissileController missile = ItemManager.Instance.GetMissile();
                missile.Launch(this, lookDirection);
                break;
            default:
                Debug.Log("Item out of range");
                break;
        }

        //item = ItemType.MISSILE;
        item = ItemType.NONE;
    }

    public void Respawn()
    {
        damage = 0;

        foreach (FlagBehaviour flag in flags)
        {
            FlagManager.Instance.RemoveFlag(flag);
            Destroy(flag);
        }
        flags.Clear();
        flagSymbol.SetActive(false);
    }
}
