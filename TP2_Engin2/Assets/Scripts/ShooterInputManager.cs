using Mirror;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterInputManager : NetworkBehaviour
{
    private static ShooterInputManager _instance;

    // Property to access the singleton instance
    public static ShooterInputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no ShooterInputManager Instance");
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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [Command(requiresAuthority =false)]
    public void CMDHandleActivateInput(KeyCode input)
    {
        if(input  == KeyCode.W) 
        {
            ShooterUIManager.Instance.ActivateArrow(0);
            RPCHandleActivateInput(input);
        }
        if (input == KeyCode.D)
        {
            ShooterUIManager.Instance.ActivateArrow(1);
            RPCHandleActivateInput(input);
        }
        if (input == KeyCode.S)
        {
            ShooterUIManager.Instance.ActivateArrow(2);
            RPCHandleActivateInput(input);
        }
        if (input == KeyCode.A)
        {
            ShooterUIManager.Instance.ActivateArrow(3);
            RPCHandleActivateInput(input);
        }
    }

    [ClientRpc]
    public void RPCHandleActivateInput(KeyCode input)
    {
        if (input == KeyCode.W)
        {
            ShooterUIManager.Instance.ActivateArrow(0);
        }
        if (input == KeyCode.D)
        {
            ShooterUIManager.Instance.ActivateArrow(1);
        }
        if (input == KeyCode.S)
        {
            ShooterUIManager.Instance.ActivateArrow(2);
        }
        if (input == KeyCode.A)
        {
            ShooterUIManager.Instance.ActivateArrow(3);
        }
    }

    [Command(requiresAuthority =false)]
    public void CMDHandleDeactivateInput(KeyCode input)
    {
        if (input == KeyCode.W)
        {
            ShooterUIManager.Instance.DeactivateArrow(0);
            RPCHandleDeactivateInput(input);
        }
        if (input == KeyCode.D)
        {
            ShooterUIManager.Instance.DeactivateArrow(1);
            RPCHandleDeactivateInput(input);
        }
        if (input == KeyCode.S)
        {
            ShooterUIManager.Instance.DeactivateArrow(2);
            RPCHandleDeactivateInput(input);
        }
        if (input == KeyCode.A)
        {
            ShooterUIManager.Instance.DeactivateArrow(3);
            RPCHandleDeactivateInput(input);
        }
    }

    [ClientRpc]
    public void RPCHandleDeactivateInput(KeyCode input)
    {
        if (input == KeyCode.W)
        {
            ShooterUIManager.Instance.DeactivateArrow(0);
        }
        if (input == KeyCode.D)
        {
            ShooterUIManager.Instance.DeactivateArrow(1);
        }
        if (input == KeyCode.S)
        {
            ShooterUIManager.Instance.DeactivateArrow(2);
        }
        if (input == KeyCode.A)
        {
            ShooterUIManager.Instance.DeactivateArrow(3);
        }
    }
}
