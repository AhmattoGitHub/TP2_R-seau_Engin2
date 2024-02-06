using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    private Resolution[] m_resolutions;

    [SerializeField]
    private TMPro.TMP_Dropdown m_resolutionDropdown;

    // Start is called before the first frame update
    void Start()
    {

        m_resolutions = Screen.resolutions;

        InitDropdownList(m_resolutions.Length);
    }

    private void InitDropdownList(int listLength)
    {
        m_resolutionDropdown.ClearOptions();
        List<string> resolutionList = new List<string>();
        int currentResolution = 0;
        for(int i = 0; i < listLength; i++)
        {
            int refreshRate = (int)m_resolutions[i].refreshRateRatio.value;
            string resolution = "w: " + m_resolutions[i].width + " h: " + m_resolutions[i].height + "- " + refreshRate + "Hz";
            resolutionList.Add(resolution);
            if (m_resolutions[i].width == Screen.currentResolution.width &&
                m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        m_resolutionDropdown.AddOptions(resolutionList);
        m_resolutionDropdown.value = currentResolution;
        m_resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = m_resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
