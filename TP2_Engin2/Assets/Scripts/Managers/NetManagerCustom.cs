using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetManagerCustom : NetworkManager
{
    public static NetManagerCustom _Instance { get; private set; }
    [field:SerializeField] public Identifier Identifier { get; private set; }
    [field:SerializeField] public NetworkMatchManager MatchManager { get; private set; }


    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private GameObject runnerPrefab;
    [SerializeField] private List<Vector3> runnerSpawns;
    private int runnerSpawnsIndex = 0;
    [SerializeField] private GameObject m_levelPrefab;
    [SerializeField] private bool spawnRunner = true;
    [SerializeField] private bool testing = false;

    [SerializeField] private GameObject m_spawner;



    public override void Awake()
    {
        base.Awake();

        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        _Instance = this;
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        if (testing)
        {

            
            
            if (spawnRunner)
            {

                var player = Instantiate(runnerPrefab);

                player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
                NetworkServer.AddPlayerForConnection(conn, player);

            }
            else
            {
                var player = Instantiate(shooterPrefab, shooterPrefab.transform.position, shooterPrefab.transform.rotation);

                player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
                NetworkServer.AddPlayerForConnection(conn, player);


                //OnServerAddPlayer(conn, shooterPrefab);
            }


            //if (conn.identity.isLocalPlayer)
            //{
            //    var spawner = Instantiate(m_spawner);
            //    NetworkServer.Spawn(spawner);
            //
            //    m_identifier = spawner.GetComponent<Identifier>();
            //}


            return;
        }

        if (SceneManager.GetActiveScene().name != "MainLevel")
        {
            return;
        }

        Debug.Log("trying to change :  " + conn.m_name);
        if (conn.m_tag == "Runner")
        {
            NetworkServer.ReplacePlayerForConnection(conn, Instantiate(runnerPrefab, runnerSpawns[runnerSpawnsIndex], Quaternion.identity), true);
            runnerSpawnsIndex++;
            conn.m_isInMainLevel = true;

        }
        else
        {
            NetworkServer.ReplacePlayerForConnection(conn, Instantiate(shooterPrefab), true);
            conn.m_isInMainLevel = true;

        }

        int mainLevelCounter = 0;
        foreach (var player in LobbyManager.Instance.GetList())
        {
            Debug.Log(player.m_name + "  " + player.m_isInMainLevel);
            if (player.m_isInMainLevel)
            {
                Debug.Log("mainLevelCounter++");
                mainLevelCounter++;
            }
            else
            {
                Debug.Log("not all ready");
                return;
            }
        }

        if (mainLevelCounter == LobbyManager.Instance.GetList().Count)
        {
            //All players ready in scene
            MatchManager.SetConnectedPlayersList(LobbyManager.Instance.GetList());
            MatchManager.LaunchGame();
            
            NetworkServer.Destroy(LobbyManager.Instance.gameObject);
        }
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        if (SceneManager.GetActiveScene().name != "MainLevel")
        {
            return;
        }

        var spawner = m_spawner.GetComponent<NetworkSpawner>();
        if (spawner != null)
        {
            spawner.Spawn();
        }
    }

    public override void OnValidate()
    {
        base.OnValidate();
        
        if (shooterPrefab != null && !shooterPrefab.TryGetComponent(out NetworkIdentity _))
        {
            Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
            shooterPrefab = null;
        }
        if (runnerPrefab != null && !runnerPrefab.TryGetComponent(out NetworkIdentity _))
        {
            Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
            runnerPrefab = null;
        }
        if (m_levelPrefab != null && !m_levelPrefab.TryGetComponent(out NetworkIdentity _))
        {
            Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
            m_levelPrefab = null;
        }
        
        if (shooterPrefab != null && spawnPrefabs.Contains(shooterPrefab))
        {
            Debug.LogWarning("NetworkManager - Player Prefab doesn't need to be in Spawnable Prefabs list too. Removing it.");
            spawnPrefabs.Remove(shooterPrefab);
        }
        if (runnerPrefab != null && spawnPrefabs.Contains(runnerPrefab))
        {
            Debug.LogWarning("NetworkManager - Player Prefab doesn't need to be in Spawnable Prefabs list too. Removing it.");
            spawnPrefabs.Remove(runnerPrefab);
        }
        if (m_levelPrefab != null && spawnPrefabs.Contains(m_levelPrefab))
        {
            Debug.LogWarning("NetworkManager - Player Prefab doesn't need to be in Spawnable Prefabs list too. Removing it.");
            spawnPrefabs.Remove(m_levelPrefab);
        }
    }

    GameObject SpawnLevel(SpawnMessage msg)
    {
        var level = Instantiate(m_levelPrefab, m_spawner.transform);
        Identifier.AssignAllIds(m_spawner.transform);

        return level;
    }

    public void UnSpawnLevel(GameObject spawned)
    {
        Destroy(spawned);
    }

    public override void RegisterClientMessages()
    {
        base.RegisterClientMessages();

        if (shooterPrefab != null)
            NetworkClient.RegisterPrefab(shooterPrefab);
        if (runnerPrefab != null)
            NetworkClient.RegisterPrefab(runnerPrefab);
        if (m_levelPrefab != null)
        {
            NetworkClient.RegisterPrefab(m_levelPrefab, SpawnLevel, UnSpawnLevel);
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        if (testing)
        {
            return;
        }
        LobbyManager.Instance.WaitForConfig();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //Debug.Log("OnserverAddPlayer");
        base.OnServerAddPlayer(conn);
        conn.identity.name = "";
        LobbyManager.Instance.AddToConnections(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (!testing)
        {
            LobbyManager.Instance.RemoveFromConnections(conn);
        }
        base.OnServerDisconnect(conn);
    }

}
