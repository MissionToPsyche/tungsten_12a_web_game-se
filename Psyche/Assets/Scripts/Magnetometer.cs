using System.Collections;
using UnityEngine;

/// <summary>
/// Tool to pull the player towards iron objects
/// </summary>
/// Author: jmolive8
public class Magnetometer : MonoBehaviour {
    public GameObject magHitBox;

    private Transform hitBoxRotator;

    BoxCollider2D test;

    private void Start()
    {
        hitBoxRotator = magHitBox.transform.parent;
        test = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Activates magnetometer tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator handleMagnet(AudioManager audioManager)
    {
        PlayerManagement.Instance.playerCharacter.velocity = Vector2.zero;
        PlayerManagement.Instance.magnetActive = true;
        hitBoxRotator.gameObject.SetActive(true);
        Collider2D hit, target = null;
        float curGrav = PlayerManagement.Instance.playerCharacter.gravityScale;

        do
        {
            /**
             * Finds angle between player center and mouse position
             */
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - hitBoxRotator.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBoxRotator.eulerAngles = new Vector3(0, 0, angle);

            hit = Physics2D.OverlapBox(magHitBox.transform.position, magHitBox.transform.lossyScale, angle, 1 << 14);
            if (hit != null)
            {
                if (hit.attachedRigidbody != null)
                {
                    hit.attachedRigidbody.velocity = Vector2.zero;
                    hit.attachedRigidbody.angularVelocity = 0;
                    hit.attachedRigidbody.MovePosition(Vector2.MoveTowards(hit.transform.position, transform.position, Time.deltaTime * 40)); //stop from pushing player: set mass back to 1 when done?
                }
                else if (hit != target)
                {
                    /**
                     * Ensures audio and gravity are only changed on the first iron object hit
                     */
                    if (target == null)
                    {
                        //audioManager.PlayToolMagnetometer(); // play tool sound
                        PlayerManagement.Instance.playerCharacter.gravityScale = 0;
                        //PlayerManagement.Instance.beingPulled = true;
                    }
                    target = hit;
                }
            }

            if (target != null && !test.IsTouching(target) /*Vector2.Distance(transform.position, target.transform.position) >= 1.5*/) //spazing, hit.IsTouching
                PlayerManagement.Instance.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 40));

            yield return null;
        } while (Input.GetButton("Fire1"));

        //audioManager.StopToolMagnetometer(); // stop tool sound
        PlayerManagement.Instance.playerCharacter.gravityScale = curGrav;
        hitBoxRotator.gameObject.SetActive(false);
        PlayerManagement.Instance.magnetActive = false;
    }
}