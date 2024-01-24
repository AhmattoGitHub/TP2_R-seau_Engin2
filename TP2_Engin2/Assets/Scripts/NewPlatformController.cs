using UnityEngine;

public class NewPlatformController : MonoBehaviour
{    
    [SerializeField] private GameObject m_platform = null;
    [SerializeField] private GameObject m_previewObject = null;
    [SerializeField] private float m_rotationSpeed = 1.0f;
    [SerializeField] private float m_angleLimit = 15.0f;    
    [SerializeField] private float m_dampingSpeed = 10.0f;
    [SerializeField] private float m_pivotRadius = 0.01f;
    
    private float m_rotationX = 0.0f;
    private float m_rotationZ = 0.0f;

    private Vector2 m_worldInputs = Vector2.zero;

    private void Update()
    {
        m_worldInputs = m_worldInputs.normalized;

        if (m_worldInputs != Vector2.zero)
        {
            m_rotationX = m_worldInputs.y * m_rotationSpeed * Time.deltaTime;
            m_rotationZ = m_worldInputs.x * m_rotationSpeed * Time.deltaTime;

            // TODO remettre une limite d'angle pour la platfeforme
            PreviewApplyRotate(m_previewObject);
                        
            if (CalculatePreviewAngleFromPivot() >= m_angleLimit)
                return;

            Debug.Log("TestYO");

            ApplyRotate(m_platform);

        }

        // TODO Retour vers automatique vers le centre quand on ne reçoit pas d'input
        //Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.up);
        //m_platform.transform.rotation = Quaternion.Slerp(m_platform.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);
        
        m_worldInputs = Vector2.zero;

    }

    private void PreviewApplyRotate(GameObject gO)
    {
        if (gO == null) return;

        m_previewObject.transform.position = m_platform.transform.position;
        m_previewObject.transform.rotation = m_platform.transform.rotation;

        m_previewObject.transform.position = transform.position - (-transform.up * m_pivotRadius);
        m_previewObject.transform.RotateAround(transform.position, Vector3.right, m_rotationX);
        m_previewObject.transform.RotateAround(transform.position, Vector3.forward, m_rotationZ);
    }

    private void ApplyRotate(GameObject gO)
    {
        if (gO == null) return;

        //m_previewObject.transform.position = m_platform.transform.position;
        //m_previewObject.transform.rotation = m_platform.transform.rotation;

        m_platform.transform.position = transform.position - (-transform.up * m_pivotRadius);
        m_platform.transform.RotateAround(transform.position, Vector3.right, m_rotationX);
        m_platform.transform.RotateAround(transform.position, Vector3.forward, m_rotationZ);
    }

    private float CalculatePreviewAngleFromPivot()
    {       
        Vector3 pivotToObjectPreviewDir = transform.position - m_previewObject.transform.position;
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);
        //Debug.Log("Preview Angle is " + previewObjectToPivotDirAngle);        
        return previewObjectToPivotDirAngle;
    }

    public void ReceiveWorldInputs(Vector2 worldInputs)
    {
        m_worldInputs = worldInputs;
    }
}
