using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private Transform m_pivot = null;
    [SerializeField]
    private GameObject m_previewObject = null;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private float m_angleLimit = 15.0f;
    [SerializeField]
    private float m_pivotRadius = 10.0f;

    private void Update()
    {        
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

        if (inputXPositive == 0 && inputXNegative == 0 && inputZPositive == 0 && inputZNegative == 0)
            return;

        float rotationX = (inputXPositive + inputXNegative) * m_rotationSpeed * Time.deltaTime;
        float rotationZ = (inputZPositive + inputZNegative) * m_rotationSpeed * Time.deltaTime;
                
        m_previewObject.transform.position = transform.position;
        m_previewObject.transform.rotation = transform.rotation;

        m_previewObject.transform.position = m_pivot.position - (-transform.up * m_pivotRadius);
        m_previewObject.transform.RotateAround(m_pivot.position, Vector3.right, rotationX);
        m_previewObject.transform.RotateAround(m_pivot.position, Vector3.forward, rotationZ);

        Vector3 pivotToObjectPreviewDir = m_pivot.position - m_previewObject.transform.position;
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);

        if (previewObjectToPivotDirAngle >= m_angleLimit)
            return;

        transform.position = m_pivot.position - (-transform.up * m_pivotRadius);
        transform.RotateAround(m_pivot.position, Vector3.right, rotationX);
        transform.RotateAround(m_pivot.position, Vector3.forward, rotationZ);
    }}
