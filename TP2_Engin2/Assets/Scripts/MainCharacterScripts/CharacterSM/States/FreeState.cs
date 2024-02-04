using UnityEngine;

public class FreeState : CharacterState
{
    private bool m_isMovingForward = false;
    private bool m_isMovingLateral = false;
    private bool m_isMovingBackward = false;
    // PMM_ADDITION
    private float m_frictionTimer = 0.0f;
    private bool m_highFrictionEnabled = false;
    private bool m_highFrictionEnabledOnce = false;
    private CapsuleCollider m_capsuleCollider;

    public override void OnEnter()
    {
        //Debug.Log("Entering FreeState");

        // PMM_ADDITION 
        m_capsuleCollider = m_stateMachine.MC.GetComponent<CapsuleCollider>();
    }

    public override void OnFixedUpdate()
    {
        AddForceFromInputs();          

        ApplySlopeForce();
        
        CapMaximumSpeed();
    }

    void ApplySlopeForce()
    {
        RaycastHit hit;
        float slopeForceMagnitude = 1000.0f;
                
        if (Physics.Raycast(m_stateMachine.MC.transform.position, Vector3.down, out hit))
        {            
            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);

            Debug.Log("Angle " + groundAngle);
                        
            if (groundAngle > 0 && groundAngle < 90)
            {               
                float forceMagnitude = slopeForceMagnitude * Mathf.Sin(Mathf.Deg2Rad * groundAngle);
                                
                Vector3 slopeForce = -hit.normal * forceMagnitude;

                //Debug.Log("Slope Force Magnitude: " + slopeForce.magnitude);
                //Debug.Log("Slope Force Direction: " + slopeForce.normalized);
                //Debug.Log("Slopeforce" + slopeForce);
                m_stateMachine.Rb.AddForce(slopeForce, ForceMode.Force);
            }
        }
    }   

    private void AddForceFromInputs()
    {
        Vector2 inputs = Vector2.zero;

        m_isMovingForward = false;
        m_isMovingLateral = false;
        m_isMovingBackward = false;

        if (Input.GetKey(KeyCode.W))
        {
            inputs.y += 1;

            m_isMovingForward = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputs.y -= 1;

            m_isMovingBackward = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputs.x -= 1;

            m_isMovingLateral = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputs.x += 1;

            m_isMovingLateral = true;
        }

        inputs.Normalize();

        m_stateMachine.Rb.AddForce(inputs.y * m_stateMachine.ForwardVectorForPlayer * m_stateMachine.AccelerationValue,
                ForceMode.Acceleration);
        m_stateMachine.Rb.AddForce(inputs.x * m_stateMachine.RightVectorForPlayer * m_stateMachine.AccelerationValue,
                ForceMode.Acceleration);
    }

    private void CapMaximumSpeed()
    {
        if (m_isMovingForward)
        {
            if (m_isMovingLateral)
            {
                float diagonalMaxVelocity = (m_stateMachine.ForwardMaxVelocity + m_stateMachine.LateralMaxVelocity) / 2;

                if (m_stateMachine.Rb.velocity.magnitude < diagonalMaxVelocity)
                {
                    return;
                }
                m_stateMachine.Rb.velocity = Vector3.Normalize(m_stateMachine.Rb.velocity);
                m_stateMachine.Rb.velocity *= diagonalMaxVelocity;
                return;
            }
            if (m_stateMachine.Rb.velocity.magnitude < m_stateMachine.ForwardMaxVelocity)
            {
                return;
            }

            m_stateMachine.Rb.velocity = Vector3.Normalize(m_stateMachine.Rb.velocity);
            m_stateMachine.Rb.velocity *= m_stateMachine.ForwardMaxVelocity;
            return;
        }
        if (m_isMovingLateral)
        {
            if (m_isMovingBackward)
            {
                float diagonalMaxVelocity = (m_stateMachine.LateralMaxVelocity + m_stateMachine.BackwardMaxVelocity) / 2;

                if (m_stateMachine.Rb.velocity.magnitude < diagonalMaxVelocity)
                {
                    return;
                }
                m_stateMachine.Rb.velocity = Vector3.Normalize(m_stateMachine.Rb.velocity);
                m_stateMachine.Rb.velocity *= diagonalMaxVelocity;
                return;
            }
            if (m_stateMachine.Rb.velocity.magnitude < m_stateMachine.LateralMaxVelocity)
            {
                return;
            }
            m_stateMachine.Rb.velocity = Vector3.Normalize(m_stateMachine.Rb.velocity);
            m_stateMachine.Rb.velocity *= m_stateMachine.LateralMaxVelocity;
            return;
        }
        if (m_isMovingBackward)
        {
            if (m_stateMachine.Rb.velocity.magnitude < m_stateMachine.BackwardMaxVelocity)
            {
                return;
            }
            m_stateMachine.Rb.velocity = Vector3.Normalize(m_stateMachine.Rb.velocity);
            m_stateMachine.Rb.velocity *= m_stateMachine.BackwardMaxVelocity;
        }
        else
        {
            if (m_stateMachine.Rb.velocity.magnitude > 0)
            {
                m_stateMachine.Rb.velocity *= m_stateMachine.SlowingVelocity;
            }
        }
    }

    public override void OnUpdate()
    {
        SendAnimatorValuesToSM();
    }
    private void SendAnimatorValuesToSM()
    {
        float forwardComponent = Vector3.Dot(m_stateMachine.Rb.velocity, m_stateMachine.ForwardVectorForPlayer); 
        float lateralComponent = Vector3.Dot(m_stateMachine.Rb.velocity, m_stateMachine.RightVectorForPlayer); 
        m_stateMachine.UpdateAnimatorMovementValues(new Vector2(lateralComponent, forwardComponent)); 
    }

    public override void OnExit()
    {
        //Debug.Log("Exiting FreeState");
    }

    public override bool CanEnter(CC_IState currentState)
    {
        if (currentState is JumpState)
        {
            return m_stateMachine.IsInContactWithFloor();
        }
        return false;
    }
    public override bool CanExit()
    {
        return true;
    }

}
