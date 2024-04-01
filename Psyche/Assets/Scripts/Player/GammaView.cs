/** 
Description: Gamma Ray Neutron Spectrometer Gamma View Script
Author: blopezro
Version: 20240326
**/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Gamma View script which captures all sprites of the game objects within the scene
/// and changes their colors based on the layer assigned to the game objects
/// </summary>
public class GammaView : MonoBehaviour
{

    /* arrays and lists that hold data of the grns */
    public List<GameObject> sceneObjects;            // holds all current game objects in the scene
    public List<SpriteRenderer> spriteRenderersList; // holds all sprite renderers of game objects
    public Color[] origColorArray;                   // holds original colors of the sprite renderers
    public List<GameObject> colorBlindModeObjects;   // holds objects stored for assistance with color blindness
    /* scene light */
    public Light2D sceneLight;                       // light in the current scene
    public float origSceneLightIntensity;            // light intensity of the scene
    public bool grnsControlsSceneLight = false;      // boolean to set if grns will influence scene light    
    /* scene terrain */
    public Tilemap sceneTilemap;                     // tilemap of terrain component in current scene
    public Tilemap sceneTilemapFake;                 // tilemap of fake terrain component in current scene
    public Color origSceneTilemapColor;              // terrain color of the scene
    /* scene background image */
    public SpriteRenderer sceneBackground;           // background image in current scene
    public Color origSceneBackgroundColor;           // background color of the scene
    /* other variables */
    public Color defaultColor = Color.green;         // default color to use when default layer is used
    public bool colorBlindMode;                      // color blind mode boolean
    public bool colorBlindModeNames = true;          // boolean to set if color blind mode will set layer name vice number
    public Camera mainCamera;                        // scene camera used to only load objects within view

    /// <summary>
    /// Subscribes to the SceneManager.sceneLoaded event
    /// </summary>
    void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from event to prevent memory loss
    /// </summary>
    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Runs each time a new scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStateManager.Scene GameStateManagerSn = GameController.Instance.gameStateManager.MatchScene(scene.name);

        if (GameStateManagerSn != GameStateManager.Scene.Outro)
        {
            sceneLight = GameObject.FindGameObjectWithTag("SceneLight").GetComponent<Light2D>();
            sceneTilemap = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();

            if (GameStateManagerSn == GameStateManager.Scene.Landing ||
                GameStateManagerSn == GameStateManager.Scene.eMagnet ||
                GameStateManagerSn == GameStateManager.Scene.Combo1 ||
                GameStateManagerSn == GameStateManager.Scene.Combo3)
            {
                sceneTilemapFake = GameObject.FindGameObjectWithTag("FakeTerrain").GetComponent<Tilemap>();
            }

            sceneBackground = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
            origSceneLightIntensity = sceneLight.intensity;
            origSceneTilemapColor = sceneTilemap.color;
            origSceneBackgroundColor = sceneBackground.color;

            if (GameStateManagerSn != GameStateManager.Scene.Landing)
            {
                TurnOffSceneLight();
            }
        }
    }

    /// <summary>
    /// Checks every frame to see if GRNS tool is active and 
    /// if it's not will ensure it's deactivated 
    /// </summary>
    private void Update()
    {
        DeactivateGRNS();
    }

    /// <summary>
    /// Objects loaded at the activation of the GRNS
    /// </summary>
    void Initialize()
    {
        // sets main camera for use during objects in view capture
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // capture only objects within cameras view in scene
        sceneObjects = CaptureObjectsInView();
        CaptureSpriteRenderers(sceneObjects);

        // resizes based on sprite renderers in the scene
        Array.Resize(ref origColorArray, spriteRenderersList.Count);

        // captures original colors of sprites
        for (int i = 0; i < spriteRenderersList.Count; i++)
        {
            origColorArray[i] = spriteRenderersList[i].color;
        }

        // apply modifications to sprites when color blind mode is enabled
        colorBlindMode = GameController.Instance.colorBlindMode;
        if (colorBlindMode)
        {
            foreach (var spriteRenderer in spriteRenderersList)
            {
                ApplyColorBlindModifications(spriteRenderer);
            }
        }

        // changes terrain and background color
        ChangeTerrainAndBackgroundColor();
    }

    /// <summary>
    /// Capture only game objects within the cameras view, these objects must 
    /// have sprite renderer components in order to be detected 
    /// </summary>
    /// <returns></returns>
    List<GameObject> CaptureObjectsInView()
    {
        List<GameObject> objectsInView = new List<GameObject>();
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            GameObject obj = sprite.gameObject;
            // check if the object is within the camera's viewport
            // and that it does not have layer 5 (UI) or 16 (GRNS_Ignore)
            if (IsObjectInView(obj) && !obj.layer.Equals(5))
            {
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
    bool IsObjectInView(GameObject obj)
    {
        Bounds bounds = obj.GetComponent<SpriteRenderer>().bounds;
        Vector3 objectPosition = bounds.center;
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(objectPosition);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    /// <summary>
    /// Activates gamma ray neutron spectrometer and shows new color
    /// </summary>
    public void ActivateGRNS()
    {
        Initialize(); //DebugReportLog();
        for (int i = 0; i < spriteRenderersList.Count; i++)
        {
            if (spriteRenderersList[i] != null)
            {
                spriteRenderersList[i].color = LayerColor(spriteRenderersList[i].gameObject);
                if (Input.GetButtonDown("FireGRNS"))
                {
                    GameController.Instance.audioManager.toolGRNS.Play();
                }
                if (colorBlindMode) { ActivateGRNSaltView(); }
                if (!sceneLight.intensity.Equals(1) && grnsControlsSceneLight)
                {
                    TurnOnSceneLight();
                }
            }
        }
    }

    /// <summary>
    /// Deactivates gamma ray neutron spectrometer and reverts to original color
    /// </summary>
    public void DeactivateGRNS()
    {
        for (int i = 0; i < spriteRenderersList.Count; i++)
        {
            if (spriteRenderersList[i] != null)
            {
                spriteRenderersList[i].color = origColorArray[i];
                if (Input.GetButtonUp("FireGRNS"))
                {
                    GameController.Instance.audioManager.toolGRNS.Stop();
                }
                if (colorBlindMode) { DeactivateGRNSaltView(); }
                if (!sceneLight.intensity.Equals(origSceneLightIntensity) && grnsControlsSceneLight)
                {
                    RevertSceneLight();
                }
            }
        }
        DeInitialize(); //DebugReportLog();
    }

    /// <summary>
    /// Turns on the scene light
    /// </summary>
    void TurnOnSceneLight()
    {
        sceneLight.intensity = 1;
    }

    /// <summary>
    /// Turns on the scene light
    /// </summary>
    void TurnOffSceneLight()
    {
        sceneLight.intensity = 0;
    }

    /// <summary>
    /// Reverts the scene light to original intensity
    /// </summary>
    void RevertSceneLight()
    {
        sceneLight.intensity = origSceneLightIntensity;
    }

    /// <summary>
    /// Adds to the list the sprite renderers from the game objects in the scene
    /// </summary>
    /// <param name="sceneObjects"></param>
    void CaptureSpriteRenderers(List<GameObject> sceneObjects)
    {
        foreach (GameObject obj in sceneObjects)
        {
            if (obj.GetComponent(typeof(SpriteRenderer)))
            {
                spriteRenderersList.Add((SpriteRenderer)obj.GetComponent(typeof(SpriteRenderer)));
            }
        }
    }

    /// <summary>
    /// Returns the game objects layer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    int GetLayer(GameObject obj)
    {
        return obj.layer;
    }

    /// <summary>
    /// Returns set color for the layer
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Color LayerColor(GameObject obj)
    {
        // if colorblind mode return clear or default color
        if (colorBlindMode)
        {
            return obj.layer switch
            {
                15 => Color.clear,
                _ => defaultColor,
            };
        }
        else
        {
            return obj.layer switch
            {
                // User Layers within Unity are 3 and 6-31                
                3 => Color.green,  // Terrain
                6 => Color.red,    // Titanium
                7 => Color.yellow, // Iron
                8 => Color.black,  // Cave
                9 => Color.grey,   // Nickel
                10 => Color.blue,   // Copper
                11 => Color.gray,   // Silver
                12 => Color.magenta,// Tungsten
                13 => Color.white,  // Platinum
                14 => Color.cyan,   // Gold
                15 => Color.clear,  // Clear
                _ => defaultColor,
            };
        }
    }

    /// <summary>
    /// Modifies game object properties to assist with color blind players
    /// by displaying the layers text vice a new color
    /// </summary>
    /// <param name="spriteRenderer"></param>
    void ApplyColorBlindModifications(SpriteRenderer spriteRenderer)
    {
        if (colorBlindMode)
        {
            GameObject gameObject = spriteRenderer.gameObject;      // get current game object
            int layerNum = gameObject.layer;                        // get layer number of game object
            string layerName = LayerMask.LayerToName(layerNum);     // get layer name of game object

            // create game object to display text and anchor in center
            GameObject grnsTextObject = new GameObject("GRNS_ColorBlindMode");
            TextMesh textMesh = grnsTextObject.AddComponent<TextMesh>();

            // initially set to false until GRNS is activated
            grnsTextObject.SetActive(false);
            colorBlindModeObjects.Add(grnsTextObject);

            // set the text and game object properties
            SetTextMeshProperties(textMesh, layerNum, layerName);
            SetTextObjProperties(grnsTextObject, spriteRenderer);
        }
    }

    /// <summary>
    /// Sets the text properties of the given text mesh
    /// </summary>
    /// <param name="textMesh"></param>
    /// <param name="layerNum"></param>
    /// <param name="layerName"></param>
    public void SetTextMeshProperties(TextMesh textMesh, int layerNum, String layerName)
    {
        // text properties based on layer number
        textMesh.text = layerNum switch
        {
            <= 5 or >= 15 => "", // layers to ignore for labeling
            // else displays text based on bool? true:false
            _ => colorBlindModeNames ? layerName.ToString() : layerNum.ToString(),
        };
        textMesh.characterSize = 0.15f;
        textMesh.fontSize = 30;
        textMesh.font = Resources.Load<Font>("");
        textMesh.color = Color.yellow;
        textMesh.anchor = TextAnchor.MiddleCenter;
    }

    /// <summary>
    /// Sets the game object properties
    /// </summary>
    /// <param name="grnsTextObject"></param>
    /// <param name="spriteRenderer"></param>
    public void SetTextObjProperties(GameObject grnsTextObject, SpriteRenderer spriteRenderer)
    {
        // set the text object as a child of the sprite renderer
        grnsTextObject.transform.parent = spriteRenderer.transform;
        // position at the center and rotate 45 degrees
        grnsTextObject.transform.position = spriteRenderer.transform.position;
        grnsTextObject.transform.Rotate(new Vector3(0, 0, 45));
        // set sorting layer and order to view text in front of game object
        grnsTextObject.GetComponent<Renderer>().sortingLayerName = "UI";
        grnsTextObject.GetComponent<Renderer>().sortingOrder = 1;
    }

    /// <summary>
    /// Show alternative view
    /// </summary>
    void ActivateGRNSaltView()
    {
        foreach (GameObject obj in colorBlindModeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Hide alternative view and additionally destroy game object
    /// </summary>
    void DeactivateGRNSaltView()
    {
        foreach (GameObject obj in colorBlindModeObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// Reports list and array sizes
    /// </summary>
    void DebugReportLog()
    {
        Debug.Log("Objects captured, Sprite renderers captured, Orig color array length, Colorblind objects: "
        + sceneObjects.Count + ", " + spriteRenderersList.Count + ", " + origColorArray.Length + ", " + colorBlindModeObjects.Count);
    }

    /// <summary>
    /// Used to clear out lists and arrays
    /// </summary>
    void DeInitialize()
    {
        sceneObjects.Clear();
        spriteRenderersList.Clear();
        origColorArray = new Color[0];
        colorBlindModeObjects.Clear();
        RevertTerrainAndBackgroundColor();
    }

    /// <summary>
    /// Changes the color of the terrain and background
    /// </summary>
    void ChangeTerrainAndBackgroundColor()
    {
        if (sceneTilemap != null) { sceneTilemap.color = defaultColor; }
        if (sceneTilemapFake != null) { sceneTilemapFake.color = LayerColor(sceneTilemapFake.gameObject); }
        if (sceneBackground != null) { sceneBackground.color = defaultColor; }
    }

    /// <summary>
    /// Reverts the color of the terrain and background
    /// </summary>
    void RevertTerrainAndBackgroundColor()
    {
        if (sceneTilemap != null) { sceneTilemap.color = origSceneTilemapColor; }
        if (sceneTilemapFake != null) { sceneTilemapFake.color = origSceneTilemapColor; }
        if (sceneBackground != null) { sceneBackground.color = origSceneBackgroundColor; }
    }

}