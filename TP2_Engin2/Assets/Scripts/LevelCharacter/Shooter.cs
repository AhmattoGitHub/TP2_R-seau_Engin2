using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EProjectileType
{
    Bullet,
    Bomb
};

public class Shooter : NetworkBehaviour
{
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private GameObject m_bombPrefab;
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_bulletCooldownTimer = 0;
    [SerializeField] private float m_bombCooldownTimer = 0;
    [SerializeField] private float m_bulletCooldownTimerMax = 2;
    [SerializeField] private float m_bombCooldownTimerMax = 5;

    private EProjectileType m_currentProjectile;


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = m_camera.nearClipPlane - 5;
            Vector3 screenPosition = m_camera.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (transform.position - screenPosition).normalized;

            switch (m_currentProjectile)
            {
                case EProjectileType.Bullet:
                    if (m_bulletCooldownTimer < 0)
                    {
                        CMD_ShootBullet(direction);
                        m_bulletCooldownTimer = m_bulletCooldownTimerMax;
                    }
                    break;
                case EProjectileType.Bomb:
                    if (m_bombCooldownTimer < 0)
                    {
                        //CMD_ShootBomb(direction);
                        CMD_ShootBomb(direction);
                        m_bombCooldownTimer = m_bombCooldownTimerMax;
                    }
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_currentProjectile = EProjectileType.Bullet;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_currentProjectile = EProjectileType.Bomb;
        }

        m_bulletCooldownTimer -= Time.deltaTime;
        m_bombCooldownTimer -= Time.deltaTime;

    }

    [Command(requiresAuthority = false)]
    public void CMD_ShootBullet(Vector3 direction)
    {
        var bullet = Instantiate(m_bulletPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bullet);

        bullet.GetComponent<Bullet>().CMD_Shoot(direction);
    }

    [Command(requiresAuthority = false)]
    public void CMD_ShootBomb(Vector3 direction)
    {
        var bomb = Instantiate(m_bombPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bomb);

        bomb.GetComponent<BombNetwork>().CMD_Shoot(direction);
    }

    public void SetCamera(Camera camera)
    {
        m_camera = camera;
    }
}
