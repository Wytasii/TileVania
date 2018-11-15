using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    Rigidbody2D myRigidBody;
    [SerializeField] private float speed = 5f;

	// Use this for initialization
	void Start () {

        myRigidBody = GetComponent <Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
        Run();
        FlipSprite();
	}

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow*speed, myRigidBody.velocity.y); //keep y set to the current velocity
        myRigidBody.velocity = playerVelocity; // update velocity with new x velocity
    }

    private void FlipSprite() { //reverse scaling of x-axis 
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a tiny floating point value
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
