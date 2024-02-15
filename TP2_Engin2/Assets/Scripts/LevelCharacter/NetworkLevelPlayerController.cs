using Mirror;
using UnityEngine;

public class NetworkLevelPlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkPlatformManager m_platformController;

    private void Start()
    {
        m_platformController = NetworkPlatformManager._Instance?.GetComponent<NetworkPlatformManager>();
        //Debug.Log("controller = " + m_platformController);
    }

    [Command]
    public void CMD_SendInputs(Vector3 inputs)
    {
        if (m_platformController == null)
        {
            Debug.Log("No platform controller");
            return;
        }

        if (isServer) 
        {
            //Debug.Log("ENTERED NETWORK PLAYER CONTROLLER INSIDE IF SERVER");
        }

        m_platformController.ReceivePlayersInputs(inputs);
    }

    public void SetPlatformController(NetworkPlatformManager manager)
    {
        //Debug.Log("Set controller " + manager.name);
        m_platformController = manager;
    }
}
