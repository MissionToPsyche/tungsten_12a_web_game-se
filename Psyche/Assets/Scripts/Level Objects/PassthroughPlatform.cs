/*
 * Description: Enables one way functionality on enabled platforms
 * Authors: mcmyers4
 * Version: 20240402
 */
using System.Collections;
using UnityEngine;

/// <summary>
///  Press down while standing on a platform to fall through it
/// </summary>
public class PassthroughPlatform : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    [SerializeField] private BoxCollider2D PlayerCollider;
    private GameObject Platform;

    /// <summary>
    /// Update is called once per frame.
    /// Checks for the user pressing the down input.
    /// </summary>
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetButton("Vertical") && verticalAxis < 0)
        {
            if (Platform != null)
            {
                StartCoroutine(Passthrough());
            }
        }
    }

    //=================================================== Collisions ==============================================

    /// <summary>
    /// Checks if the object the player is touching is a passthrough enabled platform.
    /// </summary>
    /// <param name="collision"><see cref="Collision2D"/></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform"))
        {
            Platform = collision.gameObject;
        }
    }

    /// <summary>
    /// Releases the focused platform to allow the next platform to be stored.
    /// </summary>
    /// <param name="collision"><see cref="Collision2D"/></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform"))
        {
            Platform = null;
        }
    }

    /// <summary>
    /// Disables the current platform for the specified time.
    /// </summary>
    /// <returns><see cref="IEnumerator"/></returns>
    private IEnumerator Passthrough()
    {
        BoxCollider2D platformCollider = Platform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(PlayerCollider, platformCollider);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreCollision(PlayerCollider, platformCollider, false);
    }
}
