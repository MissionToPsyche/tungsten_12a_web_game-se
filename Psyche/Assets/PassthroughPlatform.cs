/*
 * Description: Enables one way functionality on Enabled platforms
 * Authors: mcmyers4
 * Version: 20240227
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
///  Press down while standing on a platform to fall through
/// </summary>
public class PassthroughPlatform : MonoBehaviour
{
    private GameObject platform;
    [SerializeField] private BoxCollider2D playerCollider;

    // Update is called once per frame
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        if (Input.GetButton("Vertical") && verticalAxis < 0)
        {
            if (platform != null)
            {
                StartCoroutine(Passthrough());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform"))
        {
            platform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassthroughPlatform"))
        {
            platform = null;
        }
    }

    /// <summary>
    /// disables platform for specified time
    /// </summary>
    /// <returns></returns>
    private IEnumerator Passthrough()
    {
        BoxCollider2D platformCollider = platform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
