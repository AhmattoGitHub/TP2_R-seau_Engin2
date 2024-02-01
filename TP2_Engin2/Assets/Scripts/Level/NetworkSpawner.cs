using UnityEngine;
using Mirror;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_platformPrefab;
    [SerializeField] private GameObject m_dummyPrefab;
    [SerializeField] private Vector3 m_dummySpawnPos;


    public override void OnStartServer()
    {
        GameObject platformInstance = Instantiate(m_platformPrefab);
        NetworkServer.Spawn(platformInstance);
        
        GameObject dummyInstance = Instantiate(m_dummyPrefab, m_dummySpawnPos, Quaternion.identity);
        NetworkServer.Spawn(dummyInstance);
    }
}
