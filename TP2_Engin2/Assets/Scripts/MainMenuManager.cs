using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_mainMenu;
    [SerializeField]
    private GameObject m_optionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PlayTutorial()
    {
        SceneManager.LoadScene(3);
    }
    public void OpenOptionsMenu()
    {
        m_mainMenu.SetActive(false);
        m_optionsMenu.SetActive(true);
    }
    public void CloseOptionsMenu()
    {
        m_mainMenu.SetActive(true);
        m_optionsMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
