using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingArmAddForce : MonoBehaviour
{
    [SerializeField] private float m_impulseForce;
    [SerializeField] private Transform m_tower;

    //void OnCollisionEnter(Collision collision)
    //{
    //    //if (!isServer)
    //    //{
    //    //    return;
    //    //}
    //    Debug.Log("coll");
    //    var runner = collision.gameObject.GetComponent<Rigidbody>();
    //    //var runner = collision.gameObject.GetComponentInChildren<RunnerSM>();
    //    Debug.Log(collision.gameObject.name);
    //    if (runner != null)
    //    {
    //        Debug.Log("addforce");
    //
    //        runner.AddForce(m_tower.right * m_impulseForce, ForceMode.Impulse);
    //    }
    //
    //    
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (!isServer)
        //{
        //    return;
        //}
        //var runner = other.gameObject.GetComponent<Rigidbody>();
        var runner = other.gameObject.GetComponentInChildren<RunnerSM>();
        Debug.Log(other.gameObject.name);
        if (runner != null)
        {
            Debug.Log("addforce");

            //runner.Rb.AddForce(m_tower.right * m_impulseForce, ForceMode.Impulse);
            runner.AddImpulseForce(m_tower.right, m_impulseForce);
        }

    }
}
