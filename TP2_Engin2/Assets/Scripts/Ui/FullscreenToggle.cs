using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle m_fullscreenToggle;

    private void Start()
    {
        m_fullscreenToggle.isOn = Screen.fullScreen;
    }
    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
