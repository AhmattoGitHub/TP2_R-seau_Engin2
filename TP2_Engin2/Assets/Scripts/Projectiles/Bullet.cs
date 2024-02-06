using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_projectileSpeed = 10;
    [SerializeField] private float m_timerDuration = 10;

    private float m_timer = 0;
    //private const float MAX_DURATION = 20;



    void Start()
    {
        m_timer = m_timerDuration;

    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }
        HandleTimer();
    }

    [Server]
    private void HandleTimer()
    {
        if (m_timer < 0)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }
        m_timer -= Time.deltaTime;
    }

    [Command(requiresAuthority = false)]
    public void CMD_Shoot(Vector3 direction)
    {
        m_rb.AddForce(direction * m_projectileSpeed, ForceMode.Impulse);
    }
}
