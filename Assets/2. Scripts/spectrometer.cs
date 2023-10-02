/** 
Description: spectrometer tool script
Author: blopezro
Version: 20231001
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrometer : MonoBehaviour {

    public Color origColor; 
    public Color newColor;
    public SpriteRenderer render; 

    // Start is called before the first frame update
    void Start() {
        // captures original color of sprite
        origColor = render.color;
    }

    // Update is called when keyboard keys are pressed
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            render.color = newColor;
        }

        if (Input.GetKeyUp(KeyCode.G)) {
            render.color = origColor;
        }
    }
}
