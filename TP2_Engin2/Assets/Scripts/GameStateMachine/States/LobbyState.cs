using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyState : GM_IState
{
    public bool CanEnter(GM_IState currentState)
    {
        return SceneManager.GetActiveScene().buildIndex == 1;
    }

    public bool CanExit()
    {
        return SceneManager.GetActiveScene().buildIndex != 1;
    }

    public void OnEnter()
    {
        Debug.Log("Entering Lobby State!");
    }

    public void OnExit()
    {
        Debug.Log("Leaving lobby State!");
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
