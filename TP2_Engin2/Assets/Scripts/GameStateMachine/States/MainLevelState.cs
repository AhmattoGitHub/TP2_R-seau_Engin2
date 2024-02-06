using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelState : GM_IState
{
    public bool CanEnter(GM_IState currentState)
    {
        return SceneManager.GetActiveScene().buildIndex == 2;
    }

    public bool CanExit()
    {
        return SceneManager.GetActiveScene().buildIndex != 2;
    }

    public void OnEnter()
    {
        Debug.Log("Entering Main Level State!");
    }

    public void OnExit()
    {
        Debug.Log("Leaving Main Level State!");
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
