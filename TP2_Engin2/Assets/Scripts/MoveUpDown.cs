using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    [SerializeField]
    private float m_upLimit;
    [SerializeField] 
    private float m_downLimit;
    [SerializeField]
    private float m_moveSpeed;
   
    private float m_worldUpPosition;
    private float m_currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
      
        m_moveSpeed = 10.0f;
        m_worldUpPosition = transform.position.y;
        m_currentSpeed = m_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < m_downLimit)
        {
            m_currentSpeed = -m_moveSpeed;
        }
        else if (transform.localPosition.y > m_upLimit) 
        { m_currentSpeed = m_moveSpeed; }
        m_worldUpPosition -= m_currentSpeed * Time.deltaTime;

        //Debug.Log(transform.localPosition.y);
        transform.position = new Vector3(transform.position.x, m_worldUpPosition, transform.position.z);
    }
}
