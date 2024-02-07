using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject m_platformPrefab;
    [SerializeField] private GameObject m_dummyPrefab;
    [SerializeField] private Vector3 m_dummyPrefabPosition;

    public override void OnStartServer()
    {
        GameObject platformInstance = Instantiate(m_platformPrefab);
        NetworkServer.Spawn(platformInstance);
        
        GameObject dummyInstance = Instantiate(m_dummyPrefab, m_dummyPrefabPosition, Quaternion.identity);
        NetworkServer.Spawn(dummyInstance);
    }

}
