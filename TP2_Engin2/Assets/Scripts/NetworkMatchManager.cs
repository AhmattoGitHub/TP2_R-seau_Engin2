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
    public void CMD_BoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
    {
        if (isServer) 
        {
            BoundsTriggeredByPlayerOnServer(player, triggerType);
        }
        else 
        {
            BoundsTriggeredByPlayerOnServer(player, triggerType);
        }        
    }
    
    [Server]
    private void BoundsTriggeredByPlayerOnServer(GameObject player, E_TriggerTypes triggerType)
    {
        Debug.Log("We entered the 111111111111111111111111!!!");

        switch (triggerType)
        {
            case E_TriggerTypes.OutOfVerticalMapBounds:
                RespawnPlayerAfterOutOfMapVerticalBounds(player);
                break;
            case E_TriggerTypes.Win:
                Win(player);
                break;
            default:
                break;
        }
    }
    
    [ClientRpc]
    private void RespawnPlayerAfterOutOfMapVerticalBounds(GameObject player)
    {
        Vector2 randomPosOnCircle = RandomPosOnCircle();
        Vector3 randomPosition = new Vector3(randomPosOnCircle.x, 10, randomPosOnCircle.y);

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

































//public void BoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
//{
//    //Debug.Log("Entered BoundsTriggeredByPlayer in NetworkMatchManager");
//    //Debug.Log("The player is " + player);
//
//    //if (isServer)
//    //{
//    //    CMD_BoundsTriggeredByPlayerOnServer(player, triggerType);
//    //    //BoundsTriggeredByPlayerOnServer(player, triggerType);
//    //}
//    //else
//    //{
//    //    Debug.Log("The player is here now22222222222222222 " + player);
//    //
//    //    // If not running on the server, send a command to call the server method
//    //    CMD_BoundsTriggeredByPlayerOnServer(player, triggerType);
//    //}
//
//    //CMD_BoundsTriggeredByPlayerOnServer(player, triggerType);
//
//}




//public void BoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
//{
//    
//    Debug.Log("Entered BoundsTrigerredByPlayer in NetworkMatchManager");
//    Debug.Log("The player is " + player);
//
//    NetworkIdentity playerNetId = player.GetComponent<NetworkIdentity>();
//
//    switch (triggerType)
//    {
//        case E_TriggerTypes.OutOfVerticalMapBounds:
//            RespawnPlayerAfterOutOfMapVerticalBounds(playerNetId);
//            break;
//        case E_TriggerTypes.Win:
//            break;
//        default:
//            break;
//    }
//    
//    
//}
//
//[ClientRpc]
//private void RespawnPlayerAfterOutOfMapVerticalBounds(NetworkIdentity playerId)
//{
//    //Debug.Log("Received 222222222222" + playerId);
//
//    Vector2 randomPosOnCircle = RandomPosOnCircle();
//
//    Vector3 randomPosition = new Vector3(randomPosOnCircle.x, 10, randomPosOnCircle.y);
//
//    playerId.transform.position = randomPosition;
//}




//[Server]
//public void ReceiveOutOfBoundsPlayerId(NetworkIdentity playerId)
//{
//    Debug.Log("Received 1111111111" + playerId);
//
//    MovePlayerThatWasOutOfBounds(playerId);
//}

//[ClientRpc]
//private void MovePlayerThatWasOutOfBounds(NetworkIdentity playerId)
//{
//    Debug.Log("Received 222222222222" + playerId);
//    
//    Vector2 randomPosOnCircle = RandomPosOnCircle();        
//
//    Vector3 randomPosition = new Vector3(randomPosOnCircle.x, 10, randomPosOnCircle.y);
//            
//    playerId.transform.position = randomPosition;
//}















//private Vector2 RandomPosBetweenTwoCircles(float minRadius, float maxRadius)
//{
//    float randomRadius = Random.Range(minRadius, maxRadius);
//
//    //randomRadius = 100.0f;
//
//    float randomAngle = Random.Range(0f, Mathf.PI * 2f);
//
//    float randomX = randomRadius * Mathf.Cos(randomAngle);
//    float randomY = randomRadius * Mathf.Sin(randomAngle);
//
//    return new Vector2(randomX, randomY);
//}
//


