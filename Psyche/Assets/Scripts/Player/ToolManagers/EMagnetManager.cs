using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tool to pull the player towards magnetized objects
/// </summary>
/// Author: jmolive8
public class EMagnetManager : ToolManager {
    public GameObject EMagHitBox;

    /// <summary>
    /// Parent object of EMagnet Hit Box. Used for rotating the Hit Box around the player's center
    /// </summary>
    private Transform HitBoxRotator;
    private int PullSpeed = 20;

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
        HitBoxRotator = EMagHitBox.transform.parent;
    }

    /// <summary>
    /// Activates the electromagnet
    /// </summary>
    public override void Activate()
    {
        StartCoroutine(HandleEMagnet());
    }

    /// <summary>
    /// Activates EMagnet tool
    /// </summary>
    /// <returns></returns>
    public IEnumerator HandleEMagnet()
    {
        GameController.Instance.AudioManager.ToolEMagnet.Play();
        PlayerController.eMagnetActive = true;
        HitBoxRotator.gameObject.SetActive(true);
        Collider2D hit, targetDeposit = null, grabbedObject = null;
        float curGrav = PlayerController.playerCharacter.gravityScale;

        do
        {
            PlayerController.solarArrayManager.DrainBatt(1);

            /**
             * Finds angle between player center and mouse position
             */
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - HitBoxRotator.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            HitBoxRotator.eulerAngles = new Vector3(0, 0, angle);

            /**
             * Makes a box cast using the scale of the EMagnet Hit Box
             */
            hit = Physics2D.OverlapBox(EMagHitBox.transform.position, EMagHitBox.transform.lossyScale, angle, 1 << 7);
            if (hit != null && !hit.isTrigger)
            {
                //If movable Magnetized object hit
                if (hit.attachedRigidbody != null)
                {
                    if (hit != grabbedObject)
                    {
                        grabbedObject = hit;
                    }
                }
                //If new Magnetized Deposit hit
                else if (hit != targetDeposit)
                {
                    /**
                     * Disables gravity and player movement when being pulled towards an Magnetized Deposit
                     */
                    if (targetDeposit == null)
                    {
                        PlayerController.playerCharacter.transform.SetParent(null);
                        PlayerController.beingPulled = true;
                        PlayerController.playerCharacter.gravityScale = 0;
                    }

                    PlayerController.playerCharacter.velocity = Vector2.zero;
                    targetDeposit = hit;
                }
            }

            /**
             * Pulls Player towards most recently hit Magnetized Deposit
             */
            if (targetDeposit != null)
            {
                if (targetDeposit.gameObject.activeInHierarchy)
                {
                    PlayerController.playerCharacter.MovePosition(Vector2.MoveTowards(transform.position, targetDeposit.transform.position, Time.deltaTime * PullSpeed));
                }
                else
                {
                    /**
                     * Stops pulling the player if the most recently hit Magnetized Deposit has disappeared
                     */
                    PlayerController.beingPulled = false;
                    PlayerController.playerCharacter.gravityScale = curGrav;
                    targetDeposit = null;
                }
            }

            /**
             * Pulls the most recently hit Movable Magnetized Object towards the Player
             */
            if (grabbedObject != null)
            {
                grabbedObject.attachedRigidbody.velocity = Vector2.zero;
                grabbedObject.attachedRigidbody.angularVelocity = 0;
                if (!PlayerController.playerCollider.IsTouching(grabbedObject))
                {
                    grabbedObject.attachedRigidbody.MovePosition(Vector2.MoveTowards(grabbedObject.transform.position, transform.position, Time.deltaTime * PullSpeed));
                }
            }

            yield return null;
        } while (Input.GetButton("EMagnet") && !PlayerController.toolInterrupt && PlayerController.solarArrayManager.BatteryPercent != 0);

        GameController.Instance.AudioManager.ToolEMagnet.Stop();
        PlayerController.playerCharacter.gravityScale = curGrav;
        HitBoxRotator.gameObject.SetActive(false);
        PlayerController.eMagnetActive = false;
        PlayerController.beingPulled = false;
    }

    /// <summary>
    /// Increases speed at which the EMagnet pulls the player or objects
    /// </summary>
    protected override void UpgradeTool()
    {
        PullSpeed += 6;
    }
}