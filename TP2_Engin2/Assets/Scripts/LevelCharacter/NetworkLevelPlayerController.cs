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
    public void CMD_SendWorldInputs(Vector3 worldInput)
    {
        if (m_platformController == null)
        {
            Debug.Log("No platform controller");
            return;
        }

        //Debug.Log("CMD_SendWorldInputs");
        m_platformController.ReceiveWorldInputs(worldInput);
    }

    public void SetPlatformController(NetworkPlatformManager manager)
    {
        //Debug.Log("Set controller " + manager.name);
        m_platformController = manager;
    }
}
