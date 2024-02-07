using Mirror;
using UnityEngine;

public class NetworkLevelPlayerController : NetworkBehaviour
{
    private NetworkPlatformManager m_platformController = null;

    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_objectToLookAt;
    [SerializeField] private Vector2 m_verticalLimits;
    [SerializeField] private float m_startDistance = 5.0f;
    [SerializeField] private float m_lerpF = 0.1f;
    [SerializeField] private float m_rotationSpeed = 2.0f;
    [SerializeField] private float m_moveSpeed = 0.1f;
    [SerializeField] private float m_edgeDistance = 50.0f;
    
    private float m_lerpedAngleX;
    private float m_lerpedInputY;

    private bool m_inputPaused = false;

    void Start()
    {
        if (!isLocalPlayer)
        {
            m_camera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool controllingPlatform = false;
        
        if (m_platformController == null)
        {
            var platformController = NetworkPlatformManager._Instance?.GetComponent<NetworkPlatformManager>();
            if (platformController == null)
            {
                Debug.LogError("No platform controller in scene");
            }
            else
            {
                m_platformController = platformController;
                controllingPlatform = true;
            }
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))    //For easier testing    //Feature ?
        {
            m_inputPaused = !m_inputPaused;
        }

        if (m_inputPaused == true)
        {
            return;
        }
        MoveHorizontally();
        MoveVertically();

        if (controllingPlatform)
        {
            SendInputsToMovePlatform();
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

    private void SendInputsToMovePlatform()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        Vector3 localInput = GetLocalInput();

        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), localInput * 5, Color.green);

        CMD_SendWorldInputs(localInput);
    }

    [Command]
    public void CMD_SendWorldInputs(Vector3 worldInput)
    {
        m_platformController.ReceiveWorldInputs(worldInput);
    }

    Vector3 GetLocalInput() //private ?
    {
        Vector3 localInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            localInput += m_camera.transform.forward;
        if (Input.GetKey(KeyCode.S))
            localInput -= m_camera.transform.forward;
        if (Input.GetKey(KeyCode.D))
            localInput += m_camera.transform.right;
        if (Input.GetKey(KeyCode.A))
            localInput -= m_camera.transform.right;

        if (localInput == Vector3.zero)
        {
            //Debug.Log("Local Inputs = Zero");
            return Vector3.zero;
        }

        return localInput;
    }

}
