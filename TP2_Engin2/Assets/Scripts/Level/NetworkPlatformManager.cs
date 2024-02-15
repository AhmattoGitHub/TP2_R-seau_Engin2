using Mirror;
using UnityEngine;

public class NetworkPlatformManager : NetworkBehaviour
{    
    [SerializeField] private GameObject m_platform = null;
    [SerializeField] private GameObject m_previewObject = null;
    [SerializeField] private GameObject m_previewAngleObject = null;    
    [SerializeField] private float m_rotationSpeed = 25.0f;
    [SerializeField] private float m_angleLimit = 15.0f;    
    [SerializeField] private float m_dampingSpeed = 10.0f;
    [SerializeField] private float m_pivotRadius = 0.01f;

    private Vector3 m_playersInputs = Vector3.zero;
    private Vector3 m_rotationAxis = Vector3.zero; 
    
    public static NetworkPlatformManager _Instance { get; private set; }

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
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), m_playersInputs * 7, Color.blue);

        m_playersInputs = m_playersInputs.normalized;
        //Debug.Log("PLAYER INPUT " + m_playersInputs);        
        
        if (m_playersInputs != Vector3.zero)
        {           
            m_rotationAxis = Quaternion.Euler(0, 90, 0) * m_playersInputs;   

            ApplyRotate(m_previewObject);            

            if (CalculatePreviewAngleFromPivot() >= m_angleLimit)
                return;

            ApplyRotate(m_platform);            
        }
        else 
        {                   
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, transform.up);
            m_platform.transform.rotation = Quaternion.Slerp(m_platform.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);            
        }       

        m_playersInputs = Vector3.zero;
    }
    
    private void ApplyRotate(GameObject gO)
    {
        Debug.DrawRay(transform.position, m_rotationAxis * 10, Color.magenta);

        if (gO == null) return;

        m_previewObject.transform.position = m_platform.transform.position;
        m_previewObject.transform.rotation = m_platform.transform.rotation;

        gO.transform.position = transform.position - (-transform.up * m_pivotRadius);        
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
    public void ReceivePlayersInputs(Vector3 inputs)
    {
        //Debug.Log("ReceiveWorldInputs " + worldInputs);        
        if (inputs == Vector3.zero)
        {
            m_playersInputs = Vector3.zero;
            return;
        }

        m_playersInputs += inputs; 
        // Le probleme de la plateforme qui reste pognee a son angle maximum se fait ici
        // Le statement if peut-etre commente et ca permet de retrouver le comportement de la plateforme qui reste pognee
        // Je ne sais pas si ce fix demeure bon pour le jeu en reseau avec des inputs de plus
        // d'un joueur, a tester
    }
}



/*
 
// TRYING TO CHANGE TO PARENT CHILDREN ONLY

 if (m_playersInputs == Vector3.zero)    
        {
            Debug.Log("no input");
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, m_previewObject.transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);            
        }

        m_playersInputs = Vector3.zero;

    
    private void ApplyRotate(GameObject gO)
    {
        Debug.DrawRay(transform.position, m_rotationAxis * 10, Color.magenta);

        if (gO == null) return;

        m_previewObject.transform.position = transform.position;
        m_previewObject.transform.rotation = transform.rotation;

        gO.transform.position = Vector3.zero - (-transform.up * m_pivotRadius);
        gO.transform.rotation = Quaternion.Euler(gO.transform.rotation.eulerAngles.x, 0f, gO.transform.rotation.eulerAngles.z);
        gO.transform.RotateAround(transform.position, m_rotationAxis, m_rotationSpeed * Time.deltaTime);
    }
 
 
 
 
 */

