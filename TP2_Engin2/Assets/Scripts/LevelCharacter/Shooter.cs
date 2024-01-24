using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : NetworkBehaviour
{
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private Camera m_camera;


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //Shoot();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = m_camera.nearClipPlane - 5;
            Vector3 screenPosition = m_camera.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (transform.position - screenPosition).normalized;



            ShootProjectileCmd(direction);
        }
    }

    //private void Shoot()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GameObject projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity);
    //        projectile.GetComponent<Projectile>().SetCamera(m_camera);
    //        NetworkServer.Spawn(projectile);
    //    }
    //}

    [Command(requiresAuthority = false)]
    public void ShootProjectileCmd(Vector3 direction)
    {
        Debug.Log(netId + "   " + Input.mousePosition);
        
        var projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(projectile);

        //Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = m_camera.nearClipPlane - 5;
        //Vector3 screenPosition = m_camera.ScreenToWorldPoint(mousePosition);
        //Vector3 direction = (transform.position - screenPosition).normalized;


        projectile.GetComponent<Projectile>().SetTrajectoryRpc(direction);

    }

}
