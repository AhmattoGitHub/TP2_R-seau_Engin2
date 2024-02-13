using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{

    [SerializeField]
    private Transform m_objectToRotateAround;

    [SerializeField]
    private float m_rotationSpeed;

    private float m_angle;
    private float m_time = 4;
   
    // Start is called before the first frame update
    void Start()
    {
        m_angle = 0.0f;
        m_rotationSpeed = 90.0f;
        m_time = 4;
     
    }

    // Update is called once per frame
    void Update()
    {
       //m_time -= Time.deltaTime;
       
       m_angle = m_rotationSpeed * Time.deltaTime;
       transform.RotateAround(m_objectToRotateAround.position, m_objectToRotateAround.up, m_angle);
       
        
       
    }
}
