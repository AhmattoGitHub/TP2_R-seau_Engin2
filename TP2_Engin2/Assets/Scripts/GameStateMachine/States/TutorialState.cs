using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialState : GM_IState
{
    public bool CanEnter(GM_IState currentState)
    {
        return SceneManager.GetActiveScene().buildIndex == 3;
    }

    public bool CanExit()
    {
        return SceneManager.GetActiveScene().buildIndex != 3;
    }

    public void OnEnter()
    {
        Debug.Log("Entering Tutorial State!");
    }

    public void OnExit()
    {
        Debug.Log("Leaving Tutorial State!");
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
