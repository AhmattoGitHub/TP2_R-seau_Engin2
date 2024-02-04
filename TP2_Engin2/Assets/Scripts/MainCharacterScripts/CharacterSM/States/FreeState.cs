using UnityEngine;

public class FreeState : CharacterState
{
    private bool m_isMovingForward = false;
    private bool m_isMovingLateral = false;
    private bool m_isMovingBackward = false;    

    public override void OnEnter()
    {
        //Debug.Log("Entering FreeState");
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
        int layerMask = 1 << 8; // Ground layer devrait être à 8

        if (Physics.Raycast(m_stateMachine.MC.transform.position + new Vector3(0, 2, 0), Vector3.down, out hit, Mathf.Infinity, layerMask))
        {            
            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);                      

            if (groundAngle > m_stateMachine.SteepnessBeforeSlopeForce && groundAngle < 90)
            {
                // peut-être à changer pour un lerp?
                float slopeMultiplier = m_stateMachine.SlopeForceAngleMultiplier + (groundAngle / 90.0f); // À ajuster
                float forceMagnitude = m_stateMachine.SlopeForceMagnitude * slopeMultiplier * Mathf.Sin(Mathf.Deg2Rad * groundAngle);

                Vector3 slopeDirection = Vector3.ProjectOnPlane(-hit.normal, Vector3.up).normalized;
                Vector3 slopeForce = -slopeDirection * forceMagnitude;
                
                Debug.DrawRay(m_stateMachine.MC.transform.position + new Vector3(0, 2, 0), slopeForce, Color.blue);
                
                m_stateMachine.Rb.AddForce(slopeForce, ForceMode.Acceleration);
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
