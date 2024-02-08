using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocalPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject m_localPlayerPrefab;
    [SerializeField] private bool isRunner;

    [field: Header("RUNNERS")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rb;

    [field: Header("SHOOTERS")]
    [SerializeField] private NetworkLevelPlayerController m_controller;
    [SerializeField] private Shooter m_shooter;



    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (isRunner)
        {
            var go = Instantiate(m_localPlayerPrefab, transform);
            
            var childSM = go.GetComponentInChildren<RunnerSM>();
            childSM.SetAnimator(m_animator);
            childSM.SetRigidbody(m_rb);
            childSM.SetParentGo(gameObject);
        }
        else
        {
            var go = Instantiate(m_localPlayerPrefab, transform);
            
            var childController = go.GetComponentInChildren<LocalLevelPlayerController>();
            childController.SetNetworkController(m_controller);
            childController.SetParentGo(gameObject);

            var camera = go.GetComponentInChildren<Camera>();
            m_shooter.SetCamera(camera);
        }
    }
}
