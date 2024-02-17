using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLaunchCinematic : NetworkBehaviour
{

    [SerializeField] private LaunchCinematic m_localCinematic;

    public void LaunchLocalCinematic()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}
        Debug.Log("in netCin");
        //m_localCinematic.Launch();
    }

    public void SetLocalLaunchCinematic(LaunchCinematic localCin)
    {
        m_localCinematic = localCin;
    }
}
