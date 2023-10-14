/** 
Description: spectrometer tool gamma view script
Author: blopezro
Version: 20231014
**/

using System;
using System.Collections.Generic;
using UnityEngine;

public class GammaView : MonoBehaviour {

    public GameObject[] sceneObjects;                // holds all current game objects in the scene
    public List<SpriteRenderer> spriteRenderersList; // holds all sprite renderers of game objects
    public Color[] origColorArray;                   // holds original colors of the sprite renderers
    public Color tempColor = Color.green;            // testing / default color

    // Start is called before the first frame update
    void Start() {
        sceneObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        CaptureSpriteRenderers(sceneObjects);

        // resizes based on sprite renderers in the scene
        Array.Resize(ref origColorArray, spriteRenderersList.Count);
        
        // captures original colors of sprites
        for (int i = 0; i < spriteRenderersList.Count; i++) {
                origColorArray[i] = spriteRenderersList[i].color;
        }
    }

    // Update is called when keyboard keys are pressed
    void Update() {
        // shows new color
        if (Input.GetKeyDown(KeyCode.G)) {
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                spriteRenderersList[i].color = LayerColor(spriteRenderersList[i].gameObject);
            }
        }

        // reverts to original color
        if (Input.GetKeyUp(KeyCode.G)) {
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                spriteRenderersList[i].color = origColorArray[i];
            }
        }
    }

    // adds to the list the sprite renderers from the game objects in the scene
    void CaptureSpriteRenderers(GameObject[] sceneObjects) {
        foreach (GameObject obj in sceneObjects) {
            if (obj.GetComponent(typeof(SpriteRenderer))) {
                spriteRenderersList.Add((SpriteRenderer) obj.GetComponent(typeof(SpriteRenderer)));
            }           
        }
    }

    // returns the game objects layer
    int GetLayer(GameObject obj) {
        return obj.layer;
    }

    // returns set color for the layer
    Color LayerColor(GameObject obj) {
        return tempColor;
    }

}