using UnityEngine;

/// <summary>
/// Custom check box for managing color blind mode
/// </summary>
/// Author: jmolive8
public class ColorBlindCheckBox : MonoBehaviour
{
    public GameObject checkMark;

    private void OnEnable()
    {
        checkMark.SetActive(TempGameManager.colorBlindMode);
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark()
    {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        TempGameManager.ChangeColorBlindMode(checkMark.activeInHierarchy);
    }
}
