using System.Collections;
using UnityEngine;

/// <summary>
/// Logic for handling the Locked Door gameobject. Script is a component of the Pad child object of Locked Door.
/// </summary>
public class DoorUnlockPad : MonoBehaviour
{
    /// <summary>
    /// The specific Magnetized Square that is needed to open the door. It is a child object of the door.
    /// </summary>
    public Collider2D magKey;
    public SpriteRenderer padLight;
    public GameObject door;
    private Animator Animator;

    [Tooltip("Change this value if you change the height of the door.")]
    public float openDistance = 3f;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Animator.Play("red pad");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /**
         * When the magKey collides with the activation pad the pad turns green and opens the door.
         */
        if (collision == magKey)
        {
            //padLight.color = new Color(0, 1, 0, 0.3f);
            Animator.Play("green_pad");
            Destroy(collision.gameObject);
            StartCoroutine(MoveDoor());
        }
    }

    /// <summary>
    /// Code to slowly move the door upwards until it is fully open.
    /// </summary>
    private IEnumerator MoveDoor()
    {
        Vector2 dest = (Vector2)door.transform.position + new Vector2(0, openDistance);

        do
        {
            Vector2 moveVect = Vector2.MoveTowards(door.transform.position, dest, 2 * Time.deltaTime);
            door.transform.position = new Vector3(moveVect.x, moveVect.y, door.transform.position.z);
            yield return null;
        } while ((Vector2)door.transform.position != dest);
    }
}
