using System.Collections;
using UnityEngine;

/// <summary>
/// Tool to pull the player towards metal objects
/// </summary>
/// Author: jmolive8
public class Magnetometer : MonoBehaviour {
    public PlayerManagement pManage; //use instance referance
    public GameObject magEffect;

    private int magRange = 6;

    /// <summary>
    /// Activates magnetometer tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator handleMagnet(AudioManager audioManager)
    {
        pManage.magnetActive = true;
        magEffect.SetActive(true);
        RaycastHit2D hit = new RaycastHit2D();

        do
        {
            Vector2 endpoint;
            float shiftX = 0, shiftY = 0;

            /**
             * If player is holding the up button
             */
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                shiftX = .25f;
                endpoint = Vector2.up * magRange;
                magEffect.transform.localEulerAngles = new Vector3(0,0,90);
            }
            else
            {
                shiftY = .25f;
                if (Input.GetAxisRaw("Horizontal") < 0) //account for last direction faced
                {
                    endpoint = Vector2.right * -magRange;
                    magEffect.transform.localEulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    endpoint = Vector2.right * magRange;
                    magEffect.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }

            /**
             * Raycasts 6 units away slightly from the side of the player's center and only hits objects on the Metal layer
             */
            Vector2 origin1 = new Vector2(transform.position.x + shiftX, transform.position.y + shiftY);
            RaycastHit2D ray1 = Physics2D.Linecast(origin1, origin1 + endpoint, 1 << 7);
            if (ray1)
                hit = ray1;
            else
            {
                /**
                 * Second raycast to create box-like area for magnet hit box. Doesn't run if first ray hit something already. Will make proper box cast later
                 */
                Vector2 origin2 = new Vector2(transform.position.x - shiftX, transform.position.y - shiftY);
                RaycastHit2D ray2 = Physics2D.Linecast(origin2, origin2 + endpoint, 1 << 7);
                if (ray2)
                    hit = ray2;
            }

            yield return null;
        } while (Input.GetButton("Fire1") && !hit);

        /**
         * Pulls the player towards the hit metal object and disables gravity
         */
        if (Input.GetButton("Fire1"))
        {
            audioManager.PlayToolMagnetometer(); // play tool sound

            if (hit.rigidbody != null)
            {
                do
                {
                    if (pManage.isGrounded)
                        hit.rigidbody.MovePosition(Vector2.MoveTowards(hit.transform.position, transform.position, Time.deltaTime * 40)); //stop from pushing player: set mass back to 1 when done?, remove ground check?
                    yield return null;
                } while (Input.GetButton("Fire1"));
            }
            else
            {
                Vector2 pullDirection;
                if (Input.GetAxisRaw("Vertical") > 0)
                    pullDirection = new Vector2(transform.position.x, hit.transform.position.y);
                else
                    pullDirection = new Vector2(hit.transform.position.x, transform.position.y);

                pManage.playerCharacter.gravityScale = 0;

                do
                {
                    pManage.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, pullDirection, Time.deltaTime * 40));
                    yield return null;
                } while (Input.GetButton("Fire1"));

                pManage.playerCharacter.gravityScale = 1;
            }
        }

        audioManager.StopToolMagnetometer(); // stop tool sound
        magEffect.SetActive(false);
        pManage.magnetActive = false;
    }
}