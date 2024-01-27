using UnityEngine;
using Mirror;

public class NetworkLevelPlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkPlatformController m_platformController = null;    
    [SerializeField] private Camera m_camera;    

    private Vector3 m_localForward = Vector3.zero;
    private Vector3 m_localRight = Vector3.zero;

    //[SerializeField] private Transform positionSouth;
    //[SerializeField] private Transform positionWest;
    //[SerializeField] private Transform positionEast;
    //[SerializeField] private Transform positionNorth;

    //private enum CameraPosition
    //{
    //    South,
    //    West,
    //    East,
    //    North,
    //    Count
    //}

    //private CameraPosition currentCameraPosition = CameraPosition.South;

    private void Awake()
    {
        //transform.position = positionSouth.position;
        //transform.rotation = positionSouth.rotation;        
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        
        
        if (m_platformController == null) 
        {
            m_platformController = NetworkPlatformController._Instance.GetComponent<NetworkPlatformController>();            
        }

        // TODO remove switch camera feature, only for testing purposes
        //if (Input.GetKeyDown(KeyCode.Q))
            //SwitchCameraPosition();

        Vector3 localInput = GetLocalInput();

        Debug.DrawRay(transform.position + new Vector3(0,1,0), localInput * 5, Color.green);


        //if (localInput == Vector3.zero)
        //{
        //    Debug.Log("Local input is 0");
        //    return;
        //}

        //Vector2 worldInputs = TransformLocalInputToWorld(localInput);
        
        CMD_SendWorldInputs(localInput);       
    }

    [Command]
    public void CMD_SendWorldInputs(Vector3 worldInput)
    {
        m_platformController.ReceiveWorldInputs(worldInput);
    }

    Vector3 GetLocalInput()
    {
        Vector3 localInput = Vector3.zero;

        //m_localForward = m_camera.transform.TransformDirection(Vector3.forward);
        //m_localRight = m_camera.transform.TransformDirection(Vector3.right);

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
            Debug.Log("Local Inputs = Zero");
            return Vector3.zero;
        }

        return localInput;
    }

    //Vector3 GetLocalInput()
    //{
    //    Vector3 localInput = Vector3.zero;
    //
    //    m_localForward = m_camera.transform.TransformDirection(Vector3.forward);
    //    m_localRight = m_camera.transform.TransformDirection(Vector3.right);
    //
    //    if (Input.GetKey(KeyCode.W))
    //        localInput += m_localForward;
    //    if (Input.GetKey(KeyCode.S))
    //        localInput -= m_localForward;
    //    if (Input.GetKey(KeyCode.D))
    //        localInput -= m_localRight;
    //    if (Input.GetKey(KeyCode.A))
    //        localInput += m_localRight;
    //
    //    if (localInput == Vector3.zero)
    //    {
    //        Debug.Log("Local Inputs = Zero");
    //        return Vector2.zero;
    //    }
    //
    //    return localInput;
    //}

    Vector2 TransformLocalInputToWorld(Vector3 localInput)
    {
        Vector2 returnWorldInput = Vector2.zero;

        float forwardThreshold = 0.5f;
        float rightThreshold = 0.5f;

        if (Mathf.Abs(m_localForward.x) < forwardThreshold && Mathf.Abs(m_localForward.z) > forwardThreshold
            && Mathf.Abs(m_localRight.x) > rightThreshold && Mathf.Abs(m_localRight.z) < rightThreshold)
        {
            // South
            Debug.Log("Entered 1");
            localInput += 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput += 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (Mathf.Abs(m_localForward.x) > forwardThreshold && Mathf.Abs(m_localForward.z) < forwardThreshold
            && Mathf.Abs(m_localRight.x) < rightThreshold && Mathf.Abs(m_localRight.z) > rightThreshold)
        {
            // East
            Debug.Log("Entered 2");
            localInput -= 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput -= 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }         
        else
        {
            Debug.Log("Did not enter anything");
        }

        returnWorldInput.x = Vector3.Dot(localInput, Vector3.right);
        returnWorldInput.y = Vector3.Dot(localInput, Vector3.forward);

        return returnWorldInput;
    }    

    /*private void SwitchCameraPosition()
    {
        currentCameraPosition = (CameraPosition)(((int)currentCameraPosition + 1) % (int)CameraPosition.Count);

        switch (currentCameraPosition)
        {
            case CameraPosition.South:
                transform.position = positionSouth.position;
                transform.rotation = positionSouth.rotation;
                break;
            case CameraPosition.West:
                transform.position = positionWest.position;
                transform.rotation = positionWest.rotation;
                break;
            case CameraPosition.East:
                transform.position = positionEast.position;
                transform.rotation = positionEast.rotation;
                break;
            case CameraPosition.North:
                transform.position = positionNorth.position;
                transform.rotation = positionNorth.rotation;
                break;
            default:
                break;
        }
    }*/
}





/*
 
 prendre le m_camera.transform.forward 
 
 
 
 Quand on recoit un input

    W = cam.transf.forward
    S = -cam.transf.forward
    A = -cam.transf.right
    D = cam.transf.right
 
 
 
 
 
 
 
 
 
 
 
 
 */