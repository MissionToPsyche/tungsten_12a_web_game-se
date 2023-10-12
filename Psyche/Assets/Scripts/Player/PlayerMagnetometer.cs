/** 
Description: Tool to pull the player towards metal objects
Author: jmolive8
Version: 1
**/

using UnityEngine;
using Physics = RotaryHeart.Lib.PhysicsExtension.Physics2D;
using RotaryHeart.Lib.PhysicsExtension;

public class PlayerMagnetometer : MonoBehaviour {
    //public PlayerMovement p; //to access player RigidBody and isFacingRight

    Vector3 magDirection;

    public void MagnetometerCall(Rigidbody2D rb)
    {

    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Input.GetAxisRaw("Vertical") > 0) //if player is holding the up button
                magDirection = transform.up;
            //else if (p.isFacingRight)
            //    magDirection = transform.right;
            //else
            //    magDirection = transform.right * -1;

            /**
             * Raycasts 6 units away from the center of the player and only hits objects on the Metal layer
             */
            RaycastHit2D hit = Physics.Linecast(transform.position, transform.position + magDirection * 6, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);

            /**
             * Pulls the player towards the hit metal object
             */
            if (hit == true)
            {
                Vector2 pullDirection;
                if (Input.GetAxisRaw("Vertical") > 0)
                    pullDirection = new Vector2(transform.position.x, hit.point.y);
                else
                    pullDirection = new Vector2(hit.point.x, transform.position.y);

                //p.rb.MovePosition(Vector2.MoveTowards(transform.position, pullDirection, Time.deltaTime * 10));
            }
        }
    }
}


//**Experimental code**

//Physics.OverlapBox(transform.root.position + transform.up + transform.right * 3, new Vector2(6,1), 0, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);

//RaycastHit hit;
//if (Physics.Linecast(transform.root.position + transform.up, transform.root.position + transform.up + magDirection * 6, out hit, 1 << 3)) //transform.root.position + transform.up = about the center of the player