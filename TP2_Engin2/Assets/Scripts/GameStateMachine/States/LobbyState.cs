using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyState : IState
{
    public bool CanEnter(IState currentState)
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
