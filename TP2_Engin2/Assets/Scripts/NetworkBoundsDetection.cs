using UnityEngine;
using Mirror;

public enum E_TriggerTypes
{
    OutOfBounds,
    Win,
    Count
}

public class NetworkBoundsDetection : NetworkBehaviour
{
    

    [field: SerializeField]
    public E_TriggerTypes TriggerType { get; set; }

    


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered vertical bounds!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");


        switch (TriggerType)
        {
            case E_TriggerTypes.OutOfBounds:
                OutOfBoundsTrigger(other);
                break;
            case E_TriggerTypes.Win:
                break;            
            default:
                break;
        }


        
    }

    

    private void OutOfBoundsTrigger(Collider other)
    {
        var gameObject = other.transform.root.gameObject.GetComponentInChildren<RunnerSM>();

        if (gameObject != null)
        {
            //Debug.Log("gameObject " + other.gameObject.name);
            //Debug.Log("netId " + networkIdentity.gameObject.name);

            Debug.Log("Hello");

            NetworkIdentity networkIdentity = other.transform.root.GetComponent<NetworkIdentity>();

            CMD_SendOutOfBoundsPlayerId(networkIdentity);



        }
    }


    [Command(requiresAuthority = false)]
    public void CMD_SendOutOfBoundsPlayerId(NetworkIdentity playerId)
    {
        Debug.Log("ENNNNTTRRREEEEESSSS");

        NetworkMatchManager._Instance.ReceiveOutOfBoundsPlayerId(playerId);

    }

}









