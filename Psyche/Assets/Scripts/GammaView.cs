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
        colorBlindMode = TempGameManager.colorBlindMode;
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
                    audioManager.PlayAudio(audioManager.toolGRS);
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
                    audioManager.StopAudio(audioManager.toolGRS);
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
                3 =>  Color.green,  // Terrain
                6 =>  Color.red,    // Titanium
                7 =>  Color.yellow, // Iron
                8 =>  Color.black,  // Cobalt
                9 =>  Color.grey,   // Nickel
                10 => Color.blue,   // Copper
                11 => Color.gray,   // Silver
                12 => Color.magenta,// Tungsten
                13 => Color.white,  // Platinum
                14 => Color.cyan,   // Gold
                15 => Color.clear,  // Clear
                16 => new Color(0f, 0f, 0f, 0f),
                /** custom colors bellow to be used in future levels
                3 =>  new Color(0f, 0f, 0f, 1f),            // Terrain
                6 =>  new Color(0.5f, 0.5f, 0.5f, 1f),      // Titanium
                7 =>  new Color(0.5f, 0.5f, 0.5f, 1f),      // Iron
                8 =>  new Color(0f, 0f, 0f, 1f),            // Cobalt
                9 =>  new Color(0.5f, 0.5f, 0.5f, 1f),      // Nickel
                10 => new Color(0.647f, 0.165f, 0.165f, 1f),// Copper
                11 => new Color(0f, 0.502f, 0f, 1f),        // Silver
                12 => new Color(0f, 0f, 1f, 1f),            // Tungsten
                13 => new Color(0f, 0f, 0f, 0f),            // Platinum
                14 => new Color(0.5f, 0.5f, 0.5f, 1f),      // Gold
                15 => new Color(0f, 0f, 0f, 0f),            // Clear
                **/
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