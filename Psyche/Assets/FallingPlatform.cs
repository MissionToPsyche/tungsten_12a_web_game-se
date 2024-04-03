/*
 * Description: Falling platform script
 * Authors: mcmyers4
 * Version: 20240402
 */
using System.Collections;
using UnityEngine;

/// <summary>
/// Platform begins to fall after player touches it.
/// </summary>
public class FallingPlatform : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    [SerializeField] private float OnDelay = 2.0f;
    [SerializeField] private float OffDelay = 2.0f;
    private Vector2 Spot;
    private bool Falling = false;

    // Public variables
    public Rigidbody2D RigidBody;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Spot = transform.position;
    }

    //=================================================== Collisions ==============================================

    /// <summary>
    /// Checks if the platform is already falling and if the object touching the platform is the player.
    /// </summary>
    /// <param name="collision"><see cref="Collision2D"/></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Falling) 
        { 
            return; 
        }
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fall());
        }
    }

    //============================================== Platform Actions ============================================

    /// <summary>
    /// Triggers the falling ability of the platform after the desired time,
    /// and brings the platform back to position after the desired time.
    /// </summary>
    /// <returns><see cref="IEnumerator"/></returns>
    private IEnumerator Fall()
    {
        Falling = true;
        yield return new WaitForSeconds(OnDelay);
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(OffDelay);
        Falling = false;
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        RigidBody.velocity = new Vector3(0, 0, 0);
        transform.position = Spot;
    }
}
