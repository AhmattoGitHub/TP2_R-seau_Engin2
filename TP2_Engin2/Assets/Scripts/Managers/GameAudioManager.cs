using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    private static GameAudioManager _instance;
    [SerializeField] private AudioSource m_mainMenuAudioSource;
    [SerializeField] private AudioSource m_lobbySceneAudioSource;
    [SerializeField] private AudioClip m_buttonClickSFX;
    [SerializeField] private AudioClip m_lobbyCountdownSFX;

    // Property to access the singleton instance
    public static GameAudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no GameAudioMangager Instance");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayButtonClickOnMainMenu()
    {
        m_mainMenuAudioSource.PlayOneShot(m_buttonClickSFX);
    }

    public void PlayButtonClickOnLobby()
    {
        m_lobbySceneAudioSource.PlayOneShot(m_buttonClickSFX);
    }

    public void PlayLobbyCountdown()
    {
        m_lobbySceneAudioSource.Stop();
        m_lobbySceneAudioSource.PlayOneShot(m_lobbyCountdownSFX);
    }
}
