using UnityEngine;

public class TriggerForPlayer : MonoBehaviour
{
    [field: SerializeField] public E_TriggerTypes TriggerType { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        var runnerSM = other.transform.root.gameObject.GetComponentInChildren<RunnerSM>();

        if (runnerSM != null)
        {
            GameObject player = other.gameObject.transform.root.gameObject;

            NetworkMatchManager._Instance.CMD_SendPlayerAndTrigger(player, TriggerType);
        }       
    }
}
