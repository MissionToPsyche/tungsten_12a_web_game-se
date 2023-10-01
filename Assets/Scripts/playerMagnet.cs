using UnityEngine;
using Physics = RotaryHeart.Lib.PhysicsExtension.Physics2D;
using RotaryHeart.Lib.PhysicsExtension;
using System.Net;

public class playerMagnet : MonoBehaviour {
    public SpriteRenderer magnetRange;
    public BoxCollider2D magnetColl;
    public Rigidbody2D player;

    void FixedUpdate()
    {
        if (Input.GetKey("f"))
        {
            //magnetRange.enabled = true;
            //magnetColl.enabled = true;

            //Collider2D[] hitColliders = 
            //Physics.OverlapBox(transform.root.position + transform.up + transform.right * 3, new Vector2(6,1), 0, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);
            Physics.Linecast(transform.root.position + transform.up, transform.root.position + transform.up + transform.right * 6, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);
        }

        //if (Input.GetKeyUp("f"))
        //{
        //    //magnetRange.enabled = false;
        //    //magnetColl.enabled = false;
        //}
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("HIT");
    //}
}
