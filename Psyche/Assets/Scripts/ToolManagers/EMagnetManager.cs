using System.Collections;
using UnityEngine;

/// <summary>
/// Tool to pull the player towards iron objects
/// </summary>
/// Author: jmolive8
public class EMagnetManager : ToolManager {
    public GameObject eMagHitBox;

    /// <summary>
    /// Parent object of EMagnet Hit Box. Used for rotating the Hit Box around the player's center
    /// </summary>
    private Transform hitBoxRotator;

    public void Initialize(PlayerManagement playerManagement)
    {
        toolName = "Electromagnet";
        _playerManagement = playerManagement;
        hitBoxRotator = eMagHitBox.transform.parent;
    }

    /// <summary>
    /// Activates EMagnet tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator handleEMagnet()
    {
        _playerManagement.audioManager.PlayAudio(_playerManagement.audioManager.toolMagnetometer);
        _playerManagement.eMagnetActive = true;
        hitBoxRotator.gameObject.SetActive(true);
        Collider2D hit, target = null;
        float curGrav = _playerManagement.playerCharacter.gravityScale;

        do
        {
            /**
             * Finds angle between player center and mouse position
             */
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - hitBoxRotator.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBoxRotator.eulerAngles = new Vector3(0, 0, angle);

            /**
             * Makes a box cast using the scale of the EMagnet Hit Box
             */
            hit = Physics2D.OverlapBox(eMagHitBox.transform.position, eMagHitBox.transform.lossyScale, angle, 1 << 14);
            if (hit != null)
            {
                ///If movable Iron object hit
                if (hit.attachedRigidbody != null)
                {
                    hit.attachedRigidbody.velocity = Vector2.zero;
                    hit.attachedRigidbody.angularVelocity = 0;
                    if (!_playerManagement.playerCollider.IsTouching(hit))
                        hit.attachedRigidbody.MovePosition(Vector2.MoveTowards(hit.transform.position, transform.position, Time.deltaTime * 40));
                }
                ///If new Iron Vein hit
                else if (hit != target)
                {
                    /**
                     * Disables gravity and player movement when being pulled towards an Iron Vein
                     */
                    if (target == null)
                    {
                        _playerManagement.beingPulled = true;
                        _playerManagement.playerCharacter.gravityScale = 0;
                    }

                    _playerManagement.playerCharacter.velocity = Vector2.zero;
                    target = hit;
                }
            }

            /**
             * Pulls Player towards most recently hit Iron Vein
             */
            if (target != null)
                _playerManagement.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 40));

            yield return null;
        } while (Input.GetButton("Fire1"));

        _playerManagement.audioManager.StopAudio(_playerManagement.audioManager.toolMagnetometer);
        _playerManagement.playerCharacter.gravityScale = curGrav;
        hitBoxRotator.gameObject.SetActive(false);
        _playerManagement.eMagnetActive = false;
        _playerManagement.beingPulled = false;
    }

    /// <summary>
    /// Increases length of EMagnet Hit Box to increase hit range
    /// </summary>
    public override void Modify()
    {
        eMagHitBox.transform.localScale += new Vector3(0.5f, 0, 0);
        eMagHitBox.transform.localPosition += new Vector3(0.25f, 0, 0);
    }
}