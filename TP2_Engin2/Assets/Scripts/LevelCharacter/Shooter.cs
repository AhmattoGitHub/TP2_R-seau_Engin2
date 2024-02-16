using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EProjectileType
{
    Bomb,
    BigBomb
};

public class Shooter : NetworkBehaviour
{
    [SerializeField] private GameObject m_bombPrefab;
    [SerializeField] private GameObject m_bigBombPrefab;
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_bombCooldownTimer = 0;
    [SerializeField] private float m_bombCooldownTimerMax = 2;

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
                case EProjectileType.Bomb:
                    if (m_bombCooldownTimer < 0)
                    {
                        CMD_ShootBomb(direction);
                        m_bombCooldownTimer = m_bombCooldownTimerMax;
                    }
                    break;
                case EProjectileType.BigBomb:
                    if (NetManagerCustom._Instance.MatchManager.GetPermissionToShoot())
                    {
                        CMD_ShootBigBomb(direction);
                    }
                    else
                    {
                        Debug.Log("Can't shoot !");
                    }
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_currentProjectile = EProjectileType.Bomb;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_currentProjectile = EProjectileType.BigBomb;
        }

        m_bombCooldownTimer -= Time.deltaTime;
    }

    [Command(requiresAuthority = false)]
    public void CMD_ShootBomb(Vector3 direction)
    {
        var bomb = Instantiate(m_bombPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bomb);

        bomb.GetComponent<BombNetwork>().CMD_Shoot(direction);
    }
    
    [Command(requiresAuthority = false)]
    public void CMD_ShootBigBomb(Vector3 direction)
    {
        var bigBomb = Instantiate(m_bigBombPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bigBomb);

        bigBomb.GetComponent<BombNetwork>().CMD_Shoot(direction);
    }

    public void SetCamera(Camera camera)
    {
        m_camera = camera;
    }

    /*
    Va falloir genre te faire quelque chose comme :

    for each playerShooter in playersList
    if isLocalPlayer
    shooter.GetComponentInChildren<Shooter>().GetBulletRemainingPercentage()
    */
    public float GetBulletRemainingPercentage()
    {
        if (m_bombCooldownTimer < 0)
        {
            return 0.0f;
        }
        return m_bombCooldownTimer / m_bombCooldownTimerMax;
    }

}
