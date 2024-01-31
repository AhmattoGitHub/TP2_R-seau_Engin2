using Mirror;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CameraController_LevelPlayer : MonoBehaviour       // Level
{

    [SerializeField] private Transform m_objectToLookAt;
    private Vector3 m_targetPosition;
    [SerializeField] private float m_startDistance = 5.0f;
    private float m_targetDistance;
    private float m_lerpedDistance;
    [SerializeField] private float m_scrollSpeed = 0.5f;
    [SerializeField] private float m_lerpF = 0.1f;
    [SerializeField] private float m_rotationSpeed = 5.0f;
    private float m_lerpedAngleX;
    private float m_lerpedAngleY;
    [SerializeField] private Vector2 m_clampingXRotationValues;
    [SerializeField] private Vector2 m_clampingCameraDistance;

    [SerializeField] private float m_edgeDistance = 10.0f;  //new


    // Start is called before the first frame update
    void Start()
    {
        //if (!isLocalPlayer)
        //{
        //    gameObject.SetActive(false);
        //}
        m_targetDistance = m_startDistance;
    }

    private void FixedUpdate()
    {
        CalculateDistance();
        CalculateTargetPosition();

        RotateAroundObjectHorizontal();   //new
        //RotateAroundObjectVertical();

        transform.position = Vector3.Lerp(transform.position, m_targetPosition, m_lerpF);

        HardSetCameraZRotation();
        MoveCameraInFrontOfObstructionsFUpdate();
    }
    void CalculateDistance()
    {
        float mouseInput = Input.mouseScrollDelta.y * m_scrollSpeed;

        if ((mouseInput < 0 && m_targetDistance > m_clampingCameraDistance.x) ||
            (mouseInput > 0 && m_targetDistance < m_clampingCameraDistance.y))
        {
            m_targetDistance += mouseInput;
        }

        m_lerpedDistance = Mathf.Lerp(m_lerpedDistance, m_targetDistance, m_lerpF);
    }

    void CalculateTargetPosition()
    {
        Vector3 CameraForwardVec = transform.forward;
        CameraForwardVec.Normalize();
        Vector3 desiredCameraOffset = CameraForwardVec * m_lerpedDistance;

        m_targetPosition = m_objectToLookAt.position - desiredCameraOffset;
    }

    void RotateAroundObjectHorizontal()
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

    void RotateAroundObjectVertical()
    {
        //if (Input.mousePosition.y >= Screen.height - m_edgeDistance ||
        //    Input.mousePosition.y <= 0 + m_edgeDistance)
        //{
        //    return;
        //}



        //Diff�rence de l'angle � chaque frame
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        //Valeur de mon transform
        var xRotationValue = transform.rotation.eulerAngles.x;
        //R�sultat de ma rotation + diff�rence
        float comparisonAngle = xRotationValue + currentAngleY;

        //S'assure que l'angle n'est pas converti � 360 lorsqu'il atteint 0 (0 �tant � l'horizontal)
        //Permet d'avoir une limite du bas en valeur n�gative
        if (comparisonAngle > 180)
        {
            comparisonAngle -= 360;
        }
        //Early return si les valeurs de l'angle sortent de mon min,max (clamp)
        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x) ||
            (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }

        m_lerpedAngleY = Mathf.Lerp(m_lerpedAngleY, currentAngleY, m_lerpF);

        if (comparisonAngle > m_clampingXRotationValues.x && comparisonAngle < m_clampingXRotationValues.y)
        {
            transform.RotateAround(m_objectToLookAt.position, transform.right, m_lerpedAngleY);
        }
    }

    private void HardSetCameraZRotation()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0.0f);
    }

    void MoveCameraInFrontOfObstructionsFUpdate()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;

        Vector3 vDiff = transform.position - m_objectToLookAt.position;
        float distance = vDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vDiff, out hit, distance, layerMask))
        {
            //Objet d�tect�
            Debug.DrawRay(m_objectToLookAt.position, vDiff.normalized * hit.distance, Color.yellow);

            transform.SetPositionAndRotation(hit.point, transform.rotation);
        }
        else
        {
            //Objet non d�tect�
            Debug.DrawRay(m_objectToLookAt.position, vDiff, Color.white);
        }
    }

}