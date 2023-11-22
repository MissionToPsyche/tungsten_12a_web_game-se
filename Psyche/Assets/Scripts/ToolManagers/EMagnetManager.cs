using System.Collections;
using System.Collections.Generic;
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

    public void Initialize(PlayerController playerManagement)
    {
        //Base class varibles
        toolName = "Electromagnet";
        toolEnabled = false;
        _playerController = playerManagement;
        level = 0;
        levelRequirements = new Dictionary<int, Dictionary<string, int>>()
        {
            {  1, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 1 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  2, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 2 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  3, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 3 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  4, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 4 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  5, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 5 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
        };

        //Tool specific variables
        hitBoxRotator = eMagHitBox.transform.parent;
    }

    /// <summary>
    /// Activates the electromagnet
    /// </summary>
    public override void Activate()
    {
        StartCoroutine(handleEMagnet());
    }

    /// <summary>
    /// Activates EMagnet tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator handleEMagnet()
    {
        GameController.Instance.audioManager.toolEMagnet.Play();
        _playerController.eMagnetActive = true;
        hitBoxRotator.gameObject.SetActive(true);
        Collider2D hit, target = null;
        float curGrav = _playerController.playerCharacter.gravityScale;

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
                    if (!_playerController.playerCollider.IsTouching(hit))
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
                        _playerController.beingPulled = true;
                        _playerController.playerCharacter.gravityScale = 0;
                    }

                    _playerController.playerCharacter.velocity = Vector2.zero;
                    target = hit;
                }
            }

            /**
             * Pulls Player towards most recently hit Iron Vein
             */
            if (target != null)
                _playerController.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 40));

            yield return null;
        } while (Input.GetButton("Fire1"));

        GameController.Instance.audioManager.toolEMagnet.Stop();
        _playerController.playerCharacter.gravityScale = curGrav;
        hitBoxRotator.gameObject.SetActive(false);
        _playerController.eMagnetActive = false;
        _playerController.beingPulled = false;
    }

    /// <summary>
    /// Increases length of EMagnet Hit Box to increase hit range
    /// </summary>
    protected override void UpgradeTool()
    {
        eMagHitBox.transform.localScale += new Vector3(0.5f, 0, 0);
        eMagHitBox.transform.localPosition += new Vector3(0.25f, 0, 0);
    }
}