using UnityEngine;

/// <summary>
/// Custom check box for turning off the controls overlay
/// </summary>
/// Author: jmolive8
public class ControlsOverlayCheckBox : MonoBehaviour
{
    public GameObject CheckMark;
    public GameObject ControlsOverlay;

    /// <summary>
    /// Toggles the check mark and the visibility of the Controls Overlay
    /// </summary>
    public void ToggleCheckMark()
    {
        CheckMark.SetActive(!CheckMark.activeInHierarchy);
        ControlsOverlay.SetActive(!ControlsOverlay.activeInHierarchy);
    }
}
