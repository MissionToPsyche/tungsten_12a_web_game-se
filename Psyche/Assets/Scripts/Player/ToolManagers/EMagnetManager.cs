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
        ToolName = "Electromagnet";
        ToolEnabled = false;
        PlayerController = playerManagement;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 2 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 2 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 3 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 0 },
                }
            },
        };

        //Tool specific variables
        MaxLevel = LevelRequirements.Count + 1;
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
        PlayerController.eMagnetActive = true;
        hitBoxRotator.gameObject.SetActive(true);
        Collider2D hit, targetVein = null, grabbedObject = null;
        float curGrav = PlayerController.playerCharacter.gravityScale;

        do
        {
            PlayerController.solarArrayManager.DrainBatt(1);

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
                        PlayerController.beingPulled = true;
                        PlayerController.playerCharacter.gravityScale = 0;
                    }

                    PlayerController.playerCharacter.velocity = Vector2.zero;
                    targetVein = hit;
                }
            }

            /**
             * Pulls Player towards most recently hit Iron Vein
             */
            if (targetVein != null)
            {
                if (targetVein.gameObject.activeInHierarchy)
                    PlayerController.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, targetVein.transform.position, Time.deltaTime * pullSpeed));
                else
                {
                    /**
                     * Stops pulling the player if the most recently hit Iron Vein has disappeared
                     */
                    PlayerController.beingPulled = false;
                    PlayerController.playerCharacter.gravityScale = curGrav;
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
                if (!PlayerController.playerCollider.IsTouching(grabbedObject))
                    grabbedObject.attachedRigidbody.MovePosition(Vector2.MoveTowards(grabbedObject.transform.position, transform.position, Time.deltaTime * pullSpeed));
            }

            yield return null;
        } while (Input.GetButton("EMagnet") && !PlayerController.magnetInterrupt);

        GameController.Instance.audioManager.toolEMagnet.Stop();
        PlayerController.playerCharacter.gravityScale = curGrav;
        hitBoxRotator.gameObject.SetActive(false);
        PlayerController.eMagnetActive = false;
        PlayerController.beingPulled = false;
    }

    /// <summary>
    /// Increases speed at which the EMagnet pulls object
    /// </summary>
    protected override void UpgradeTool()
    {
        pullSpeed += 6;
    }
}