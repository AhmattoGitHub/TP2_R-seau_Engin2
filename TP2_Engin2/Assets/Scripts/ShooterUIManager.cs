using JetBrains.Annotations;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShooterUIManager : NetworkBehaviour
{
    //private static ShooterUIManager _instance;

    [SerializeField]
    private Image[] m_playerArrows;
    [SerializeField]
    private Image[] m_platformArrows;

    private Vector3 m_arrowOneInitialPosition = new Vector3();

    private Vector3 m_arrowTwoInitialPosition = new Vector3();
    [SerializeField]
    private GameObject m_shooterCanvas;

    // Property to access the singleton instance
    //public static ShooterUIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            Debug.LogError("There is no Instance");
    //        }
    //        return _instance;
    //    }
    //}

    //private void Awake()
    //{
    //    if (_instance != null && _instance != this)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        _instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    private void Start()
    {
        m_arrowOneInitialPosition = m_playerArrows[0].gameObject.transform.localPosition;
        m_arrowTwoInitialPosition = m_playerArrows[1].gameObject.transform.localPosition;
    }

    private void Update()
    {
        //CheckForInput();
    }

    //private void CheckForInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        ShooterInputManager.Instance.CMDHandleArrowInput(KeyCode.Alpha1);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        ShooterInputManager.Instance.CMDHandleArrowInput(KeyCode.Alpha2);
    //    }
    //}
    public void MovePlayerArrow(int index, bool selectingBomb)
    {
        if (selectingBomb)
        {
            m_playerArrows[index].transform.localPosition = new Vector3(17.5f, 111, 0);
            CMDMovePlayerArrow(index, selectingBomb);
        }
        else
        {
            if (index == 0)
            {
                m_playerArrows[index].transform.localPosition = m_arrowOneInitialPosition;
            }
            else
            {
                m_playerArrows[index].transform.localPosition = m_arrowTwoInitialPosition;
            }
            CMDMovePlayerArrow(index, selectingBomb);
        }
    }

    [Command(requiresAuthority = false)]
    private void CMDMovePlayerArrow(int index, bool selectingBomb)
    {
        if (selectingBomb)
        {
            m_playerArrows[index].transform.localPosition = new Vector3(17.5f, 111, 0);
            //RPCMovePlayerArrow(index, selectingBomb);
        }
        else
        {
            if (index == 0)
            {
                m_playerArrows[index].transform.localPosition = m_arrowOneInitialPosition;
            }
            else
            {
                m_playerArrows[index].transform.localPosition = m_arrowTwoInitialPosition;
            }
            //RPCMovePlayerArrow(index, selectingBomb);
        }
    }

    //[ClientRpc]
    public void RPCMovePlayerArrow(int index, bool selectingBomb)
    {
        Debug.Log("in rpc");
        if (selectingBomb)
        {
            m_playerArrows[index].transform.localPosition = new Vector3(17.5f, 111, 0);
        }
        else
        {
            if (index == 0)
            {
                m_playerArrows[index].transform.localPosition = m_arrowOneInitialPosition;
            }
            else
            {
                m_playerArrows[index].transform.localPosition = m_arrowTwoInitialPosition;
            }
        }
    }
}