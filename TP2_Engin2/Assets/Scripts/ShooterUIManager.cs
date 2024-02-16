using JetBrains.Annotations;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShooterUIManager : NetworkBehaviour
{
    private static ShooterUIManager _instance;

    [SerializeField]
    private Image[] m_playerArrows;
    [SerializeField]
    private Image[] m_platformArrows;

    // Property to access the singleton instance
    public static ShooterUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no Instance");
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

    private void Update()
    {
        //CheckForPlatformKey();
        //CheckForPlatformKeyUp();
    }

    private void CheckForPlatformKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ShooterInputManager.Instance.CMDHandleActivateInput(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ShooterInputManager.Instance.CMDHandleActivateInput(KeyCode.D);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ShooterInputManager.Instance.CMDHandleActivateInput(KeyCode.S);
        }
        if (Input.GetKey(KeyCode.A))
        {
            ShooterInputManager.Instance.CMDHandleActivateInput(KeyCode.A);
        }
    }

    private void CheckForPlatformKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            ShooterInputManager.Instance.CMDHandleDeactivateInput(KeyCode.W);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            ShooterInputManager.Instance.CMDHandleDeactivateInput(KeyCode.D);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            ShooterInputManager.Instance.CMDHandleDeactivateInput(KeyCode.S);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            ShooterInputManager.Instance.CMDHandleDeactivateInput(KeyCode.A);
        }
    }

    public void ActivateArrow(int index)
    {
        m_platformArrows[index].color = Color.green;
    }

    public void DeactivateArrow(int index)
    {
        m_platformArrows[index].color = Color.white;
    }
}
