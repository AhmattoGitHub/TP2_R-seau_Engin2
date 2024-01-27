using UnityEngine;

public class CameraShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject m_projectilePrefab;
    [SerializeField]
    private float m_projectileSpeed = 10.0f;

    private void Update()
    {        
        if (Input.GetMouseButtonDown(0)) // Left mouse button fire
        {            
            // https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                        
            Vector3 direction = (screenPosition - transform.position).normalized;

            GameObject projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity);
            
            projectile.GetComponent<Rigidbody>().velocity = direction * m_projectileSpeed;
        }
    }
}
