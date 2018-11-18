using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
    // Config
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpSpeed = 5f;

    // State

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myPlayerColider;

	void Start () {

        myRigidBody = GetComponent <Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myPlayerColider = GetComponent<Collider2D>();
		
	}
	
	
	void Update () {
        Run();
        FlipSprite();
        Jump();
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
        if (CrossPlatformInputManager.GetButtonDown("Jump") & TouchingGround())
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite() { //reverse scaling of x-axis 
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a tiny floating point value
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private bool TouchingGround()
    {
        return myPlayerColider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
