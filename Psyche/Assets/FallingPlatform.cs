using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float onDelay = 2.0f;
    [SerializeField] private float offDelay = 2.0f;
    public Rigidbody2D rb;
    private Vector2 spot;
    private bool falling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spot = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling) { return; }
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fall());
        }
    }

/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boundary")
        {
            falling = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector3(0, 0, 0);
            transform.position = spot;
        }
    }*/

    private IEnumerator Fall()
    {
        falling = true;
        yield return new WaitForSeconds(onDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(offDelay);
        falling = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = spot;
    }
}
