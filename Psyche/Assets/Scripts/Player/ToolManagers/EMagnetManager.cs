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
    private int pullSpeed = 20;

    public void Initialize(PlayerController playerManagement)
    {
        //Base class varibles
        toolName = "Electromagnet";
        toolEnabled = false;
        _playerController = playerManagement;
        level = 1;
        levelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 2 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 2 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 3 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
        };

        //Tool specific variables
        maxLevel = levelRequirements.Count + 1;
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
        Collider2D hit, targetVein = null, grabbedObject = null;
        float curGrav = _playerController.playerCharacter.gravityScale;

        do
        {
            _playerController.solarArrayManager.DrainBatt(1);

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
            hit = Physics2D.OverlapBox(eMagHitBox.transform.position, eMagHitBox.transform.lossyScale, angle, 1 << 7);
            if (hit != null && !hit.isTrigger)
            {
                //If movable Iron object hit
                if (hit.attachedRigidbody != null)
                {
                    if (hit != grabbedObject)
                        grabbedObject = hit;
                }
                //If new Iron Vein hit
                else if (hit != targetVein)
                {
                    /**
                     * Disables gravity and player movement when being pulled towards an Iron Vein
                     */
                    if (targetVein == null)
                    {
                        _playerController.beingPulled = true;
                        _playerController.playerCharacter.gravityScale = 0;
                    }

                    _playerController.playerCharacter.velocity = Vector2.zero;
                    targetVein = hit;
                }
            }

            /**
             * Pulls Player towards most recently hit Iron Vein
             */
            if (targetVein != null)
            {
                if (targetVein.gameObject.activeInHierarchy)
                    _playerController.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, targetVein.transform.position, Time.deltaTime * pullSpeed));
                else
                {
                    /**
                     * Stops pulling the player if the most recently hit Iron Vein has disappeared
                     */
                    _playerController.beingPulled = false;
                    _playerController.playerCharacter.gravityScale = curGrav;
                    targetVein = null;
                }
            }

            /**
             * Pulls the most recently hit Movable Iron Object towards the Player
             */
            if (grabbedObject != null)
            {
                grabbedObject.attachedRigidbody.velocity = Vector2.zero;
                grabbedObject.attachedRigidbody.angularVelocity = 0;
                if (!_playerController.playerCollider.IsTouching(grabbedObject))
                    grabbedObject.attachedRigidbody.MovePosition(Vector2.MoveTowards(grabbedObject.transform.position, transform.position, Time.deltaTime * pullSpeed));
            }

            yield return null;
        } while (Input.GetButton("EMagnet") && !_playerController.magnetInterrupt);

        GameController.Instance.audioManager.toolEMagnet.Stop();
        _playerController.playerCharacter.gravityScale = curGrav;
        hitBoxRotator.gameObject.SetActive(false);
        _playerController.eMagnetActive = false;
        _playerController.beingPulled = false;
    }

    /// <summary>
    /// Increases speed at which the EMagnet pulls object
    /// </summary>
    protected override void UpgradeTool()
    {
        pullSpeed += 6;
    }
}