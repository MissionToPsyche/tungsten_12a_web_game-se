using UnityEngine;

/// <summary>
/// Audio control for game
/// </summary>
/// Author: blopezro
public class AudioControl : MonoBehaviour
{
    public GameObject checkMark;

    private void OnEnable()
    {
        checkMark.SetActive(GameController.Instance.colorBlindMode);
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark()
    {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        GameController.Instance.ToggleBackgroundAudioMute();
    }
}