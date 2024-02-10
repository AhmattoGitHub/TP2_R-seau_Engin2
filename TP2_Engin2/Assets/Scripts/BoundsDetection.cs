using UnityEngine;



public class BoundsDetection : MonoBehaviour
{
    [field: SerializeField]
    public E_TriggerTypes TriggerType { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        var runnerSM = other.transform.root.gameObject.GetComponentInChildren<RunnerSM>();

        if (runnerSM != null)
        {
            GameObject player = other.gameObject.transform.root.gameObject;

            NetworkMatchManager._Instance.CMD_BoundsTriggeredByPlayer(player, TriggerType);
        }
       
    }
 

}










//private void BoundsTriggered(Collider other)
//{
//    var runnerSM = other.transform.root.gameObject.GetComponentInChildren<RunnerSM>();
//
//    if (runnerSM != null)
//    {
//        GameObject player = other.gameObject.transform.root.gameObject;
//
//        NetworkMatchManager._Instance.CMD_BoundsTriggeredByPlayer(player, triggerType);
//    }
//}

//private void BoundsTriggered(Collider other, E_TriggerTypes triggerType)
//{
//    var runnerSM = other.transform.root.gameObject.GetComponentInChildren<RunnerSM>();
//
//    if (runnerSM != null)
//    {
//        GameObject player = other.gameObject.transform.root.gameObject;
//
//        NetworkMatchManager._Instance.CMD_BoundsTriggeredByPlayer(player, triggerType);
//    }
//}   










//[Command(requiresAuthority = false)]
//public void CMD_SendBoundsTriggeredByPlayer(GameObject player, E_TriggerTypes triggerType)
//{
//    Debug.Log("ENNNNTTRRREEEEESSSS");
//
//    //NetworkMatchManager._Instance.ReceiveOutOfBoundsPlayerId(playerId);
//
//
//    NetworkMatchManager._Instance.BoundsTriggeredByPlayer(player, triggerType);
//}





//[Command(requiresAuthority = false)]
//public void CMD_SendOutOfBoundsPlayerId(NetworkIdentity playerId)
//{
//    Debug.Log("ENNNNTTRRREEEEESSSS");
//
//    NetworkMatchManager._Instance.ReceiveOutOfBoundsPlayerId(playerId);
//
//}


