using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSM : BaseStateMachine<IState>
{
    [SerializeField]
    private GameObject m_mainMenu;
    [SerializeField]
    private GameObject m_optionsMenu;

    private void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
        CreatePossibleStates();
    }
    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<IState>();
        m_possibleStates.Add(new MainMenuState());
        m_possibleStates.Add(new LobbyState());
    }
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
