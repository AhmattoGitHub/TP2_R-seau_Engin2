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

    public static NetworkMatchManager _Instance { get; private set; }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        _Instance = this;
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
        
    }
    
    [Command (requiresAuthority = false)]
    public void CMD_SendPlayerAndTrigger(GameObject player, E_TriggerTypes triggerType)
    {
        if (isServer) 
        {
            BoundsTriggeredByPlayer(player, triggerType);
        }
        else 
        {
            BoundsTriggeredByPlayer(player, triggerType);
        }        
    }
    
    [Server]
    private void BoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
    {
        Debug.Log("We entered the 111111111111111111111111!!!");

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

