using UnityEngine;

/// <summary>
/// Custom check box for turning off the controls overlay
/// </summary>
/// Author: jmolive8
public class ControlsOverlayCheckBox : MonoBehaviour
{
    public GameObject checkMark;
    public GameObject controlsOverlay;

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark()
    {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        controlsOverlay.SetActive(!controlsOverlay.activeInHierarchy);
    }
}
