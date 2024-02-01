using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_projectileSpeed = 10;

    private float m_timer = 0;
    private const float MAX_DURATION = 20;


    void Start()
    {
        m_timer = MAX_DURATION;

    }

    void Update()
    {
        if (m_timer < 0)
        {
            //NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
            return;
        }
        m_timer -= Time.deltaTime;

    }

    public void Shoot(Vector3 direction)
    {
        m_rb.AddForce(direction * m_projectileSpeed, ForceMode.Impulse);
    }
}
