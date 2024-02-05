using UnityEngine;
using Mirror;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_platformPrefab;
    [SerializeField] private GameObject m_dummyPrefab;
    [SerializeField] private GameObject m_movingObjPrefab;
    [SerializeField] private Vector3 m_dummySpawnPos;
    [SerializeField] private Vector3 m_movingObjSpawnPos;


    public override void OnStartServer()
    {
        GameObject platformInstance = Instantiate(m_platformPrefab);
        NetworkServer.Spawn(platformInstance);
        
        GameObject dummyInstance = Instantiate(m_dummyPrefab, m_dummySpawnPos, Quaternion.identity);
        NetworkServer.Spawn(dummyInstance);
        
        GameObject movingObj = Instantiate(m_movingObjPrefab, m_movingObjSpawnPos, Quaternion.identity);
        NetworkServer.Spawn(movingObj);
    }
}
