using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMoveAround : MonoBehaviour
{
    [SerializeField] private float m_rotSpeed = 1;
    [SerializeField] private float m_moveSpeed = 3;

    private float m_timer = 0;

    void Update()
    {
        transform.Rotate(new Vector3(m_rotSpeed, 0, 0));
        transform.Translate(new Vector3(m_moveSpeed, 0, 0));

        if (m_timer > 1.0f)
        {
            m_timer = 0.0f;
            m_moveSpeed *= -1.0f;
        }
        m_timer += Time.deltaTime;
    }
}
