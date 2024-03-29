using System.Collections;
using UnityEngine;

public class DoorUnlockPad : MonoBehaviour
{
    public SpriteRenderer padLight;
    public Collider2D magKey;
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == magKey)
        {
            padLight.color = new Color(0, 1, 0, 0.3f);
            Destroy(collision.gameObject);
            StartCoroutine(MoveDoor());
        }
    }

    private IEnumerator MoveDoor()
    {
        Vector2 dest = (Vector2)door.transform.position + new Vector2(0, 3);

        do
        {
            Vector2 moveVect = Vector2.MoveTowards(door.transform.position, dest, 2 * Time.deltaTime);
            door.transform.position = new Vector3(moveVect.x, moveVect.y, door.transform.position.z);
            yield return null;
        } while ((Vector2)door.transform.position != dest);
    }
}
