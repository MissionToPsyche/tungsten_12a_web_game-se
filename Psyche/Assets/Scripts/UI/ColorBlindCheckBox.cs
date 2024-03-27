/**
Description: Custom check box for managing color blind mode
Author: jmolive8
Version: 20240326
**/

using UnityEngine;

public class ColorBlindCheckBox : MonoBehaviour {
    public GameObject checkMark;

    /// <summary>
    /// Activates color blind mode instance
    /// </summary>
    private void OnEnable() {
        checkMark.SetActive(GameController.Instance.colorBlindMode);
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark() {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        GameController.Instance.ChangeColorBlindMode(checkMark.activeInHierarchy);
    }
    
}
