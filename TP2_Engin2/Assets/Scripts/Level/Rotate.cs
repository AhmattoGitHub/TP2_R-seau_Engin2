using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private Rigidbody m_rb;
    

    void FixedUpdate()
    {
        //transform.Rotate(0, m_rotationSpeed, 0);
        //return;
        
        Vector3 rot = m_rb.rotation.eulerAngles;
        rot.y += m_rotationSpeed;
        Quaternion quat = Quaternion.Euler(rot);
        
        m_rb.MoveRotation(quat);
    }
}
