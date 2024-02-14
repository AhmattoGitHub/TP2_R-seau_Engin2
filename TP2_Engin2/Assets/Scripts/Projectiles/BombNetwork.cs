using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;


public class BombNetwork : NetworkBehaviour
{
    [SerializeField] private float m_projectileSpeed = 100;
    [SerializeField] private float m_explosionForce = 10;
    [SerializeField] private float m_explosionRadius = 10;
    [SerializeField] private float m_explosionTimer = 5;
    [SerializeField] private float m_height = 20;
    [SerializeField] private Rigidbody m_rb;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }

        if (m_stuck)
        {
            return;
        }

        if (collision.gameObject.GetComponent<BombNetwork>() != null)
        {
            NetworkServer.Destroy(gameObject);  //EXPLODE
            return;
        }



        //Vector3 prevScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //
        //float x = prevScale.x / collision.transform.localScale.x;
        //float y = prevScale.y / collision.transform.localScale.y;
        //float z = prevScale.z / collision.transform.localScale.z;
        //
        //transform.localScale = new Vector3(x, y, z);


        m_rb.velocity = Vector3.zero;
        Debug.Log("collision name: " + collision.gameObject.name);
        int collidedGoIdx = NetManagerCustom._Instance.Identifier.GetIndex(collision.collider.gameObject);
        Debug.Log("collided object idx: " + collidedGoIdx);
        CMD_SetParent(collision.transform.root, collidedGoIdx);
        m_stuck = true;
    }


    [Server]
    private void HandleTimer()
    {
        if (m_timer < 0)
        {
            //CMD_Explode();
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

    [Command(requiresAuthority = false)]
    public void CMD_Explode()
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
    public void CMD_SetParent(Transform collidedTransformRoot, int collidedObjIdx)
    {
        transform.SetParent(collidedTransformRoot);

        var go = NetManagerCustom._Instance.Identifier.GetObjectAtIndex(collidedObjIdx);
        Debug.Log("CMD: setting parent: " + go.name);
        transform.SetParent(go.transform);

        RPC_SetParent(collidedTransformRoot, collidedObjIdx);
    }

    [ClientRpc]
    public void RPC_SetParent(Transform collidedTransformRoot, int collidedObjIdx)
    {
        gameObject.SetActive(false);

        transform.SetParent(collidedTransformRoot);

        var go = NetManagerCustom._Instance.Identifier.GetObjectAtIndex(collidedObjIdx);
        Debug.Log("RPC: setting parent: " + go.name);
        transform.SetParent(go.transform);
        gameObject.SetActive(true);

    }
}
