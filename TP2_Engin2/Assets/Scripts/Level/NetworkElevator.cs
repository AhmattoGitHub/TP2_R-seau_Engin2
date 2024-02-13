using UnityEngine;
using Mirror;

public class NetworkElevator : NetworkBehaviour
{
    [SerializeField] private GameObject m_elevator;
    [SerializeField] private Transform m_startPoint;
    [SerializeField] private Transform m_endPoint;
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_waitTime = 2.0f;

    private bool m_isMovingTowardsEnd = true;
    private bool m_isWaiting = false;
    private float m_timer = 0.0f;    

    private void Update()
    {
        if (isServer)
        {
            ServerUpdate();            
        }        
    }

    [Server]
    private void ServerUpdate()
    {
        if (m_isWaiting)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_waitTime)
            {
                m_timer = 0.0f;
                m_isWaiting = false;
            }
            return;
        }

        if (m_isMovingTowardsEnd)
        {            
            MoveTowards(m_endPoint.position);
            if (m_elevator.transform.position.Equals(m_endPoint.position))
            {
                m_isMovingTowardsEnd = false;
                m_isWaiting = true;
            }
        }
        else
        {            
            MoveTowards(m_startPoint.position);
            if (m_elevator.transform.position.Equals(m_startPoint.position))
            {
                m_isMovingTowardsEnd = true;
                m_isWaiting = true;
            }            
        }
    }

    [ClientRpc]
    private void MoveTowards(Vector3 position)
    {
        m_elevator.transform.position = Vector3.MoveTowards(m_elevator.transform.position, position, m_speed * Time.deltaTime);
    }

}
