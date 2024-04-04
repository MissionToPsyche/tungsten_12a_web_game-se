/*
 * Description: Moving platform script
 * Authors: mcmyers4
 * Version: 20240402
 */
using UnityEngine;

/// <summary>
/// Basic moving platform that moves between a list of points
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    private Vector2[] Points;
    private int PointsIndex = 1;

    // Public variables
    public Transform PointsObject;
    public float MoveSpeed = 2.5f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Points = new Vector2[PointsObject.childCount];

        int i = 0;
        foreach (Transform child in PointsObject)
        {
            Points[i] = child.position;
            i++;
        }

        transform.position = Points[0];
    }

    /// <summary>
    /// Update is called once per frame.
    /// Sets the next target position when current target is reached.
    /// When final position reached starts moving towards the first position.
    /// </summary>
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Points[PointsIndex], MoveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == Points[PointsIndex])
        {
            PointsIndex++;

            if (PointsIndex == Points.Length)
            {
                PointsIndex = 0;
            }
        }
    }

    //=================================================== Collisions ===============================================

    /// <summary>
    /// Keeps the player on the platform by making the player a child of the object.
    /// </summary>
    /// <param name="collision"><see cref="Collision2D"/></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    /// <summary>
    /// Releases the player by setting the player's parent to null when the player leaves the platform.
    /// </summary>
    /// <param name="collision"><see cref="Collision2D"/></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        DontDestroyOnLoad(collision.gameObject);
    }
}
