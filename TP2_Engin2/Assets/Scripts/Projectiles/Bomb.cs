using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Bomb : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_projectileSpeed = 10;
    [SerializeField] private float m_explosionForce = 10;
    [SerializeField] private float m_explosionRadius = 10;
    [SerializeField] private float m_height = 20;
    [SerializeField] private float m_explosionTimer = 5;

    private float m_timer = 0;
    private bool m_stuck = false;

    void Start()
    {
        m_timer = m_explosionTimer;

    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        HandleTimer();
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    [Server]
    private void HandleTimer()
    {
        if (m_timer < 0)
        {
            //Debug.Log("explode");
            CMD_Explode();
            return;
        }
        m_timer -= Time.deltaTime;
    }

    [Command(requiresAuthority = false)]
    private void CMD_Explode()
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, m_explosionRadius);

        foreach (var obj in surroundingObjects)
        {
            // Needs to affect only characterPlayers
            // Basic implementation would be to check tag
            // if (obj.gameObject.tag != "CharacterPlayer") continue;

            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null || rb == m_rb)
            {
                continue;
            }


            rb.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius, m_height, ForceMode.Impulse);
        }

        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CMD_Shoot(Vector3 direction)
    {
        m_rb.AddForce(direction * m_projectileSpeed, ForceMode.Impulse);
    }

    //[Server]
    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            Debug.Log("not server");
            return;
        }


        if (m_stuck) return;
        if (collision.gameObject.GetComponent<Bullet>() != null ||
            collision.gameObject.GetComponent<Bomb>() != null) return;

        m_rb.velocity = Vector3.zero;

        //Vector3 prevScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        CMD_SetParent(collision.transform);

        //float x = prevScale.x / collision.transform.localScale.x;
        //float y = prevScale.y / collision.transform.localScale.y;
        //float z = prevScale.z / collision.transform.localScale.z;

        //transform.localScale = new Vector3(x, y, z);


        //CMD_SetParent(collision.transform);
        
        m_stuck = true;
    }

    [Command(requiresAuthority = false)]
    private void CMD_SetParent(Transform collidedTransform)
    {
        Debug.Log("CMD_SetParent");

        Vector3 prevScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.SetParent(collidedTransform);

        float x = prevScale.x / collidedTransform.localScale.x;
        float y = prevScale.y / collidedTransform.localScale.y;
        float z = prevScale.z / collidedTransform.localScale.z;

        transform.localScale = new Vector3(x, y, z);

        //transform.SetParent(collidedTransform);
        RPC_SetParent(collidedTransform);
    }

    [ClientRpc]
    private void RPC_SetParent(Transform collidedTransform)
    {
        Debug.Log("rpc_setParent");
        transform.SetParent(collidedTransform);
    }
}
