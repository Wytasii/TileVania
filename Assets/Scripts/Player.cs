using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
    // Config
    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;

    // State

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myPlayerColider;
    float gravityScaleAtStart;

	void Start () {
        myRigidBody = GetComponent <Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myPlayerColider = GetComponent<Collider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
	}
	
	
	void Update () {
        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
	}

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow*speed, myRigidBody.velocity.y); //keep y set to the current velocity
        myRigidBody.velocity = playerVelocity; // update velocity with new x velocity
     
        myAnimator.SetBool("isRunning", Mathf.Abs(playerVelocity.x) > Mathf.Epsilon);

    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") )
        { if (TouchingGround() || TouchingLadder()) // trying to make jumping off ladder work
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
            }
        }
    }

    private void ClimbLadder()
    {
        if (!TouchingLadder())
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 to +1
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed); //keep x set to the current velocity
        myRigidBody.velocity = climbVelocity; // update velocity with new y velocity
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        if (TouchingLadder())
        {
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed || !TouchingGround());
        }
    }

    private void FlipSprite()
    { //reverse scaling of x-axis 
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a tiny floating point value
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private bool TouchingGround()
    {
        return myPlayerColider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private bool TouchingLadder()
    {
        return myPlayerColider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
}
