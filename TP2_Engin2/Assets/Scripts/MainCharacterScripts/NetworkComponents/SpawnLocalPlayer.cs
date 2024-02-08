using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocalPlayer : NetworkBehaviour
{
    [SerializeField] GameObject m_localPlayerPrefab;
    [SerializeField] Animator m_animator;
    [SerializeField] Rigidbody m_rb;
    
    private void Start()
    {    
        if (isLocalPlayer)
        {
            var go = Instantiate(m_localPlayerPrefab, transform);
            var childSM = go.GetComponentInChildren<RunnerSM>();

            childSM.SetAnimator(m_animator);
            childSM.SetRigidbody(m_rb);
            childSM.SetCharacterGo(gameObject);
        }
    }
}
