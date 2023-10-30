/** 
Description: spectrometer tool gamma view script
Author: blopezro
Version: 20231030
**/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gamma View script which captures all sprites of the game objects within the scene
/// and changes their colors based on the layer assigned to the game objects.
/// </summary>
public class GammaView : MonoBehaviour {

    public GameObject[] sceneObjects;                // holds all current game objects in the scene
    public List<SpriteRenderer> spriteRenderersList; // holds all sprite renderers of game objects
    public Color[] origColorArray;                   // holds original colors of the sprite renderers
    public Color defaultColor = Color.green;         // default color of items when default layer is used
    public List<GameObject> colorBlindModeObjects;   // text objects stored for assistance with color blindness
    public bool colorBlindMode;                      // color blind mode 

    // subscribes to the SceneManager.sceneLoaded event
    void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // unsubscribes from event to prevent memory loss
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // runs each time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // clears and empties out items before use in the scene
        sceneObjects = new GameObject[0];
        spriteRenderersList.Clear();
        origColorArray = new Color[0];
        colorBlindModeObjects.Clear();
        Debug.Log("Reloaded needed data for GRS");

        sceneObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        CaptureSpriteRenderers(sceneObjects);

        // resizes based on sprite renderers in the scene
        Array.Resize(ref origColorArray, spriteRenderersList.Count);
        
        // captures original colors of sprites
        for (int i = 0; i < spriteRenderersList.Count; i++) {
                origColorArray[i] = spriteRenderersList[i].color;
        }

        // apply modifications to sprites when color blind mode is enabled
        if (colorBlindMode) {
            foreach (var spriteRenderer in spriteRenderersList) {
                ApplyColorBlindModifications(spriteRenderer);
            }
        }
    }

    // activates gamma ray spectrometer
    public void ActivateGRS(AudioManager audioManager) {
        // shows new color
        if (Input.GetKeyDown(KeyCode.G)) {
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                if (spriteRenderersList[i] != null) {
                    spriteRenderersList[i].color = LayerColor(spriteRenderersList[i].gameObject);
                    audioManager.PlayToolGRS();
                    if (colorBlindMode) {
                        ActivateGRSnumberObjs();
                    }
                }
            }
        }
    }

    // deactivates gamma ray spectrometer
    public void DeactivateGRS(AudioManager audioManager) {
        // reverts to original color
        if (Input.GetKeyUp(KeyCode.G)) {
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                if (spriteRenderersList[i] != null) {
                    spriteRenderersList[i].color = origColorArray[i];
                    audioManager.StopToolGRS();
                    if (colorBlindMode) {
                        DeactivateGRSnumberObjs();
                    }
                }
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
        if (colorBlindMode) {
            return defaultColor;
        } else {
            return obj.layer switch {
                // User Layers within Unity are 3 and 6-31
                3 =>  Color.black,  // Terrain
                6 =>  Color.grey,   // Rock
                7 =>  Color.grey,   // Metal
                8 =>  Color.magenta,// Crystal
                9 =>  Color.green,  // Grass
                10 => Color.blue,   // FloatingPlatform
                11 => Color.green,  // Background
                12 => Color.blue,   // Lifeform
                13 => Color.clear,  // Clear
                14 => Color.grey,   // Iron
                15 => Color.grey,   // Nickel
                16 => Color.yellow, // Sulfur
                17 => Color.white,  // Oxygen
                18 => Color.cyan,   // Silicon
                19 => Color.blue,   // Hydrogen
                20 => Color.black,  // Carbon
                _ => defaultColor,
            };
        }
    }

    // modifies game object properties to assist with color blind players
    void ApplyColorBlindModifications(SpriteRenderer spriteRenderer) {
        if (colorBlindMode) {
            // get current game object
            GameObject gameObject = spriteRenderer.gameObject;
            // get layer of game object
            int numberToDisplay = gameObject.layer;

            // create game object to display number and anchor in center
            GameObject grsNumberObject = new GameObject("GRS_ColorBlindMode");
            TextMesh textMesh = grsNumberObject.AddComponent<TextMesh>();
            
            // initially set to false until GRS is activated
            grsNumberObject.SetActive(false);
            colorBlindModeObjects.Add(grsNumberObject);

            // text properties
            textMesh.text = numberToDisplay.ToString();
            textMesh.characterSize = 0.15f;
            textMesh.fontSize = 30;
            textMesh.font = Resources.Load<Font>("");
            textMesh.color = Color.white;
            textMesh.anchor = TextAnchor.MiddleCenter;

            // position the number object at the center of the object and set parent
            grsNumberObject.transform.position = spriteRenderer.transform.position;
            grsNumberObject.transform.parent = spriteRenderer.transform;

            // set sorting layer and order to view text in front of game object
            grsNumberObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
            grsNumberObject.GetComponent<Renderer>().sortingOrder = 1;
        } else {
            // destroy number object if created
            GameObject numberObject = spriteRenderer.transform.Find("GRS_ColorBlindMode").gameObject;
            if (numberObject != null) {
                colorBlindModeObjects.Remove(numberObject);
                Destroy(numberObject);
            }
        }
    }

    // show numbers
    void ActivateGRSnumberObjs() {
        foreach (GameObject numberObject in colorBlindModeObjects) {
            if (numberObject != null) {
                numberObject.SetActive(true);
            }
        }
    }

    // hide numbers
    void DeactivateGRSnumberObjs() {
        foreach (GameObject numberObject in colorBlindModeObjects) {
            if (numberObject != null) {
                numberObject.SetActive(false);
            }
        }
    }

}