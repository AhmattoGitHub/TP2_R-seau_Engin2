using Mirror;
using UnityEngine;

public class Rotate : NetworkBehaviour
{
    [SerializeField] private float m_rotationSpeed;
    private float m_rotation = 0;    
    
    void FixedUpdate()
    {
        //transform.RotateAround(transform.position, Vector3.up, m_rotationSpeed);       

        transform.localRotation = Quaternion.Euler(0, m_rotation, 0);
        m_rotation += m_rotationSpeed;
    }
}
