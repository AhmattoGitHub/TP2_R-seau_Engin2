using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSpawner : NetworkBehaviour
{
    
    [SerializeField] private bool m_spawnPlatform = false;
    [SerializeField] private GameObject m_platformPrefab;
    [SerializeField] private bool m_spawnDummy = false;
    [SerializeField] private GameObject m_dummyPrefab;
    [SerializeField] private Vector3 m_dummyPrefabPosition;
    [SerializeField] private bool m_spawnCubes = false;
    [SerializeField] private GameObject m_cubesPrefab;

    [SerializeField] private Identifier m_identifier;


    //Works for testing

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //
    //    if (m_spawnPlatform)
    //    {
    //        GameObject platformInstance = Instantiate(m_platformPrefab, transform);
    //        NetworkServer.Spawn(platformInstance);
    //
    //    }
    //    if (m_spawnDummy)
    //    {
    //        GameObject dummyInstance = Instantiate(m_dummyPrefab, m_dummyPrefabPosition, Quaternion.identity, transform);
    //        NetworkServer.Spawn(dummyInstance);
    //
    //    }
    //
    //    if (m_spawnCubes)
    //    {
    //        GameObject cubesInstance = Instantiate(m_cubesPrefab, transform);
    //        NetworkServer.Spawn(cubesInstance);
    //    }
    //
    //
    //    m_identifier.AssignAllIds(transform);
    //
    //}

    public void Spawn()
    {
        if (m_spawnPlatform)
        {
            GameObject platformInstance = Instantiate(m_platformPrefab, transform);
            NetworkServer.Spawn(platformInstance);

        }
        if (m_spawnDummy)
        {
            GameObject dummyInstance = Instantiate(m_dummyPrefab, m_dummyPrefabPosition, Quaternion.identity, transform);
            NetworkServer.Spawn(dummyInstance);

        }

        if (m_spawnCubes)
        {
            GameObject cubesInstance = Instantiate(m_cubesPrefab, transform);
            NetworkServer.Spawn(cubesInstance);
        }

        m_identifier.AssignAllIds(transform);
    }
}
