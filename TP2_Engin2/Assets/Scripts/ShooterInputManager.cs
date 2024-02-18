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

    [Command(requiresAuthority = false)]
    public void CMDHandleArrowInput(KeyCode input, NetworkConnectionToClient player = null)
    {
        int index = player.m_uiSlotIndex - 2;
        if (input == KeyCode.Alpha1)
        {
            var Shooter = ShooterUIManager.Instance;
            if (Shooter == null)
            {
                Debug.Log("singleton null");
            }
            ShooterUIManager.Instance.MovePlayerArrow(index, false);
            RPCHandleActivateInput(input, index);
        }
        else if (input == KeyCode.Alpha2)
        {
            var Shooter = ShooterUIManager.Instance;
            if (Shooter == null)
            {
                Debug.Log("singleton null");
            }

            ShooterUIManager.Instance.MovePlayerArrow(index, true);
            RPCHandleActivateInput(input, index);
        }
    }

    [ClientRpc]
    public void RPCHandleActivateInput(KeyCode input, int index)
    {
        if (input == KeyCode.Alpha1)
        {
            ShooterUIManager.Instance.MovePlayerArrow(index, false);
        }
        else if (input == KeyCode.Alpha2)
        {
            ShooterUIManager.Instance.MovePlayerArrow(index, true);
        }
    }
}
