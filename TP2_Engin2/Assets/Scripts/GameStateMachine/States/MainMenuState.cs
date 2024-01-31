using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : GM_IState
{
    public bool CanEnter(GM_IState currentState)
    {
        return (SceneManager.GetActiveScene().buildIndex == 0);
    }

    public bool CanExit()
    {
        return !(SceneManager.GetActiveScene().buildIndex == 0);
    }

    public void OnEnter()
    {
        Debug.Log("Entering Main Menu State!");
    }

    public void OnExit()
    {
        Debug.Log("Leaving Main Menu State!");
    }

    public void OnFixedUpdate()
    {

    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {

    }
}
