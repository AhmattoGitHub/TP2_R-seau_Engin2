using UnityEngine;

public class PlatformControllerOne : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private float m_angleLimits = 15.0f;

    private float m_xMax = 0.0f;
    private float m_xMin = 0.0f;
    private float m_zMax = 0.0f;
    private float m_zMin = 0.0f;

    //[field: Header("ANGLE LIMITS")]
    //[SerializeField] private float m_xMax =  15.0f; 
    //[SerializeField] private float m_xMin = -15.0f;
    //[SerializeField] private float m_zMax =  15.0f; 
    //[SerializeField] private float m_zMin = -15.0f;

    private void Start()
    {
        m_xMax = m_angleLimits;
        m_xMin = -m_angleLimits;
        m_zMax = m_angleLimits;
        m_zMin = -m_angleLimits;
    }

    private void Update()
    {
        // TODO Maybe add lerp to smooth movement
        // TODO Maybe add so there's always movement, like if your are stuck in south east corner
        //      and inputing movement to go east, platform would slowly move to east even though that means
        //      that you need to add automatically inputs to make sure it's possible
        //      So like the main direction are N, S, E, W and not in betweens, if that makes sense
        // TODO Maybe add an automatic return to 0 degree angle if no input is detected

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

        rotationX += ClampAngle(transform.rotation.eulerAngles.x);
        rotationZ += ClampAngle(transform.rotation.eulerAngles.z);
        
        rotationX = Mathf.Clamp(rotationX, m_xMin, m_xMax);
        rotationZ = Mathf.Clamp(rotationZ, m_zMin, m_zMax);
        
        transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, rotationZ);
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
