using UnityEngine;

public class playerMagnet : MonoBehaviour {
    public SpriteRenderer magnetRange;
    public BoxCollider2D magnetColl;
    public Rigidbody2D player;

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            magnetRange.enabled = true;
            magnetColl.enabled = true;
        }

        if (Input.GetKeyUp("f"))
        {
            magnetRange.enabled = false;
            magnetColl.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
    }
}
