using UnityEngine;
using Mirror;


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

    [Server]
    public void ReceiveOutOfBoundsPlayerId(NetworkIdentity playerId)
    {
        Debug.Log("Received 1111111111" + playerId);

        MovePlayerThatWasOutOfBounds(playerId);
    }

    [ClientRpc]
    private void MovePlayerThatWasOutOfBounds(NetworkIdentity playerId)
    {
        Debug.Log("Received 222222222222" + playerId);
        
        Vector2 randomPosOnCircle = RandomPosOnCircle();        

        Vector3 randomPosition = new Vector3(randomPosOnCircle.x, 10, randomPosOnCircle.y);
                
        playerId.transform.position = randomPosition;
    }

    

    private Vector2 RandomPosOnCircle()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        float x = m_radius * Mathf.Cos(randomAngle);
        float y = m_radius * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }


    


}














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


