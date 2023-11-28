/** 
Description: spectrometer tool gamma view script
Author: blopezro
Version: 20231124
**/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

/// <summary>
/// Gamma View script which captures all sprites of the game objects within the scene
/// and changes their colors based on the layer assigned to the game objects
/// </summary>
public class GammaView : MonoBehaviour {

    public List<GameObject> sceneObjects;            // holds all current game objects in the scene
    public List<SpriteRenderer> spriteRenderersList; // holds all sprite renderers of game objects
    public Color[] origColorArray;                   // holds original colors of the sprite renderers
    public Color defaultColor = Color.green;         // default color of items when default layer is used
    public List<GameObject> colorBlindModeObjects;   // objects stored for assistance with color blindness
    public bool colorBlindMode;                      // color blind mode boolean
    public Camera mainCamera;                        // scene camera used to only load objects within view
    public LayerMask scanLayer = -1;                 // set to -1 to include all layers
    public Light2D sceneLight;                       // light in the current scene
    public float origSceneLightIntensity;            // light intensity

    /// <summary>
    /// Subscribes to the SceneManager.sceneLoaded event
    /// </summary>
    void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from event to prevent memory loss
    /// </summary>
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Runs each time a new scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        sceneLight = GameObject.FindGameObjectWithTag("SceneLight").GetComponent<Light2D>();
        origSceneLightIntensity = sceneLight.intensity;
    }

    /// <summary>
    /// Objects loaded at the activation of the GRS
    /// </summary>
    void Initialize() {
        // sets main camera for use during objects in view capture
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        // capture only objects within cameras view in scene
        sceneObjects = CaptureObjectsInView();
        CaptureSpriteRenderers(sceneObjects);

        // resizes based on sprite renderers in the scene
        Array.Resize(ref origColorArray, spriteRenderersList.Count);
        
        // captures original colors of sprites
        for (int i = 0; i < spriteRenderersList.Count; i++) {
                origColorArray[i] = spriteRenderersList[i].color;
        }

        // apply modifications to sprites when color blind mode is enabled
        colorBlindMode = GameController.Instance.colorBlindMode;
        if (colorBlindMode) {
            foreach (var spriteRenderer in spriteRenderersList) {
                ApplyColorBlindModifications(spriteRenderer);
            }
        }
    }

    /// <summary>
    /// Capture only game objects within the cameras view, these objects must 
    /// have sprite renderer components in order to be detected 
    /// </summary>
    /// <returns></returns>
    List<GameObject> CaptureObjectsInView() {
        List<GameObject> objectsInView = new List<GameObject>();
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites) {
            GameObject obj = sprite.gameObject;
            // check if the object is within the camera's viewport
            if (IsObjectInView(obj)) {
                // add the object to the list for scanning or processing
                objectsInView.Add(obj);
            }
        }
        return objectsInView;
    }

    /// <summary>
    /// Checks if obj is within bounds
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    bool IsObjectInView(GameObject obj) {
        Bounds bounds = obj.GetComponent<SpriteRenderer>().bounds;
        Vector3 objectPosition = bounds.center;
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(objectPosition);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    /// <summary>
    /// Activates gamma ray spectrometer and shows new color
    /// </summary>
    public void ActivateGRS() {
        Initialize(); 
        //DebugReportLog(); // can comment out when not needed
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                if (spriteRenderersList[i] != null) {
                    spriteRenderersList[i].color = LayerColor(spriteRenderersList[i].gameObject);
                    GameController.Instance.audioManager.toolGRS.Play();
                    if (colorBlindMode) { ActivateGRSaltView(); }
                    if (!sceneLight.intensity.Equals(1)) {
                        TurnOnSceneLight();
                    }
                }
            }
    }

    /// <summary>
    /// Deactivates gamma ray spectrometer and reverts to original color
    /// </summary>
    public void DeactivateGRS() {
            for (int i = 0; i < spriteRenderersList.Count; i++) {
                if (spriteRenderersList[i] != null) {
                    spriteRenderersList[i].color = origColorArray[i];
                    GameController.Instance.audioManager.toolGRS.Stop();
                    if (colorBlindMode) { DeactivateGRSaltView(); }
                    if (!sceneLight.intensity.Equals(origSceneLightIntensity)) {
                        RevertSceneLight();
                    }
                }
            }
        DeInitialize();
        //DebugReportLog(); // can comment out when not needed
    }

    /// <summary>
    /// Turns on the scene light
    /// </summary>
    void TurnOnSceneLight() {
        sceneLight.intensity = 1;
    }

    /// <summary>
    /// Reverts the scene light to original intensity
    /// </summary>
    void RevertSceneLight() {
        sceneLight.intensity = origSceneLightIntensity;
    }

    /// <summary>
    /// Adds to the list the sprite renderers from the game objects in the scene
    /// </summary>
    /// <param name="sceneObjects"></param>
    void CaptureSpriteRenderers(List<GameObject> sceneObjects) {
        foreach (GameObject obj in sceneObjects) {
            if (obj.GetComponent(typeof(SpriteRenderer))) {
                spriteRenderersList.Add((SpriteRenderer) obj.GetComponent(typeof(SpriteRenderer)));
            }           
        }
    }

    /// <summary>
    /// Returns the game objects layer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    int GetLayer(GameObject obj) {
        return obj.layer;
    }

    /// <summary>
    /// Returns set color for the layer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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
                /** custom colors below to be used in future levels
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

    /// <summary>
    /// Modifies game object properties to assist with color blind players
    /// </summary>
    /// <param name="spriteRenderer"></param>
    void ApplyColorBlindModifications(SpriteRenderer spriteRenderer) {
        if (colorBlindMode) {
            // get current game object
            GameObject gameObject = spriteRenderer.gameObject;
            // get layer of game object
            int layerNum = gameObject.layer;

            // create game object to display number and anchor in center
            GameObject grsNumberObject = new GameObject("GRS_ColorBlindMode");
            TextMesh textMesh = grsNumberObject.AddComponent<TextMesh>();
            
            // initially set to false until GRS is activated
            grsNumberObject.SetActive(false);
            colorBlindModeObjects.Add(grsNumberObject);

            // text properties
            textMesh.text = layerNum.ToString();
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

    /// <summary>
    /// Show alternative view
    /// </summary>
    void ActivateGRSaltView() {
        foreach (GameObject obj in colorBlindModeObjects) {
            if (obj != null) {
                obj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Hide alternative view
    /// </summary>
    void DeactivateGRSaltView() {
        foreach (GameObject obj in colorBlindModeObjects) {
            if (obj != null) {
                obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Reports list and array sizes
    /// </summary>
    void DebugReportLog() {
        Debug.Log("Objects captured, Sprite renderers captured, Orig color array length, Colorblind objects: "
        +sceneObjects.Count+", "+spriteRenderersList.Count+", "+origColorArray.Length+", "+colorBlindModeObjects.Count);
    }

    /// <summary>
    /// Used to clear out lists and arrays
    /// </summary>
    void DeInitialize() {
        sceneObjects.Clear();
        spriteRenderersList.Clear();
        origColorArray = new Color[0];
        colorBlindModeObjects.Clear();
    }

}