using UnityEngine;

public class PlatformControllerFive : MonoBehaviour
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

    private Vector3 m_localForward = Vector3.zero;
    private Vector3 m_localRight = Vector3.zero;
    private float m_rotationX = 0.0f;
    private float m_rotationZ = 0.0f;



    // À delete
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public Camera camera4;

    private void Start()
    {
        // À delete
        camera1.enabled = true;
        camera2.enabled = false;
        camera3.enabled = false;
        camera4.enabled = false;
    }

    private void Update()
    {
        CameraChanges();

        Camera activeCamera = GetActiveCamera();

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

        m_localForward = activeCamera.transform.TransformDirection(Vector3.forward);
        m_localRight = activeCamera.transform.TransformDirection(Vector3.right);

        m_rotationX = (inputXPositive + inputXNegative) * m_rotationSpeed * Time.deltaTime;
        m_rotationZ = (inputZPositive + inputZNegative) * m_rotationSpeed * Time.deltaTime;

        ApplyRotate(m_previewObject);

        if (CalculateAngleFromPivot() >= m_angleLimit)
            return;

        ApplyRotate(gameObject);
    }

    private float CalculateAngleFromPivot()
    {
        Vector3 pivotToObjectPreviewDir = m_pivot.position - m_previewObject.transform.position;
        float previewObjectToPivotDirAngle = Vector3.Angle(-Vector3.up, pivotToObjectPreviewDir);
        // Retirer le commentaire
        Debug.Log("Angle with preview object " + previewObjectToPivotDirAngle);
        return previewObjectToPivotDirAngle;
    }

    private void ApplyRotate(GameObject gameObject)
    {
        if (gameObject == null) return;

        m_previewObject.transform.position = transform.position;
        m_previewObject.transform.rotation = transform.rotation;

        gameObject.transform.position = m_pivot.position - (-transform.up * m_pivotRadius);
        gameObject.transform.RotateAround(m_pivot.position, m_localRight, m_rotationX);
        gameObject.transform.RotateAround(m_pivot.position, m_localForward, m_rotationZ);
    }

    private void CameraChanges()
    {
        // À delete
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCamera(camera1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToCamera(camera2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToCamera(camera3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchToCamera(camera4);
        }
    }

    void SwitchToCamera(Camera targetCamera)
    {
        // À delete
        camera1.enabled = false;
        camera2.enabled = false;
        camera3.enabled = false;
        camera4.enabled = false;

        targetCamera.enabled = true;
    }

    Camera GetActiveCamera()
    {
        // À delete
        if (camera1.enabled)
            return camera1;
        else if (camera2.enabled)
            return camera2;
        else if (camera3.enabled)
            return camera3;
        else if (camera4.enabled)
            return camera4;
        else
            return null;
    }


}

