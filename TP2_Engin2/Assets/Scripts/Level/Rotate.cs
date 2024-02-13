using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed;
    

    void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.up, m_rotationSpeed);
    }
}
