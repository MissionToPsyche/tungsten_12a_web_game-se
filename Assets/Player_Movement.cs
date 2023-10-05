using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D character;

    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalDirection = Input.GetAxisRaw("Horizontal");

        character.velocity = new Vector2(horizontalDirection * 7f, character.velocity.y);

        if(Input.GetButtonDown("Jump"))
        {
            character.velocity = new Vector2(character.velocity.x, 10f);
        }
    }
}
