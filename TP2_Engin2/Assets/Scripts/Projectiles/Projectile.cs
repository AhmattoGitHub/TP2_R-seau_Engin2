using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_projectileSpeed = 10;

    private float m_timer = 0;
    private const float MAX_DURATION = 20;


    void Start()
    {
        //Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = m_camera.nearClipPlane;
        //Vector3 screenPosition = m_camera.ScreenToWorldPoint(mousePosition);
        //Vector3 direction = (screenPosition - transform.position).normalized;
        //
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(direction * m_projectileSpeed, ForceMode.Force);

        m_timer = MAX_DURATION;
    }

    void Update()
    {
        if (m_timer < 0)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }
        m_timer -= Time.deltaTime;

    }

    //useless
    [ClientRpc]
    public void SetTrajectoryRpc(Vector3 direction)
    {
        m_rb.AddForce(direction * m_projectileSpeed, ForceMode.Impulse);
        m_timer = MAX_DURATION;
    }

}
