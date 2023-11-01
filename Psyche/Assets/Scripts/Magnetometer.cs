using System.Collections;
using UnityEngine;

/// <summary>
/// Tool to pull the player towards iron objects
/// </summary>
/// Author: jmolive8
public class Magnetometer : MonoBehaviour {
    public GameObject magHitBox;

    private Transform hitBoxRotator;

    private void Start()
    {
        hitBoxRotator = magHitBox.transform.parent;
    }

    /// <summary>
    /// Activates magnetometer tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator handleMagnet(AudioManager audioManager)
    {
        PlayerManagement.Instance.magnetActive = true;
        hitBoxRotator.gameObject.SetActive(true);
        Collider2D hit;

        do
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - hitBoxRotator.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBoxRotator.eulerAngles = new Vector3(0, 0, angle);
            hit = Physics2D.OverlapBox(magHitBox.transform.position, magHitBox.transform.lossyScale, angle, 1 << 7);

            yield return null;
        } while (Input.GetButton("Fire1") && hit == null);

        /**
         * Pulls the player towards the hit metal object and disables gravity
         */
        if (Input.GetButton("Fire1"))
        {
            //audioManager.PlayToolMagnetometer(); // play tool sound

            //if (hit.rigidbody != null)
            //{
            //    do
            //    {
            //        if (PlayerManagement.Instance.isGrounded)
            //            hit.rigidbody.MovePosition(Vector2.MoveTowards(hit.transform.position, transform.position, Time.deltaTime * 40)); //stop from pushing player: set mass back to 1 when done?, remove ground check?
            //        yield return null;
            //    } while (Input.GetButton("Fire1"));
            //}
            //else
            //{
            //    Vector2 pullDirection;
            //    if (Input.GetAxisRaw("Vertical") > 0)
            //        pullDirection = new Vector2(transform.position.x, hit.transform.position.y);
            //    else
            //        pullDirection = new Vector2(hit.transform.position.x, transform.position.y);

            //    PlayerManagement.Instance.playerCharacter.gravityScale = 0;

            //    do
            //    {
            //        PlayerManagement.Instance.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, pullDirection, Time.deltaTime * 40));
            //        yield return null;
            //    } while (Input.GetButton("Fire1"));

            //    PlayerManagement.Instance.playerCharacter.gravityScale = 1;
            //}
        }

        //audioManager.StopToolMagnetometer(); // stop tool sound
        hitBoxRotator.gameObject.SetActive(false);
        PlayerManagement.Instance.magnetActive = false;
    }
}