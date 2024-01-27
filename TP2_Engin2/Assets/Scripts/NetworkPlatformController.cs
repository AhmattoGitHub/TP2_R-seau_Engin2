using Mirror;
using UnityEngine;
using UnityEngine.UIElements;

public class NetworkPlatformController : NetworkBehaviour
{    
    [SerializeField] private GameObject m_platform = null;
    [SerializeField] private GameObject m_previewObject = null;
    [SerializeField] private GameObject m_previewAngleObject = null;    
    [SerializeField] private float m_rotationSpeed = 25.0f;
    [SerializeField] private float m_angleLimit = 15.0f;    
    [SerializeField] private float m_dampingSpeed = 10.0f;
    [SerializeField] private float m_pivotRadius = 0.01f;

    private Vector3 m_worldInputs = Vector3.zero;
    private Vector3 m_rotationAxis = Vector3.zero;
    private float m_rotationX = 0.0f;
    private float m_rotationZ = 0.0f;

    public static NetworkPlatformController _Instance { get; private set; }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        _Instance = this;
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            ServerUpdate();
        }
    }

    [Server]
    private void ServerUpdate()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), m_worldInputs * 7, Color.blue);

        m_worldInputs = m_worldInputs.normalized;

        if (m_worldInputs != Vector3.zero)
        {
            Debug.Log("inputs");
            //m_rotationAxis soit m_worldInputs, mais avec une rotation de 90 degré
            m_rotationAxis = Quaternion.Euler(0, 90, 0) * m_worldInputs;
            
            //m_rotationX = m_worldInputs.y * m_rotationSpeed * Time.deltaTime;
            //m_rotationZ = m_worldInputs.x * m_rotationSpeed * Time.deltaTime;
            //m_rotationX = m_worldInputs.z * m_rotationSpeed * Time.deltaTime;
            //m_rotationZ = m_worldInputs.x * m_rotationSpeed * Time.deltaTime;

            ApplyRotate(m_previewObject);

            if (CalculatePreviewAngleFromPivot() >= m_angleLimit)
                return;

            ApplyRotate(m_platform);

            //hypothèse
            //if (CalculatePreviewAngleFromPivot() <= m_angleLimit)
            //    ApplyRotate(m_platform);
            //
            //ApplyRotate(m_platform); // à enlever
        }

         //TODO Not working perfectly, stays stuck at max angle if no input
        //if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A)
        //    && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))    
        //{
        //    Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.up);
        //    m_platform.transform.rotation = Quaternion.Slerp(m_platform.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);            
        //}

        if (m_worldInputs == Vector3.zero)    
        {
            Debug.Log("no input");
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.up);
            m_platform.transform.rotation = Quaternion.Slerp(m_platform.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);            
        }

        m_worldInputs = Vector3.zero;
    }
    
    private void ApplyRotate(GameObject gO)
    {
        Debug.DrawRay(transform.position, m_rotationAxis * 10, Color.magenta);

        if (gO == null) return;

        m_previewObject.transform.position = m_platform.transform.position;
        m_previewObject.transform.rotation = m_platform.transform.rotation;

        gO.transform.position = transform.position - (-transform.up * m_pivotRadius);
        //gO.transform.RotateAround(transform.position, Vector3.right, m_rotationX);
        //gO.transform.RotateAround(transform.position, Vector3.forward, m_rotationZ);
        gO.transform.RotateAround(transform.position, m_rotationAxis, m_rotationSpeed * Time.deltaTime);
    }
    
    private float CalculatePreviewAngleFromPivot()
    {
        Vector3 pivotToObjectPreviewDir = transform.position - m_previewAngleObject.transform.TransformPoint(Vector3.zero);       
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);
        //Debug.Log("Preview Angle is " + previewObjectToPivotDirAngle);        
        return previewObjectToPivotDirAngle;
    }

    [Server]
    public void ReceiveWorldInputs(Vector3 worldInputs)
    {
        m_worldInputs += worldInputs;
    }
}
