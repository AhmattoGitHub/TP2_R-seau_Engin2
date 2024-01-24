using UnityEngine;
using Mirror;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_platformPrefab;
    public NetworkIdentity m_platformNetIdentity;

    public override void OnStartServer()
    {
        GameObject platformInstance = Instantiate(m_platformPrefab);

        NetworkServer.Spawn(platformInstance);

        // Peut-�tre transf�rer le netIdentity au LevelPlayer somehow
        m_platformNetIdentity = platformInstance.GetComponent<NetworkIdentity>();
    }


}
