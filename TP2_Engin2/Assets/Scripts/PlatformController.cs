using Mirror;
using UnityEngine;

public class PlatformController : NetworkBehaviour
{    
    [SerializeField] private GameObject m_platform = null;
    [SerializeField] private GameObject m_previewObject = null;
    [SerializeField] private GameObject m_objectForAngle = null;    
    [SerializeField] private float m_rotationSpeed = 25.0f;
    [SerializeField] private float m_angleLimit = 15.0f;    
    [SerializeField] private float m_dampingSpeed = 10.0f;
    [SerializeField] private float m_pivotRadius = 0.01f;

    private Vector2 m_worldInputs = Vector2.zero;
    private float m_rotationX = 0.0f;
    private float m_rotationZ = 0.0f;        

    private void Update()
    {
        m_worldInputs = m_worldInputs.normalized;

        if (m_worldInputs != Vector2.zero)
        {
            m_rotationX = m_worldInputs.y * m_rotationSpeed * Time.deltaTime;
            m_rotationZ = m_worldInputs.x * m_rotationSpeed * Time.deltaTime;
            
            ApplyRotate(m_previewObject);

            if (CalculatePreviewAngleFromPivot() >= m_angleLimit)
                return;

            ApplyRotate(m_platform);
        }

        // TODO Retour vers automatique vers le centre quand on ne reçoit pas d'input
        //Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.up);
        //m_platform.transform.rotation = Quaternion.Slerp(m_platform.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);
        
        m_worldInputs = Vector2.zero;
    }    

    private void ApplyRotate(GameObject gO)
    {
        if (gO == null) return;

        m_previewObject.transform.position = m_platform.transform.position;
        m_previewObject.transform.rotation = m_platform.transform.rotation;

        gO.transform.position = transform.position - (-transform.up * m_pivotRadius);
        gO.transform.RotateAround(transform.position, Vector3.right, m_rotationX);
        gO.transform.RotateAround(transform.position, Vector3.forward, m_rotationZ);
    }

    private float CalculatePreviewAngleFromPivot()
    {
        Vector3 pivotToObjectPreviewDir = transform.position - m_objectForAngle.transform.TransformPoint(Vector3.zero);       
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);
        //Debug.Log("Preview Angle is " + previewObjectToPivotDirAngle);        
        return previewObjectToPivotDirAngle;
    }

    public void ReceiveWorldInputs(Vector2 worldInputs)
    {
        m_worldInputs = worldInputs;
    }
}
