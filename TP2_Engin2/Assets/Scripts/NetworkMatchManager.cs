using UnityEngine;
using Mirror;

public enum E_TriggerTypes
{
    OutOfVerticalMapBounds,
    Win,
    Count
}

public class NetworkMatchManager : NetworkBehaviour
{    
    [SerializeField] private float m_radius = 10.0f;
    [SerializeField] private float m_respawnHeight = 10.0f;

    [SerializeField] private float m_gameTimer = 0.0f;
    [SerializeField] private float m_maxGameTimer = 300.0f;

    [SerializeField] private float m_shootBombTimer = 0.0f;
    [SerializeField] private float m_maxShootBombTimer = 10.0f;

    public static NetworkMatchManager _Instance { get; private set; } //Nécessaire ? Déjà accessible du NetManagerCustom..

    private bool m_canShootBomb = false;

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
        if (isServer)
        {
            ServerUpdate();
        }
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
            // Call Win/Lose
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

    public int GetGameTimer()   //function to call for game timer : NetManagerCustom.Instance.MatchManager.GetGameTimer();
    {
        return (int)m_gameTimer;
    }
    
    public bool GetPermissionToShoot()  //function to call for shooting bomb availability : NetManagerCustom.Instance.MatchManager.GetPermissionToShoot();
    {
        if (m_canShootBomb)
        {
            m_shootBombTimer = m_maxShootBombTimer;
            m_canShootBomb = false;
            return true;
        }

        return false;
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
                Win(player);
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

    [ClientRpc]
    private void Win(GameObject player)
    {
        
    }

}

