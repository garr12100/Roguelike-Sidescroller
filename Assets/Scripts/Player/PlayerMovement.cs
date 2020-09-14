using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speedWalk = 10f;
    public float speedRun = 15;
    public float climbSpeed = 10f;
    public float jumpHeight = 5f;
    public float groundRadius = .1f;
    public float wallSlideSpeed = 1f;
    public float wallJumpHorizontalSpeed;
    public float wallJumpPauseMoveTime;
    public float wallJumpHeight = 5f;
    public float wallGrabTimer = 0f;
    public float ladderJumpPauseClimbTime = .25f;
    public int numJumps;
    public int numJumpsFromWall = 1;
    public float xAccel = .25f;
    public GroundChecker groundChecker;
    public GroundChecker wallChecker;
    public bool canMoveX;
    public bool canClimbLadders;


    Rigidbody2D rbody;
    SpriteRenderer spriteRenderer;
    bool facingRight = true;
    public int numJumpsRemaining;
    bool wallSliding = false;
    bool touchingLadder;
    bool climbingLadder;
    GameObject ladder;

    float targetXVel;
    float gravityScale;

    private bool jumpThisFrame;

    // Use this for initialization
    void Start ()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScale = rbody.gravityScale;
        canClimbLadders = true;
        canMoveX = true;
        groundChecker.OnNotGrounded += OnNotGrounded;
        wallChecker.OnNotGrounded += OnNotWalled;
    }

    private void Update()
    {
        if (Input.GetButtonDown(UtilityStrings.Input.Jump))
        {
            jumpThisFrame = true;
        }
    }

    void FixedUpdate()
    {

        //Get movement and set target velocity
        float moveX = Input.GetAxisRaw(UtilityStrings.Input.Horizontal);
        float moveY = Input.GetAxisRaw(UtilityStrings.Input.Vertical);
        bool running = Input.GetButton(UtilityStrings.Input.Run);

        if (canMoveX)
        {
            targetXVel = moveX * (running ? speedRun : speedWalk);
            //Check for flipping
            if (moveX > 0 && !facingRight) Flip();
            else if (moveX < 0 && facingRight) Flip();
        }



        //Reset jumps if on ground
        if (groundChecker.grounded)
        {
            numJumpsRemaining = numJumps;
        }
        if (wallSliding && numJumpsRemaining == numJumpsFromWall)
        {
            numJumpsRemaining--;
        }
        wallSliding = false;
        float jumpH = jumpHeight;
        //Look for being against wall
        if (!groundChecker.grounded && wallChecker != null && wallChecker.grounded)
        {
            //Check if user trying to move against wall
            if ((facingRight && moveX > 0) || (!facingRight && moveX < 0))
            {
                if (rbody.velocity.y < 0)
                {
                    wallSliding = true;
                    rbody.velocity = new Vector2(rbody.velocity.x, -wallSlideSpeed);
                }
                int dir = facingRight ? -1 : 1;
                //numJumpsRemaining = numJumpsFromWall;
                if (jumpThisFrame)
                {
                    //Set values for wall jump
                    //jumpH = wallJumpHeight;
                    //targetXVel = dir * wallJumpHorizontalSpeed;
                    //StopCoroutine(StopMovementForTime(wallJumpPauseMoveTime));
                    //StartCoroutine(StopMovementForTime(wallJumpPauseMoveTime));
                    //Flip();
                    targetXVel = dir * wallJumpHorizontalSpeed;
                    WallJump();
                }
            }
        }

        //Execute jump
        if (jumpThisFrame)
        {
            Jump(jumpH);
        }
        jumpThisFrame = false;

        //Check if ladder:
        if (touchingLadder && (Mathf.Abs(moveY) > 0.1f || climbingLadder) && canClimbLadders)
        {
            if (!groundChecker.grounded || moveY > 0.1f)
            {
                climbingLadder = true;
                rbody.velocity = new Vector2(0, climbSpeed * moveY);
                rbody.gravityScale = 0;
                canMoveX = false;
                transform.position = new Vector3(ladder.transform.position.x + .5f, transform.position.y, transform.position.z);
                numJumpsRemaining = numJumps;
            }
            else
                HandleExitLadder();
        }
        else if (!canClimbLadders)
        {
            HandleExitLadder();
        }

        //Set speed to lerp to target
        rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, targetXVel, xAccel), rbody.velocity.y);

    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        if (facingRight)
            wallChecker.direction = GroundChecker.Direction.forward;
        else
            wallChecker.direction = GroundChecker.Direction.back;
    }

    void Jump(float height)
    {
        if (numJumpsRemaining > 0) // || (wallChecker != null && wallChecker.grounded))
        {
            if(!groundChecker.grounded && !wallChecker.grounded)
                numJumpsRemaining--;
            rbody.velocity = new Vector2(rbody.velocity.x, height);
            if(climbingLadder)
                StartCoroutine(StopLadderClimbingForTime(ladderJumpPauseClimbTime));
        }
    }

    void WallJump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, wallJumpHeight);
        StopCoroutine(StopMovementForTime(wallJumpPauseMoveTime));
        StartCoroutine(StopMovementForTime(wallJumpPauseMoveTime));
        Flip();
    }

    IEnumerator StopMovementForTime(float t)
    {
        canMoveX = false;
        yield return new WaitForSeconds(t);
        canMoveX = true;
    }

    IEnumerator StopLadderClimbingForTime(float t)
    {
        canClimbLadders = false;
        yield return new WaitForSeconds(t);
        canClimbLadders = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(UtilityStrings.Tags.Ladder))
        {
            ladder = col.gameObject;
            touchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(UtilityStrings.Tags.Ladder))
        {
            ladder = null;
            touchingLadder = false;
            if(climbingLadder)
                HandleExitLadder();
        }
    }

    void HandleExitLadder()
    {
        climbingLadder = false;
        rbody.gravityScale = gravityScale;
        canMoveX = true;
    }

    public void OnNotGrounded()
    {
        numJumpsRemaining--;
    }

    public void OnNotWalled()
    {
        if (!groundChecker.grounded && wallSliding)
        {
            //numJumpsRemaining--;
        }
    }


}
