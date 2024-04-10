/** 
Description: Gamma Ray Neutron Spectrometer Gamma View Script
Author: blopezro
Version: 20240410
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
    public List<GameObject> SceneObjects;            // holds all current game objects in the scene
    public List<SpriteRenderer> SpriteRenderersList; // holds all sprite renderers of game objects
    public Color[] OrigColorArray;                   // holds original colors of the sprite renderers
    public List<GameObject> ColorBlindModeObjects;   // holds objects stored for assistance with color blindness
    /* scene light */
    public Light2D SceneLight;                       // light in the current scene
    public float OrigSceneLightIntensity;            // light intensity of the scene
    public bool GrnsControlsSceneLight = false;      // boolean to set if grns will influence scene light    
    /* scene terrain */
    public Tilemap SceneTilemap;                     // tilemap of terrain component in current scene
    public Tilemap SceneTilemapFake;                 // tilemap of fake terrain component in current scene
    public Color OrigSceneTilemapColor;              // terrain color of the scene
    /* scene background image */
    public SpriteRenderer SceneBackground;           // background image in current scene
    public Color OrigSceneBackgroundColor;           // background color of the scene
    /* other variables */
    public Color DefaultColor = Color.green;         // default color to use when default layer is used
    public bool ColorBlindMode;                      // color blind mode boolean
    public bool ColorBlindModeNames = true;          // boolean to set if color blind mode will set layer name vice number
    public Camera MainCamera;                        // scene camera used to only load objects within view

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
    /// <param name="Scene"></param>
    /// <param name="Mode"></param>
    void OnSceneLoaded(Scene Scene, LoadSceneMode Mode)
    {
        GameStateManager.Scene GameStateManagerSn = GameController.Instance.GameStateManager.MatchScene(Scene.name);
        if (GameStateManagerSn != GameStateManager.Scene.Outro)
        {
            SceneLight = GameObject.FindGameObjectWithTag("SceneLight").GetComponent<Light2D>();
            SceneTilemap = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Tilemap>();
            if (GameStateManagerSn == GameStateManager.Scene.EMagnet
                || GameStateManagerSn == GameStateManager.Scene.Combo1
                || GameStateManagerSn == GameStateManager.Scene.Combo3)
            {
                SceneTilemapFake = GameObject.FindGameObjectWithTag("FakeTerrain").GetComponent<Tilemap>();
            }

            SceneBackground = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
            OrigSceneLightIntensity = SceneLight.intensity;
            OrigSceneTilemapColor = SceneTilemap.color;
            OrigSceneBackgroundColor = SceneBackground.color;

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
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }

        // capture only objects within cameras view in scene
        SceneObjects = CaptureObjectsInView();
        CaptureSpriteRenderers(SceneObjects);

        // resizes based on sprite renderers in the scene
        Array.Resize(ref OrigColorArray, SpriteRenderersList.Count);

        // captures original colors of sprites
        for (int i = 0; i < SpriteRenderersList.Count; i++)
        {
            OrigColorArray[i] = SpriteRenderersList[i].color;
        }

        // apply modifications to sprites when color blind mode is enabled
        ColorBlindMode = GameController.Instance.ColorBlindMode;
        if (ColorBlindMode)
        {
            foreach (var SpriteRenderer in SpriteRenderersList)
            {
                ApplyColorBlindModifications(SpriteRenderer);
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
            GameObject Obj = sprite.gameObject;
            // check if the object is within the camera's viewport
            // and that it does not have layer 5 (UI) or 16 (GRNS_Ignore)
            if (IsObjectInView(Obj) && !Obj.layer.Equals(5))
            {
                // add the object to the list for scanning or processing
                objectsInView.Add(Obj);
            }
        }
        return objectsInView;
    }

    /// <summary>
    /// Checks if Obj is within bounds
    /// </summary>
    /// <param name="Obj"></param>
    /// <returns></returns>
    bool IsObjectInView(GameObject Obj)
    {
        Bounds bounds = Obj.GetComponent<SpriteRenderer>().bounds;
        Vector3 objectPosition = bounds.center;
        Vector3 screenPoint = MainCamera.WorldToViewportPoint(objectPosition);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    /// <summary>
    /// Activates gamma ray neutron spectrometer and shows new color
    /// </summary>
    public void ActivateGRNS()
    {
        Initialize(); //DebugReportLog();
        for (int i = 0; i < SpriteRenderersList.Count; i++)
        {
            if (SpriteRenderersList[i] != null)
            {
                SpriteRenderersList[i].color = LayerColor(SpriteRenderersList[i].gameObject);
                if (Input.GetButtonDown("FireGRNS"))
                {
                    GameController.Instance.AudioManager.ToolGRNS.Play();
                }
                if (ColorBlindMode)
                {
                    ActivateGRNSaltView();
                }
                if (!SceneLight.intensity.Equals(1) && GrnsControlsSceneLight)
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
        for (int i = 0; i < SpriteRenderersList.Count; i++)
        {
            if (SpriteRenderersList[i] != null)
            {
                SpriteRenderersList[i].color = OrigColorArray[i];
                if (Input.GetButtonUp("FireGRNS"))
                {
                    GameController.Instance.AudioManager.ToolGRNS.Stop();
                }
                if (ColorBlindMode)
                {
                    DeactivateGRNSaltView();
                }
                if (!SceneLight.intensity.Equals(OrigSceneLightIntensity) && GrnsControlsSceneLight)
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
        SceneLight.intensity = 1;
    }

    /// <summary>
    /// Turns on the scene light
    /// </summary>
    void TurnOffSceneLight()
    {
        SceneLight.intensity = 0;
    }

    /// <summary>
    /// Reverts the scene light to original intensity
    /// </summary>
    void RevertSceneLight()
    {
        SceneLight.intensity = OrigSceneLightIntensity;
    }

    /// <summary>
    /// Adds to the list the sprite renderers from the game objects in the scene
    /// </summary>
    /// <param name="SceneObjects"></param>
    void CaptureSpriteRenderers(List<GameObject> SceneObjects)
    {
        foreach (GameObject Obj in SceneObjects)
        {
            if (Obj.GetComponent(typeof(SpriteRenderer)))
            {
                SpriteRenderersList.Add((SpriteRenderer)Obj.GetComponent(typeof(SpriteRenderer)));
            }
        }
    }

    /// <summary>
    /// Returns the game objects layer
    /// </summary>
    /// <param name="Obj"></param>
    /// <returns></returns>
    int GetLayer(GameObject Obj)
    {
        return Obj.layer;
    }

    /// <summary>
    /// Returns set color for the layer
    /// </summary>
    /// <param name="Obj"></param>
    /// <returns></returns>
    Color LayerColor(GameObject Obj)
    {
        // if colorblind mode return clear or default color
        if (ColorBlindMode)
        {
            return Obj.layer switch
            {
                15 => Color.clear,
                _ => DefaultColor,
            };
        }
        else
        {
            return Obj.layer switch
            {
                // User Layers within Unity are 3 and 6-31                
                3 => Color.green,   // Terrain
                6 => Color.red,     // Titanium
                7 => Color.yellow,  // Iron
                8 => Color.black,   // Cave
                9 => Color.grey,    // Nickel
                10 => Color.blue,   // Copper
                11 => Color.gray,   // Silver
                12 => Color.magenta,// Tungsten
                13 => Color.white,  // Platinum
                14 => Color.cyan,   // Gold
                15 => Color.clear,  // Clear
                _ => DefaultColor,
            };
        }
    }

    /// <summary>
    /// Modifies game object properties to assist with color blind players
    /// by displaying the layers text vice a new color
    /// </summary>
    /// <param name="SpriteRenderer"></param>
    void ApplyColorBlindModifications(SpriteRenderer SpriteRenderer)
    {
        if (ColorBlindMode)
        {
            GameObject gameObject = SpriteRenderer.gameObject;      // get current game object
            int LayerNum = gameObject.layer;                        // get layer number of game object
            string LayerName = LayerMask.LayerToName(LayerNum);     // get layer name of game object

            // create game object to display text and anchor in center
            GameObject GrnsTextObject = new GameObject("GRNS_ColorBlindMode");
            TextMesh TextMesh = GrnsTextObject.AddComponent<TextMesh>();

            // initially set to false until GRNS is activated
            GrnsTextObject.SetActive(false);
            ColorBlindModeObjects.Add(GrnsTextObject);

            // set the text and game object properties
            SetTextMeshProperties(TextMesh, LayerNum, LayerName);
            SetTextObjProperties(GrnsTextObject, SpriteRenderer);
        }
    }

    /// <summary>
    /// Sets the text properties of the given text mesh
    /// </summary>
    /// <param name="TextMesh"></param>
    /// <param name="LayerNum"></param>
    /// <param name="LayerName"></param>
    public void SetTextMeshProperties(TextMesh TextMesh, int LayerNum, String LayerName)
    {
        // text properties based on layer number
        TextMesh.text = LayerNum switch
        {
            <= 5 or >= 15 => "", // layers to ignore for labeling
            // else displays text based on bool? true:false
            _ => ColorBlindModeNames ? LayerName.ToString() : LayerNum.ToString(),
        };
        TextMesh.characterSize = 0.15f;
        TextMesh.fontSize = 30;
        TextMesh.font = Resources.Load<Font>("");
        TextMesh.color = Color.yellow;
        TextMesh.anchor = TextAnchor.MiddleCenter;
    }

    /// <summary>
    /// Sets the game object properties
    /// </summary>
    /// <param name="GrnsTextObject"></param>
    /// <param name="SpriteRenderer"></param>
    public void SetTextObjProperties(GameObject GrnsTextObject, SpriteRenderer SpriteRenderer)
    {
        // set the text object as a child of the sprite renderer
        GrnsTextObject.transform.parent = SpriteRenderer.transform;
        // position at the center and rotate 45 degrees
        GrnsTextObject.transform.position = SpriteRenderer.transform.position;
        GrnsTextObject.transform.Rotate(new Vector3(0, 0, 45));
        // set sorting layer and order to view text in front of game object
        GrnsTextObject.GetComponent<Renderer>().sortingLayerName = "UI";
        GrnsTextObject.GetComponent<Renderer>().sortingOrder = 1;
    }

    /// <summary>
    /// Show alternative view
    /// </summary>
    void ActivateGRNSaltView()
    {
        foreach (GameObject Obj in ColorBlindModeObjects)
        {
            if (Obj != null)
            {
                Obj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Hide alternative view and additionally destroy game object
    /// </summary>
    void DeactivateGRNSaltView()
    {
        foreach (GameObject Obj in ColorBlindModeObjects)
        {
            if (Obj != null)
            {
                Obj.SetActive(false);
                Destroy(Obj);
            }
        }
    }

    /// <summary>
    /// Reports list and array sizes
    /// </summary>
    void DebugReportLog()
    {
        Debug.Log("Objects captured, Sprite renderers captured, Orig color array length, Colorblind objects: "
        + SceneObjects.Count + ", " + SpriteRenderersList.Count + ", " + OrigColorArray.Length + ", " + ColorBlindModeObjects.Count);
    }

    /// <summary>
    /// Used to clear out lists and arrays
    /// </summary>
    void DeInitialize()
    {
        SceneObjects.Clear();
        SpriteRenderersList.Clear();
        OrigColorArray = new Color[0];
        ColorBlindModeObjects.Clear();
        RevertTerrainAndBackgroundColor();
    }

    /// <summary>
    /// Changes the color of the terrain and background
    /// </summary>
    void ChangeTerrainAndBackgroundColor()
    {
        if (SceneTilemap != null) { SceneTilemap.color = DefaultColor; }
        if (SceneTilemapFake != null) { SceneTilemapFake.color = LayerColor(SceneTilemapFake.gameObject); }
        if (SceneBackground != null) { SceneBackground.color = DefaultColor; }
    }

    /// <summary>
    /// Reverts the color of the terrain and background
    /// </summary>
    void RevertTerrainAndBackgroundColor()
    {
        if (SceneTilemap != null) { SceneTilemap.color = OrigSceneTilemapColor; }
        if (SceneTilemapFake != null) { SceneTilemapFake.color = OrigSceneTilemapColor; }
        if (SceneBackground != null) { SceneBackground.color = OrigSceneBackgroundColor; }
    }

}