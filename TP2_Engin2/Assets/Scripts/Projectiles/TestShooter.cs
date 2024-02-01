using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private Camera m_camera;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = m_camera.nearClipPlane - 5;
            Vector3 screenPosition = m_camera.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (transform.position - screenPosition).normalized;

            var projectile = Instantiate(m_bulletPrefab, transform.position, Quaternion.identity);

            //projectile.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
            projectile.GetComponent<Bomb>().Shoot(direction);
        }
    }
}
