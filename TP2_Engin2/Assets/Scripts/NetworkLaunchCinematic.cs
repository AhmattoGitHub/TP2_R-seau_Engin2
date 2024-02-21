using Mirror;
using UnityEngine;

public class NetworkLaunchCinematic : NetworkBehaviour
{

    [SerializeField] private LaunchCinematic m_localCinematic;


    public void SetLocalLaunchCinematic(LaunchCinematic localCin)
    {
        m_localCinematic = localCin;
    }
}
