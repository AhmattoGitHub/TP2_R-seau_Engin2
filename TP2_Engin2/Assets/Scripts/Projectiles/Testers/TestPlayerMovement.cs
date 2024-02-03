using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform m_objectToLookAt;
    [SerializeField] private Vector2 m_verticalLimits;
    [SerializeField] private float m_startDistance = 5.0f;
    [SerializeField] private float m_lerpF = 0.1f;
    [SerializeField] private float m_rotationSpeed = 2.0f;
    [SerializeField] private float m_moveSpeed = 0.1f;
    [SerializeField] private float m_edgeDistance = 50.0f;
    [SerializeField] private float m_scrollSpeed = 5.0f;

    private float m_lerpedAngleX;
    private float m_lerpedInputY;

    private bool m_cannotMove = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_cannotMove = !m_cannotMove;
        }

        if (!m_cannotMove)
        {
            MoveHorizontally();
            MoveVertically();
        }

        float mouseInput = Input.mouseScrollDelta.y * m_scrollSpeed;

        if (mouseInput > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_objectToLookAt.position, 1);
        }
        if (mouseInput < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_objectToLookAt.position, -1);
        }

    }

    private void MoveHorizontally()
    {
        float currentAngleX = 0;

        if (Input.mousePosition.x >= Screen.width - m_edgeDistance)
        {
            currentAngleX -= m_rotationSpeed;
        }
        if (Input.mousePosition.x <= 0 + m_edgeDistance)
        {
            currentAngleX += m_rotationSpeed;
        }

        m_lerpedAngleX = Mathf.Lerp(m_lerpedAngleX, currentAngleX, m_lerpF);
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, m_lerpedAngleX);
    }

    private void MoveVertically()
    {
        float inputY = 0;

        if (Input.mousePosition.y >= Screen.height - m_edgeDistance &&
            transform.position.y <= m_verticalLimits.y)
        {
            inputY += m_moveSpeed;
        }
        if (Input.mousePosition.y <= 0 + m_edgeDistance &&
            transform.position.y >= m_verticalLimits.x)
        {
            inputY -= m_moveSpeed;
        }

        m_lerpedInputY = Mathf.Lerp(m_lerpedInputY, inputY, m_lerpF);
        transform.position += new Vector3(0, m_lerpedInputY, 0);
    }
}
