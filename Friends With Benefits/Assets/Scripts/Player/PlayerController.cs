using System.Collections;
using UnityEngine;
using Rewired;

public class PlayerController : WrappableObject
{
    private Player playerInput;
    private PlayerManager playerManager;
    private int playerID;

    private Rigidbody2D rb;
    private Vector3 velocity;

    [Header("GRAVITY")]
    public float gravity;
    public float terminalVelocity;

    [Header("MOVEMENT")]
    public float acceleration;
    public float inAirAcceleration;
    public float maxSpeed;
    public float drag;
    public float inAirDrag;
    private int facingDirection = 1;

    [Header("JUMP")]
    public float jumpForce;
    private bool canDoubleJump = true;

    [Header("DASH")]
    public float dashTime;
    public float dashSpeed;
    public AnimationCurve dashCurve;
    public float dashCooldown;
    private bool canDash = true;
    private bool isDashing = false;
    private Coroutine dashCoroutine;

    [Header("TRAIL")]
    public GameObject trailSlot;
    public GameObject trailPrefab;
    private GameObject currentTrail;

    [Header("COLLISION ATTRIBUTES")]
    public Transform groundCheckSlot1;
    public Transform groundCheckSlot2;
    public Transform wallCheckSlot1;
    public Transform wallCheckSlot2;
    public Transform wallCheckSlot3;
    public Transform ceilingCheckSlot1;
    public Transform ceilingCheckSlot2;
    public float verticalCheckDistance;
    public float horizontalCheckDistance;
    public LayerMask obstacleLayerMask;
    public LayerMask enemyLayerMask;

    public void InitializePlayerController(PlayerManager newPlayerManager, int newPlayerID)
    {
        playerManager = newPlayerManager;
        playerID = newPlayerID;

        playerInput = ReInput.players.GetPlayer(playerID);

        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;

        currentTrail = Instantiate(trailPrefab, trailSlot.transform.position, Quaternion.identity, trailSlot.transform);
        currentTrail.GetComponent<PlayerTrail>().AttachToPlayer(this);

        LayerMask layer0 = 1 << LayerMask.NameToLayer("Player0");
        LayerMask layer1 = 1 << LayerMask.NameToLayer("Player1");
        LayerMask layer2 = 1 << LayerMask.NameToLayer("Player2");
        LayerMask layer3 = 1 << LayerMask.NameToLayer("Player3");

        //set layer
        switch (playerID)
        {
            case 0:
                gameObject.layer = LayerMask.NameToLayer("Player0");
                enemyLayerMask = layer1 | layer2 | layer3;
                break;
            case 1:
                gameObject.layer = LayerMask.NameToLayer("Player1");
                enemyLayerMask = layer0 | layer2 | layer3;
                break;
            case 2:
                gameObject.layer = LayerMask.NameToLayer("Player2");
                enemyLayerMask = layer0 | layer1 | layer3;
                break;
            case 3:
                gameObject.layer = LayerMask.NameToLayer("Player3");
                enemyLayerMask = layer0 | layer1 | layer2;
                break;
            default:
                break;
        }
    }

    private new void Update()
    {
       
        base.Update();

        if (!isDashing)
        {
            Move();
            Jump();
            ApplyGravity();
            if (canDash && playerInput.GetButtonDown("Dash"))
            {
                dashCoroutine = StartCoroutine(Dash());
            }
        }
        else
        {
            if(velocity == Vector3.zero)
            {
                CancelDash();
            }
        }

        //update rigidbody velocity
        rb.velocity = velocity;
    }

    private void ApplyGravity()
    {
        if (!GroundCheck() && velocity.y > terminalVelocity)
        {
            velocity.y += gravity;
        }
    }

    private void Jump()
    {
        if (GroundCheck() && playerInput.GetButtonDown("Jump")) // jump
        {
            velocity.y = jumpForce;
        }
        else if(!GroundCheck() && canDoubleJump && playerInput.GetButtonDown("Jump")) //double jump
        {
            velocity.y = jumpForce;
            canDoubleJump = false;
        }
        else if(!GroundCheck() && playerInput.GetButtonUp("Jump") && velocity.y > 0f) // variable jump
        {
            velocity.y /= 2.5f;
        }
    }

    private void Move()
    {
        float direction = playerInput.GetAxis("Horizontal");

        // clamping the drag (enabling hard turns)
        if (direction > 0)
        {
            direction = 1.0f;
            if (velocity.x < 0)
                velocity.x = 0;
        }
        else if (direction < 0)
        {
            direction = -1.0f;
            if (velocity.x > 0)
                velocity.x = 0;
        }
//        Debug.Log("Direction: " + direction);

        if (direction != 0f)
        {
            facingDirection = (int)Mathf.Sign(direction);
        }



        if (Mathf.Abs(direction) > 0.25f)
        {
            if (!GroundCheck() && velocity.y < 0f)
            {
                velocity.x += inAirAcceleration * direction;
            }
            else
            {
                velocity.x += acceleration * direction;
            }
            
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
        else
        {
            if (Mathf.Abs(velocity.x) > 0f)
            {
                if (GroundCheck())
                {
                    velocity.x = Mathf.Lerp(velocity.x, 0f, Time.deltaTime * drag);
                }
                else
                {
                    velocity.x = Mathf.Lerp(velocity.x, 0f, Time.deltaTime * inAirDrag);
                }
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector3 direction = new Vector3(Mathf.Round(playerInput.GetAxis("Horizontal")), Mathf.Round(playerInput.GetAxis("Vertical")), 0f).normalized;
        if(direction == Vector3.zero)
        {
            direction = Vector3.right * facingDirection;
        }
        velocity = direction * dashSpeed;

        float timer = 0;
        while(timer < dashTime)
        {
            timer += Time.deltaTime;

            float normalizedTime = timer / dashTime;
            velocity = direction * (dashSpeed * dashCurve.Evaluate(normalizedTime));

            yield return null;
        }

        CancelDash();
    }

    private void CancelDash()
    {
        StopCoroutine(dashCoroutine);
        isDashing = false;
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GroundCheck() && velocity.y != 0f)
        {
            velocity.y = 0f;
        }
        WallCheck();
        CeilingCheck();

        //destroy player on head boop
        if (HeadBoopCheck())
        {
            collision.gameObject.GetComponent<PlayerController>().TriggerDeath();
            TriggerKill();
            velocity.y = jumpForce / 2;
        }

        if (isDashing && JavelinBoopCheck())
        {
            collision.gameObject.GetComponent<PlayerController>().TriggerDeath();
            TriggerKill();
        }
    }


    private bool GroundCheck()
    {
        bool check1 = Physics2D.Raycast(groundCheckSlot1.position, Vector2.down, verticalCheckDistance, obstacleLayerMask);
        bool check2 = Physics2D.Raycast(groundCheckSlot2.position, Vector2.down, verticalCheckDistance, obstacleLayerMask);

        if (check1 || check2)
        {
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CeilingCheck()
    {
        if (velocity.y > 0f)
        {
            bool check1 = Physics2D.Raycast(ceilingCheckSlot1.position, Vector2.up, verticalCheckDistance, obstacleLayerMask);
            bool check2 = Physics2D.Raycast(ceilingCheckSlot2.position, Vector2.up, verticalCheckDistance, obstacleLayerMask);

            if (check1 || check2)
            {
                velocity.y = 0f;
            }
        }
    }

    private void WallCheck()
    {
        if (Mathf.Abs(velocity.x) > 0f)
        {
            bool check1 = Physics2D.Raycast(wallCheckSlot1.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, obstacleLayerMask);
            bool check2 = Physics2D.Raycast(wallCheckSlot2.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, obstacleLayerMask);
            bool check3 = Physics2D.Raycast(wallCheckSlot3.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, obstacleLayerMask);

            if (check1 || check2 || check3)
            {
                velocity.x = 0f;
            }
        }
    }

    private bool HeadBoopCheck()
    {
        bool check1 = Physics2D.Raycast(groundCheckSlot1.position, Vector2.down, verticalCheckDistance, enemyLayerMask);
        bool check2 = Physics2D.Raycast(groundCheckSlot2.position, Vector2.down, verticalCheckDistance, enemyLayerMask);

        if (check1 || check2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool JavelinBoopCheck()
    {
        bool check1 = Physics2D.Raycast(wallCheckSlot1.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, enemyLayerMask);
        bool check2 = Physics2D.Raycast(wallCheckSlot2.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, enemyLayerMask);
        bool check3 = Physics2D.Raycast(wallCheckSlot3.position, Vector2.right * Mathf.Sign(velocity.x), horizontalCheckDistance, enemyLayerMask);

        if (check1 || check2 || check3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void TriggerWrap()
    {
        currentTrail.transform.parent = null;
        var main = currentTrail.GetComponent<ParticleSystem>().main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        currentTrail = Instantiate(trailPrefab, trailSlot.transform.position, Quaternion.identity, trailSlot.transform);
        currentTrail.GetComponent<PlayerTrail>().AttachToPlayer(this);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public int GetID()
    {
        return playerID;
    }

    public void TriggerDeath()
    {
        playerManager.PlayerDeath();
    }

    public void TriggerKill()
    {
        playerManager.PlayerKill();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        //draw ground rays
        Gizmos.DrawLine(groundCheckSlot1.position, groundCheckSlot1.position + Vector3.down * verticalCheckDistance);
        Gizmos.DrawLine(groundCheckSlot2.position, groundCheckSlot2.position + Vector3.down * verticalCheckDistance);

        //draw ceiling rays
        if (velocity.y > 0f)
        {
            Gizmos.DrawLine(ceilingCheckSlot1.position, ceilingCheckSlot1.position + Vector3.up * verticalCheckDistance);
            Gizmos.DrawLine(ceilingCheckSlot2.position, ceilingCheckSlot2.position + Vector3.up * verticalCheckDistance);
        }

        //draw wall rays
        if (Mathf.Abs(velocity.x) > 0f)
        {
            Gizmos.DrawLine(wallCheckSlot1.position, wallCheckSlot1.position + (Vector3.right * Mathf.Sign(velocity.x)) * horizontalCheckDistance);
            Gizmos.DrawLine(wallCheckSlot2.position, wallCheckSlot2.position + (Vector3.right * Mathf.Sign(velocity.x)) * horizontalCheckDistance);
        }
    }

   
}
