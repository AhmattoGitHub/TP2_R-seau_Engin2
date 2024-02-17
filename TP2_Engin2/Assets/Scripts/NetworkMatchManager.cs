using UnityEngine;
using Mirror;
using System.Collections.Generic;

public enum E_TriggerTypes
{
    OutOfVerticalMapBounds,
    Win,
    Count
}

public class NetworkMatchManager : NetworkBehaviour
{    
    public static NetworkMatchManager _Instance { get; private set; } //Nécessaire ? Déjà accessible du NetManagerCustom..

    private List<NetworkConnectionToClient> ConnectedPlayers = new List<NetworkConnectionToClient>();

    [SerializeField] private float m_radius = 10.0f;
    [SerializeField] private float m_respawnHeight = 10.0f;

    [SyncVar] private float m_gameTimer = 0.0f;
    [SerializeField] private float m_maxGameTimer = 300.0f;

    [SyncVar] private float m_shootBombTimer = 0.0f;
    [SerializeField] private float m_maxShootBombTimer = 5.0f;

    [SyncVar] private bool m_canShootBomb = false;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        _Instance = this;


    }

    private void Start()
    {
        m_gameTimer = m_maxGameTimer;
        m_shootBombTimer = m_maxShootBombTimer;
    }

    private void Update()
    {
        //Debug.Log("timer" + m_gameTimer);
        
        if (isServer)
        {
            ServerUpdate();
        }
        //ServerUpdate();
    }

    private void ServerUpdate()
    {
        HandleGameTimer();
        HandleShootBombTimer();
    }

    private void HandleGameTimer()
    {
        if (m_gameTimer < 0)
        {
            ShooterWin();
            return;
        }
        m_gameTimer -= Time.deltaTime;
    }

    private void HandleShootBombTimer()
    {
        if (m_shootBombTimer < 0)
        {
            m_canShootBomb = true;
            return;
        }
        m_shootBombTimer -= Time.deltaTime;
    }

    public void LaunchGame()
    {
        foreach (var player in ConnectedPlayers)
        {
            var cinematic = player.identity.gameObject.GetComponentInChildren<LaunchCinematic>();
            //Debug.Log(player.m_name + " launched cinematic " + cinematic);
            cinematic.RPC_Launch();
        }
    }

    public void SetConnectedPlayersList(List<NetworkConnectionToClient> list)
    {
        ConnectedPlayers = list;
    }

    public int GetGameTimer()   //function to call for game timer : NetManagerCustom.Instance.MatchManager.GetGameTimer();
    {
        return (int)m_gameTimer;
    }

    public bool GetBombAvailability()   //function to call for shooting bomb availability : NetManagerCustom.Instance.MatchManager.GetBombAvailability();
    {
        return m_canShootBomb;
    }
    
    public float GetBombRemainingPercentage()   //function to call for shooting bomb timer (between 0 & 1) : NetManagerCustom.Instance.MatchManager.GetBombRemainingPercentage();
    {
        if (m_shootBombTimer < 0)
        {
            return 0.0f;
        }
        return m_shootBombTimer / m_maxShootBombTimer;
    }

    public float GetLocalPlayerBulletRemainingPercentage() //function to call for shooting bullet timer (between 0 & 1) : NetManagerCustom.Instance.MatchManager.GetLocalPlayerBulletRemainingPercentage();
    {
        foreach (var player in ConnectedPlayers)
        {
            if (!player.identity.isLocalPlayer)
            {
                continue;
            }
            var shooterScript = player.identity.gameObject.GetComponentInChildren<Shooter>();
            if (shooterScript == null)
            {
                Debug.LogError("Player not found");
                return -1.0f;
            }
            return shooterScript.GetBulletRemainingPercentage();
        }

        Debug.LogError("Player not found");
        return -1.0f;
    }

    public bool GetPermissionToShoot()  
    {
        if (m_canShootBomb)
        {
            CMD_ResetShootBombTimer();
            return true;
        }

        return false;
    }

    [Command(requiresAuthority = false)]
    private void CMD_ResetShootBombTimer()
    {
        m_shootBombTimer = m_maxShootBombTimer;
        m_canShootBomb = false;
    }

    [Command (requiresAuthority = false)]
    public void CMD_SendPlayerAndTrigger(GameObject player, E_TriggerTypes triggerType)
    {
        //Debug.Log("in cmd");

        BoundsTriggeredByPlayer(player, triggerType);
    }
    
    [Server]
    private void BoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
    {
        //Debug.Log("We entered the 111111111111111111111111!!!");

        switch (triggerType)
        {
            case E_TriggerTypes.OutOfVerticalMapBounds:
                RespawnPlayerRandomCircle(player);
                break;
            case E_TriggerTypes.Win:
                RunnerWin(player);
                break;
            default:
                break;
        }
    }
    
    [ClientRpc]
    private void RespawnPlayerRandomCircle(GameObject player)
    {
        Vector2 randomPosOnCircle = RandomPosOnCircle();
        Vector3 randomPosition = new Vector3(randomPosOnCircle.x, m_respawnHeight, randomPosOnCircle.y);

        player.transform.position = randomPosition;
        player.transform.LookAt(new Vector3(0,m_respawnHeight, 0));
    }    

    private Vector2 RandomPosOnCircle()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        float x = m_radius * Mathf.Cos(randomAngle);
        float y = m_radius * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }

    [ClientRpc] //?
    private void RunnerWin(GameObject player)
    {
        foreach (var connPlayer in ConnectedPlayers)
        {
            if (connPlayer.m_tag == "Runner")
            {
                //var manager = connPlayer.identity.gameObject.GetComponent<UIManager>();
                // manager.EnableVictoryScreen()
                continue;
            }

            //logique de défaite des shooters


        }
    }

    [ClientRpc] //?
    private void ShooterWin()
    {
        foreach (var connPlayer in ConnectedPlayers)
        {
            if (connPlayer.m_tag == "Shooter")
            {
                //var manager = connPlayer.identity.gameObject.GetComponent<UIManager>();
                // manager.EnableVictoryScreen()
                continue;
            }

            //logique de défaite des runners
        }
    }

}

