using UnityEngine;

public class PlatformControllerPivot : MonoBehaviour
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
    [SerializeField]
    private float m_dampingSpeed = 10.0f;
    [SerializeField]
    private GameObject m_platform = null;

    private Vector3 m_localForward = Vector3.zero;
    private Vector3 m_localRight = Vector3.zero;
    private float m_rotationX = 0.0f;
    private float m_rotationZ = 0.0f;

    // À delete
    public Camera south;
    public Camera east;
    public Camera north;
    public Camera west;
    public Camera southEast;
    public Camera southWest;
    public Camera northEast;
    public Camera northWest;

    private void Start()
    {
        // À delete
        south.enabled = true;
        east.enabled = false;
        north.enabled = false;
        west.enabled = false;
        southEast.enabled = false;
        southWest.enabled = false;
        northEast.enabled = false;
        northWest.enabled = false;
    }

    private void Update()
    {
        CameraChanges();

        Vector2 inputPlayerOne = InputPlayerOne().normalized;
        //Vector2 inputPlayerTwo = InputPlayerTwo().normalized;

        //Vector2 combinedInputs = inputPlayerOne + inputPlayerTwo;
        Vector2 combinedInputs = inputPlayerOne;
        //Vector2 combinedInputs = inputPlayerTwo;
        combinedInputs = combinedInputs.normalized; // If we want to increase more rapidly if same direction remove normalized

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)
            && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)
            && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, m_pivot.up);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, m_dampingSpeed * Time.deltaTime);
            return;
        }

        m_rotationX = combinedInputs.y * m_rotationSpeed * Time.deltaTime;
        m_rotationZ = combinedInputs.x * m_rotationSpeed * Time.deltaTime;

        ApplyRotate(m_previewObject);

        if (CalculatePreviewAngleFromPivot() >= m_angleLimit)
            return;

        ApplyRotate(gameObject);
    }

    private Vector2 InputPlayerOne()
    {
        Vector2 returnInput = Vector2.zero;
        Vector3 localInput = Vector3.zero;

        Camera activeCamera = GetActiveCamera();
        m_localForward = activeCamera.transform.TransformDirection(Vector3.forward);
        m_localRight = activeCamera.transform.TransformDirection(Vector3.right);

        if (Input.GetKey(KeyCode.W))
            localInput += m_localForward;
        if (Input.GetKey(KeyCode.S))
            localInput -= m_localForward;
        if (Input.GetKey(KeyCode.D))
            localInput -= m_localRight;
        if (Input.GetKey(KeyCode.A))
            localInput += m_localRight;

        float forwardThreshold = 0.5f;
        float rightThreshold = 0.5f;

        if (Mathf.Abs(m_localForward.x) < forwardThreshold && Mathf.Abs(m_localForward.z) > forwardThreshold
            && Mathf.Abs(m_localRight.x) > rightThreshold && Mathf.Abs(m_localRight.z) < rightThreshold)
        {
            // South
            Debug.Log("Entered 1");
            localInput += Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput += Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (Mathf.Abs(m_localForward.x) > forwardThreshold && Mathf.Abs(m_localForward.z) < forwardThreshold
            && Mathf.Abs(m_localRight.x) < rightThreshold && Mathf.Abs(m_localRight.z) > rightThreshold)
        {
            // East
            Debug.Log("Entered 2");
            localInput -= 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput -= 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (Mathf.Abs(m_localForward.x) < forwardThreshold && Mathf.Abs(m_localForward.z) < forwardThreshold
            && Mathf.Abs(m_localRight.x) > rightThreshold && Mathf.Abs(m_localRight.z) < rightThreshold)
        {
            // North
            Debug.Log("Entered 3");
            localInput += Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput += Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (Mathf.Abs(m_localForward.x) > forwardThreshold && Mathf.Abs(m_localForward.z) > forwardThreshold
            && Mathf.Abs(m_localRight.x) < rightThreshold && Mathf.Abs(m_localRight.z) < rightThreshold)
        {
            // West
            Debug.Log("Entered 4");
            localInput -= 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput -= 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else
        {
            Debug.Log("Did not enter anything");
        }

        returnInput.x = Vector3.Dot(localInput, Vector3.right);
        returnInput.y = Vector3.Dot(localInput, Vector3.forward);

        return returnInput;
    }

    private Vector2 InputPlayerTwo()
    {
        Vector2 returnInput = Vector2.zero;
        Vector3 localInput = Vector3.zero;

        Camera activeCamera = GetActiveCamera();
        m_localForward = activeCamera.transform.TransformDirection(Vector3.forward);
        m_localRight = activeCamera.transform.TransformDirection(Vector3.right);

        if (Input.GetKey(KeyCode.UpArrow))
            localInput += m_localForward;
        if (Input.GetKey(KeyCode.DownArrow))
            localInput -= m_localForward;
        if (Input.GetKey(KeyCode.RightArrow))
            localInput -= m_localRight;
        if (Input.GetKey(KeyCode.LeftArrow))
            localInput += m_localRight;

        /*
         
         Quaternion cameraRotationRelativeToPlatform = Quaternion.Inverse(platformTransform.rotation) * cameraTransform.rotation;

        // Convertissez les entrées locales en entrées du monde en fonction de la rotation de la caméra
        Vector3 worldInput = cameraRotationRelativeToPlatform * localInput;
        
        // Appliquez les entrées transformées à la plateforme
        // Vous pouvez ajuster cela en fonction de votre logique spécifique de transformation de la plateforme
        platformTransform.Translate(worldInput * Time.deltaTime * speed, Space.World);
         
         */

        //Quaternion cameraRotationRelativeToPlatform = Quaternion.Inverse(m_platform.transform.rotation) * activeCamera.transform.rotation;
        //
        //Vector3 worldInput = cameraRotationRelativeToPlatform * localInput;
        //
        //return worldInput;

        if (m_localForward.x < 0.5 && m_localForward.x > -0.5
            && m_localForward.z > 0.5
            && m_localRight.x > 0.5
            && m_localRight.z > -0.5 && m_localRight.z < 0.5)
        {
            // South
            Debug.Log("Entered 1");
            localInput += Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput += Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (m_localForward.x < -0.5
            && m_localForward.z < 0.5 && m_localForward.z > -0.5
            && m_localRight.x < 0.5 && m_localRight.x > -0.5
            && m_localRight.z > 0.5)
        {
            // East
            Debug.Log("Entered 2");
            localInput -= 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput -= 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (m_localForward.x > -0.5 && m_localForward.x < 0.5
            && m_localForward.z < 0.5
            && m_localRight.x < 0.5
            && m_localRight.z > -0.5 && m_localRight.z < 0.5)
        {
            // North
            Debug.Log("Entered 3");
            localInput += Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput += Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else if (m_localForward.x > 0.5
            && m_localForward.z > -0.5 && m_localForward.z < 0.5
            && m_localRight.x > -0.5 && m_localRight.x < 0.5
            && m_localRight.z < -0.5)
        {
            // West
            Debug.Log("Entered 4");
            localInput -= 2 * Vector3.Dot(localInput, m_localForward) * m_localForward;
            localInput -= 2 * Vector3.Dot(localInput, m_localRight) * m_localRight;
        }
        else
        {
            Debug.Log("Did not enter anything");
        }

        returnInput.x = Vector3.Dot(localInput, Vector3.right);
        returnInput.y = Vector3.Dot(localInput, Vector3.forward);

        return returnInput;
    }

    private float CalculatePreviewAngleFromPivot()
    {
        Vector3 pivotToObjectPreviewDir = gameObject.transform.position - m_previewObject.transform.position;
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);
        //Debug.Log("Angle with preview object " + previewObjectToPivotDirAngle);
        return previewObjectToPivotDirAngle;
    }

    private void ApplyRotate(GameObject gO)
    {
        if (gO == null) return;

        m_previewObject.transform.position = transform.position;
        m_previewObject.transform.rotation = transform.rotation;

        gO.transform.position = m_pivot.position - (-transform.up * m_pivotRadius);
        gO.transform.RotateAround(m_pivot.position, Vector3.right, m_rotationX);
        gO.transform.RotateAround(m_pivot.position, Vector3.forward, m_rotationZ);
    }

    private void CameraChanges()
    {
        // À delete
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCamera(south);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToCamera(east);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToCamera(north);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchToCamera(west);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchToCamera(southEast);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchToCamera(southWest);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchToCamera(northEast);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SwitchToCamera(northWest);
        }
    }

    void SwitchToCamera(Camera targetCamera)
    {
        // À delete
        south.enabled = false;
        east.enabled = false;
        north.enabled = false;
        west.enabled = false;
        southEast.enabled = false;
        southWest.enabled = false;
        northEast.enabled = false;
        northWest.enabled = false;

        targetCamera.enabled = true;
    }

    Camera GetActiveCamera()
    {
        // À delete
        if (south.enabled)
            return south;
        else if (east.enabled)
            return east;
        else if (north.enabled)
            return north;
        else if (west.enabled)
            return west;
        else if (southEast.enabled)
            return southEast;
        else if (southWest.enabled)
            return southWest;
        else if (northEast.enabled)
            return northEast;
        else if (northWest.enabled)
            return northWest;
        else
            return null;
    }
}

