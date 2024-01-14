using System.Collections;
using System.Collections.Generic;
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

        m_resolutionDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < m_resolutions.Length; i++)
        {
            int currentRefreshRate = ((int)m_resolutions[i].refreshRateRatio.value);
            string resolution = m_resolutions[i].width + " x " + m_resolutions[i].height + " " + currentRefreshRate + "Hz";
            resolutionList.Add(resolution);

            if (m_resolutions[i].width == Screen.currentResolution.width && 
                m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_resolutionDropdown.AddOptions(resolutionList);
        m_resolutionDropdown.value = currentResolutionIndex;
        m_resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = m_resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
