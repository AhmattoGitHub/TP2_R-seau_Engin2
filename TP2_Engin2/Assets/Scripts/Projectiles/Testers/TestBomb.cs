using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBomb : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_projectileSpeed = 10;
    [SerializeField] private float m_explosionForce = 10;
    [SerializeField] private float m_explosionRadius = 10;
    [SerializeField] private float m_height = 20;


    private float m_timer = 0;
    private const float EXPLOSION_TIMER = 5;


    void Start()
    {
        m_timer = EXPLOSION_TIMER;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer < 0)
        {
            //NetworkServer.Destroy(gameObject);
            Debug.Log("explode");
            Explode();
            return;
        }
        m_timer -= Time.deltaTime;

    }

    private void Explode()
    {
        //m_rb.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius);

        var surroundingObjects = Physics.OverlapSphere(transform.position, m_explosionRadius);

        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null || rb == m_rb)
            {
                continue;
            }


            rb.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius, m_height, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

    public void Shoot(Vector3 direction)
    {
        m_rb.AddForce(direction * m_projectileSpeed, ForceMode.Impulse);
    }

}
