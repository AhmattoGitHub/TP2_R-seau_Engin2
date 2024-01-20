using UnityEngine;

public class PlatformControllerTwo : MonoBehaviour
{
    [SerializeField]
    private Transform m_pivot = null;    
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private float m_angleLimit = 15.0f;
    [SerializeField]
    private float m_pivotRadius = 10.0f;

    private Vector3 m_lastPosition = Vector3.zero;
    private Vector3 m_lastRotation = Vector3.zero;
    
    private void Update()
    {
        // https://gamedevbeginner.com/how-to-rotate-in-unity-complete-beginners-guide/#rotate_towards_object

        float inputXPositive = 0.0f;
        float inputXNegative = 0.0f;
        float inputZPositive = 0.0f;
        float inputZNegative = 0.0f;

        // First user inputs
        if (Input.GetKey(KeyCode.W))
            inputXPositive = 1.0f;
        if (Input.GetKey(KeyCode.S))
            inputXNegative = -1.0f;
        if (Input.GetKey(KeyCode.D))
            inputZNegative = -1.0f;
        if (Input.GetKey(KeyCode.A))
            inputZPositive = 1.0f;

        // Second user inputs
        if (Input.GetKey(KeyCode.UpArrow))
            inputXPositive = 1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            inputXNegative = -1.0f;
        if (Input.GetKey(KeyCode.RightArrow))
            inputZNegative = -1.0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            inputZPositive = 1.0f;

        float rotationX = (inputXPositive + inputXNegative) * m_rotationSpeed * Time.deltaTime;
        float rotationZ = (inputZPositive + inputZNegative) * m_rotationSpeed * Time.deltaTime;

        Vector3 pivotDir = m_pivot.position - transform.position;        
        float angleToPivotDir = Vector3.Angle(-Vector3.up, pivotDir);       

        if (angleToPivotDir <= m_angleLimit)
        {
            m_lastPosition = transform.position;
            m_lastRotation = transform.rotation.eulerAngles;

            transform.position = m_pivot.position - (-transform.up * m_pivotRadius);            
            transform.RotateAround(m_pivot.position, Vector3.right, rotationX);
            transform.RotateAround(m_pivot.position, Vector3.forward, rotationZ);            
        }
        else
        {
            transform.position = m_lastPosition;
            transform.rotation = Quaternion.Euler(m_lastRotation);     
        }
    }
}
