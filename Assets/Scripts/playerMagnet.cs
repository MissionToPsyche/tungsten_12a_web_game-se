using UnityEngine;
using Physics = RotaryHeart.Lib.PhysicsExtension.Physics2D;
using RotaryHeart.Lib.PhysicsExtension;

public class playerMagnet : MonoBehaviour {
    public PlayerMovement p;

    Vector3 magDirection;

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Input.GetAxisRaw("Vertical") > 0)
                magDirection = transform.up;
            else if (p.isFacingRight)
                magDirection = transform.right;
            else
                magDirection = transform.right * -1;

            RaycastHit2D hit = Physics.Linecast(transform.root.position + transform.up, transform.root.position + transform.up + magDirection * 6, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);

            if (hit == true) //start coroutine?
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                    p.rb.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, hit.point.y), Time.deltaTime * 5));
                else
                    p.rb.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(hit.point.x, transform.position.y), Time.deltaTime * 5));
            }
        }
    }
}

//Physics.OverlapBox(transform.root.position + transform.up + transform.right * 3, new Vector2(6,1), 0, 1 << 3, PreviewCondition.Both, 1, Color.green, Color.red);