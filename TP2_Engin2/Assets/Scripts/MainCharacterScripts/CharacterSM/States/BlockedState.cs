using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedState : CharacterState
{


    public override void OnEnter()
    {
        Debug.Log("Entered BlockedState");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
        Debug.Log("Exited BlockedState");
    }

    public override bool CanEnter(GM_IState currentState)
    {
        return true;
    }
    public override bool CanExit()
    {
        return true;
    }
}
