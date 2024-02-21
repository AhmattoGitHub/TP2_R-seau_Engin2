using UnityEngine;
using UnityEngine.UI;
public class volumeslider : MonoBehaviour
{
    public Slider m_volumeSlider;

    void Start()
    {

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = m_volumeSlider.value;
        Save();
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("Music", m_volumeSlider.value);
    }
    public void Load()
    {
        m_volumeSlider.value = PlayerPrefs.GetFloat("Music");
        AudioListener.volume = m_volumeSlider.value;
    }
}