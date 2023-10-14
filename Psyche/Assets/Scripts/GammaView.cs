/** 
Description: spectrometer tool gamma view script
Author: blopezro
Version: 20231013
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GammaView : MonoBehaviour {
    
    // used to hold the sprites we want to change the color of
    public SpriteRenderer[] spriteRendererArray;

    // used to hold the new colors for the sprites
    public Color[] newColorArray;

    // used to hold the original colors (we do not need to set these in the UI)
    // just need to make sure we have the same size as spriteRendererArray 
    public Color[] origColorArray; 

    // Start is called before the first frame update
    void Start() {
        // captures original colors of sprites
        for (int i = 0; i < spriteRendererArray.Length; i++) {
            origColorArray[i] = spriteRendererArray[i].color;
        }
    }

    // Update is called when keyboard keys are pressed
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            for (int i = 0; i < spriteRendererArray.Length; i++) {
                spriteRendererArray[i].color = newColorArray[i];
            }
        }

        if (Input.GetKeyUp(KeyCode.G)) {
            for (int i = 0; i < spriteRendererArray.Length; i++) {
                spriteRendererArray[i].color = origColorArray[i];
            }
        }
    }
}