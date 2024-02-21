using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    private static GameAudioManager _instance;
    [SerializeField] private AudioSource m_mainMenuAudioSource;
    [SerializeField] private AudioSource m_lobbySceneAudioSource;
    [SerializeField] private AudioSource m_mainLevelAudioSource;
    [SerializeField] private AudioSource m_mainLevelPlayerAudioSource;
    [SerializeField] private AudioClip m_buttonClickSFX;
    [SerializeField] private AudioClip m_lobbyCountdownSFX;
    [SerializeField] private AudioClip m_bulletShotSFX;
    [SerializeField] private AudioClip m_jumpSFX;
    [SerializeField] private AudioClip m_doubleJumpSFX;
    [SerializeField] private AudioClip m_victoryMusic;
    [SerializeField] private AudioClip m_loseMusic;
    [SerializeField] private AudioClip m_knockedOutSFX;

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

    public void PlayBulletShot()
    {
        m_mainLevelPlayerAudioSource.PlayOneShot(m_bulletShotSFX);
    }

    public void PlayJumpSFX()
    {
        m_mainLevelPlayerAudioSource.PlayOneShot(m_jumpSFX);
    }

    public void PlayDoubleJumpSFX()
    {
        m_mainLevelPlayerAudioSource.PlayOneShot(m_doubleJumpSFX);
    }

    public void PlayVictoryMusic()
    {
        m_mainLevelAudioSource.Stop();
        m_mainLevelPlayerAudioSource.PlayOneShot(m_victoryMusic);
    }

    public void PlayLoseMusic()
    {
        m_mainLevelAudioSource.Stop();
        m_mainLevelPlayerAudioSource.PlayOneShot(m_loseMusic);
    }

    public void PlayKnockedOutSFX()
    {
        m_mainLevelPlayerAudioSource.PlayOneShot(m_knockedOutSFX);
    }
}
