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
    private const float EXPLOSION_TIMER = 10;


    private bool m_stuck = false;

    void Start()
    {
        m_timer = EXPLOSION_TIMER;
    }

    void Update()
    {
        if (m_timer < 0)
        {
            //Debug.Log("explode");
            Explode();
            //Destroy(gameObject);
            return;
        }
        m_timer -= Time.deltaTime;

        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Explode()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        //Check that its not another bomb or a bullet
        
        if (m_stuck)
        {
            Debug.Log("already stuck!");
            return;
        }
        
        m_rb.velocity = Vector3.zero;

        Vector3 prevScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.SetParent(collision.transform);
        
        float x = prevScale.x / collision.transform.localScale.x;
        float y = prevScale.y / collision.transform.localScale.y;
        float z = prevScale.z / collision.transform.localScale.z;
        
        transform.localScale = new Vector3(x,y,z);
        m_stuck = true;
    }
}
