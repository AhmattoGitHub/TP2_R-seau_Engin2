using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestRotate : MonoBehaviour
{
    [SerializeField] private float m_speed = 1;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, m_speed, 0));
    }
}
